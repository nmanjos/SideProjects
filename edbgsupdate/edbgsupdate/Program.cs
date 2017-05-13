
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;
using System.Collections;
using System.Threading;
using System.Runtime.InteropServices;



namespace edbgsupdate
{
    class Program
    {
        static bool exitSystem = false;

        #region Trap application termination
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }


        #endregion



        private static string FACTIONURL = "https://eddb.io/archive/v5/factions.json";
        private static string SYSTEMURL = "https://eddb.io/archive/v5/systems_populated.json";
        private static string[] SYSTEMFIELDS = { "id", "name", "population", "is_populated", "updated_at", "needs_permit", "power_id", "power_state_id", "primary_economy_id", "reserves_id", "security_id", "state_id", "govtype_id", "allegiance_id" };
        private static string[] FACTIONFIELDS = { "id", "name", "updated_at", "is_player_faction", "allegiance_id", "government_id", "home_system_id", "state_id" };
        private static string[] FHISTORYFIELDS = { "Datecycle", "fhInfluence", "fhAllegiance_id", "fhFaction_id", "fhGovtype_id", "fhState_id", "fhSystem_id" };
        private static string[] SHISTORYFIELDS = { "Datecycle", "shAllegiance_id", "shEconomy_id", "shFaction_id", "shGovtype_id", "shPower_id", "shPower_state_id", "shSecurity_id", "shState_id", "shSystem_id" };
        private static string myServerAddress = "192.168.1.30";
        private static string myDataBase = "EDBGS";
        private static string myUsername = "edbgs";
        private static string myPassword = "Grimlock+";

        public static string FactionUrl { get => FACTIONURL; set => FACTIONURL = value; }
        public static string SystemUrl { get => SYSTEMURL; set => SYSTEMURL = value; }


        private static bool Handler(CtrlType sig)
        {
            Console.WriteLine("Exiting system due to external CTRL-C, or process kill, or shutdown");

            //do your cleanup here
            if (EDDNListener.Response.Count() > 0)
            {
                WriteJsonFile(EDDNListener.Getbackup().ToArray());
            }

            Console.WriteLine("Cleanup complete");

            //allow main to run off
            exitSystem = true;

            //shutdown right away so there are no lingering threads
            Environment.Exit(-1);

            return true;
        }

        public static void WriteJsonFile(string[] dados)
        {

            FileStream fs = new FileStream(@"backup.jsonl", FileMode.Append, FileAccess.Write);

            StreamWriter writer = new StreamWriter(fs);
            for (int i = 0; i < dados.Length; i++)
            {
                writer.WriteLine(dados[i]);
            }

            writer.Close();
            fs.Close();
        }

        private static string Wget(string fileurl)
        {
            Console.WriteLine("Opening Json File");
            Stream stream = new WebClient().OpenRead(fileurl);
            string result;
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
                result = result.Replace("null", "0");
            }
            Console.WriteLine("Json File Read");
            return result;
        }




        private static MySqlConnection OpenMYSQLDB(string server, string Database, string username, string password)
        {
            //Console.WriteLine("Opening Connection to Database !");
            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = "Server = " + myServerAddress + "; Database = " + Database + "; Uid = " + myUsername + "; Pwd = " + myPassword + ";";
            try
            {
                conn.Open();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                conn.Dispose();
                Console.WriteLine("Entering File Mode, use CTRL+C to end the program");
            }

            
             
            //Console.WriteLine("Connection to Database Open !");
            return conn;
        }
        public static int FetchID(MySqlConnection conn, string table, string fieldname, string value)
        {
            string sqlcmd = "Select id from " + table + " where " + fieldname + "=@" + fieldname + ";";
            MySqlCommand cmd = conn.CreateCommand();
            cmd.Parameters.AddWithValue("@" + fieldname, value);
            cmd.CommandText = sqlcmd;
            MySqlDataReader reader;
            int id = 0;
            using (reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        id = reader.GetInt32("id");
                    }
                }
                reader.Close();
            }
            return id;
        }
        public static bool CheckExists(MySqlConnection conn, string table, string whereclause)
        {
            string sqlcmd = "SELECT COUNT(id) as id from " + table + " where " + whereclause + ";";
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sqlcmd;
            MySqlDataReader reader;
            bool id = false;
            using (reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        id = reader.GetInt32("id") > 0;
                    }
                }
                reader.Close();
            }
            return id;
        }
        public static int LastID(MySqlConnection conn, string table)
        {
            string sqlcmd = "SELECT MAX(id) as ID from " + table + ";";
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = sqlcmd;
            MySqlDataReader reader;
            int id = 0;
            using (reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        id = reader.GetInt32("id");
                    }
                }
                reader.Close();
            }
            return id;
        }
        public static bool MySqlInsert(MySqlConnection conn, string table, string[] fields, string[] values)
        {
            if (fields.Length == values.Length)
            {
                string sqlcmd = "INSERT INTO " + table;
                string fld = " (";
                string val = " VALUES(";
                int result = 0;
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    for (int i = 0; i < fields.Length; i++)
                    {
                        fld += fields[i];
                        val += "@" + fields[i];
                        cmd.Parameters.AddWithValue("@" + fields[i], values[i]);
                        if (i < fields.Length - 1)
                        {
                            fld += ", ";
                            val += ", ";
                        }
                        else
                        {
                            fld += ")";
                            val += ");";
                        }
                    }
                    sqlcmd += fld + val;


                    cmd.CommandText = sqlcmd;
                    try
                    {
                        result = cmd.ExecuteNonQuery();
                    }
                    catch (MySqlException e)
                    {
                        Console.WriteLine(e.Message);

                    }

                }

                if (result > 0) return true;
            }
            return false;

        }

        /// <summary>
        /// Assumes that fields[0] and values[0] are the WHERE clause data
        /// </summary>
        /// <param name="conn">The Connection</param>
        /// <param name="table">The Table</param>
        /// <param name="fields">Fields Array</param>
        /// <param name="values">Values Array</param>
        /// <returns></returns>
        public static bool MySqlUpdate(MySqlConnection conn, string table, string[] fields, string[] values)
        {
            if (fields.Length == values.Length)
            {
                string sqlcmd = "UPDATE " + table + " SET ";
                using (MySqlCommand cmd = conn.CreateCommand())
                {
                    cmd.Parameters.AddWithValue("@" + fields[0], values[0]);
                    for (int i = 1; i < fields.Length; i++)
                    {
                        cmd.Parameters.AddWithValue("@" + fields[i], values[i]);
                        sqlcmd += fields[i] + "=" + "@" + fields[i];
                        if (i < fields.Length - 1)
                        {
                            sqlcmd += ", ";
                        }
                        else
                        {
                            sqlcmd += " WHERE " + fields[0] + "=" + "@" + fields[0] + ";";
                        }
                    }

                    cmd.CommandText = sqlcmd;

                    if (cmd.ExecuteNonQuery() > 0) return true;

                }
            }
            return false;

        }

        private static bool IsNewRec(MySqlConnection conn, string table, int id)
        {
            string sqlcmd = $"Select * from {table} where id={id};";
            bool result;
            using (MySqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = sqlcmd;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    result = !reader.HasRows;
                    reader.Close();
                }
            }
            return result;
        }
        public static int UpdateSystems()
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            List<EDsystem> Systems;
            Systems = JsonConvert.DeserializeObject<List<EDsystem>>(Wget(SystemUrl)); ;
            string[] values = new string[14];
            int RecordsAfected = 0;
            MySqlConnection conn = OpenMYSQLDB(myServerAddress, myDataBase, myUsername, myPassword);
            Console.WriteLine("Starting Systems update ...");
            foreach (EDsystem system in Systems)
            {

                values[0] = system.id.ToString();
                values[1] = system.name;
                values[2] = system.population.ToString();
                if (system.is_populated.ToString() == "true")
                {
                    values[3] = "1";
                }
                else
                {
                    values[3] = "0";
                }

                values[4] = epoch.AddSeconds(system.updated_at).ToString("yyyy-MM-dd H:mm:ss");
                if (system.needs_permit.ToString().ToLower() == "true")
                {
                    values[5] = "1";
                }
                else
                {
                    values[5] = "0";
                }


                values[6] = FetchID(conn, "app_power", "name", system.power.ToString()).ToString();
                values[7] = system.power_state_id.ToString();
                values[8] = system.primary_economy_id.ToString();
                values[9] = system.reserve_type_id.ToString();
                values[10] = system.security_id.ToString();
                values[11] = system.state_id.ToString();
                values[12] = system.government_id.ToString();
                values[13] = system.allegiance_id.ToString();

                if (IsNewRec(conn, "app_system", system.id))
                {
                    if (MySqlInsert(conn, "app_system", SYSTEMFIELDS, values)) RecordsAfected++;
                }
                else
                {
                    if (MySqlUpdate(conn, "app_system", SYSTEMFIELDS, values)) RecordsAfected++;
                }
                Thread.Sleep(200);
            }

            conn.Close();
            conn.Dispose();
            return RecordsAfected;
        }

        public static int UpdateFactions()
        {
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            List<EDFaction> Factions;
            Factions = JsonConvert.DeserializeObject<List<EDFaction>>(Wget(FactionUrl));
            string[] values = new string[8];
            int RecordsAfected = 0;
            MySqlConnection conn = OpenMYSQLDB(myServerAddress, myDataBase, myUsername, myPassword);
            Console.WriteLine("Starting Factions update ...");
            foreach (EDFaction faction in Factions)
            {
                //"id",
                //`name`,
                //`updated_at`,
                //`is_player_faction`,
                //`allegiance_id`,
                //`government_id`,
                //`home_system_id`,
                //`state_id`

                values[0] = faction.id.ToString();
                values[1] = faction.name;
                values[2] = epoch.AddSeconds(faction.updated_at).ToString("yyyy-MM-dd H:mm:ss");
                if (faction.is_player_faction.ToString().ToLower() == "true")
                {
                    values[3] = "1";
                }
                else
                {
                    values[3] = "0";
                }
                values[4] = faction.allegiance_id.ToString();
                values[5] = faction.government_id.ToString();
                values[6] = faction.home_system_id.ToString();
                values[7] = faction.state_id.ToString();

                if (IsNewRec(conn, "app_faction", faction.id))
                {
                    if (MySqlInsert(conn, "app_faction", FACTIONFIELDS, values)) RecordsAfected++;
                }
                else
                {
                    if (MySqlUpdate(conn, "app_faction", FACTIONFIELDS, values)) RecordsAfected++;
                }
                if (RecordsAfected % 200 == 0)
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
            conn.Close();
            conn.Dispose();
            return RecordsAfected;
        }

        public static void HistoryUpdate()
        {
            while (EDDNListener.m_Active)
            {



                //MySqlConnection conn = OpenMYSQLDB(myServerAddress, myDataBase, myUsername, myPassword);

                //noCycles.Text = (Convert.ToInt32(noCycles.Text) + 1).ToString();
                //Toprocess.Text = Program.Response.Count.ToString();
                if (EDDNListener.Response.Count() > 0)
                {
                    string[] Fvalues = new string[FHISTORYFIELDS.Length];
                    string[] Svalues = new string[SHISTORYFIELDS.Length];
                    MySqlConnection conn = OpenMYSQLDB(myServerAddress, myDataBase, myUsername, myPassword);
                    Message message = EDDNListener.Response.Dequeue().message;

                    //{"Datecycle","shAllegiance_id","shEconomy_id","shFaction_id","shGovtype_id","shPower_id","shPower_state_id","shSecurity_id","shState_id","shSystem_id"
                    DateTime.TryParse(message.timestamp.Replace('T', ' ').Replace("Z", ""), out DateTime senddate);
                    
                    string sTimestamp = senddate.AddHours(5).Date.ToString("yyyy-MM-dd"); //create an offsett of 5 hours, since after 19:00 utc the data of all the systems is updated to the next cycle data
                    Svalues[0] = sTimestamp;
                    Svalues[9] = FetchID(conn, "app_system", "name", message.StarSystem).ToString();
                    string whereclause = "shSystem_id=" + Svalues[9] + " AND Datecycle='" + sTimestamp + "';";
                    if (!CheckExists(conn, "app_shist", whereclause))
                    {

                        Console.Write("{1} - Updating data for system {0}", message.StarSystem, sTimestamp);
                        Svalues[1] = FetchID(conn, "app_superpower", "game_id", message.SystemAllegiance).ToString();
                        Svalues[2] = FetchID(conn, "app_economy", "game_id", message.SystemEconomy).ToString();
                        Svalues[3] = FetchID(conn, "app_faction", "name", message.SystemFaction).ToString();

                        Svalues[4] = FetchID(conn, "app_govtype", "game_id", message.SystemGovernment).ToString();
                        if ((message.Powers != null))
                        {
                            Svalues[5] = FetchID(conn, "app_power", "game_id", message.Powers[0]).ToString();
                            Svalues[6] = FetchID(conn, "app_powerstate", "game_id", message.PowerplayState).ToString();
                        }
                        Svalues[7] = FetchID(conn, "app_security", "game_id", message.SystemSecurity).ToString();

                        if (message.Factions != null)
                        {
                            for (int i = 0; i < message.Factions.Count; i++)
                            {//"Datecycle","fhInfluence","fhAllegiance_id","fhFaction_id","fhGovtype_id","fhState_id","fhSystem_id"
                                Fvalues[0] = sTimestamp;

                                Fvalues[1] = message.Factions[i].Influence.ToString().Replace(",", ".");
                                Fvalues[2] = FetchID(conn, "app_superpower", "name", message.Factions[i].Allegiance).ToString();
                                Fvalues[4] = FetchID(conn, "app_govtype", "type", message.Factions[i].Government).ToString();
                                Fvalues[5] = FetchID(conn, "app_fstate", "game_id", message.Factions[i].FactionState).ToString();

                                Fvalues[3] = FetchID(conn, "app_faction", "name", message.Factions[i].Name).ToString();
                                Fvalues[6] = Svalues[9];
                                if (Fvalues[3] == "0" || Svalues[3] == "0") // check if the faction exists if not create it !!!
                                {
                                    //{  "id", "name", "updated_at", "is_player_faction", "allegiance_id", "government_id", "home_system_id", "state_id"};
                                    string[] rFvalues = new string[FACTIONFIELDS.Length];
                                    int newid = LastID(conn, "app_faction") + 1;
                                    rFvalues[0] = newid.ToString();
                                    rFvalues[1] = message.Factions[i].Name;
                                    rFvalues[2] = sTimestamp;
                                    rFvalues[4] = Fvalues[2];
                                    rFvalues[5] = Fvalues[4];
                                    rFvalues[3] = "0";
                                    rFvalues[7] = Fvalues[5];
                                    rFvalues[6] = Fvalues[6];
                                    if (MySqlInsert(conn, "app_faction", FACTIONFIELDS, rFvalues))
                                    {

                                        Fvalues[3] = newid.ToString();
                                        Console.Write("  New Faction Added '{0}' ", rFvalues[1]);
                                    }
                                }

                                MySqlInsert(conn, "app_fhist", FHISTORYFIELDS, Fvalues);

                            }

                            foreach (Faction faction in message.Factions)
                            {
                                if (faction.Name == message.SystemFaction)
                                {
                                    Svalues[8] = FetchID(conn, "app_fstate", "game_id", faction.FactionState).ToString();

                                }
                            }

                        }
                        MySqlInsert(conn, "app_shist", SHISTORYFIELDS, Svalues);

                        Console.WriteLine("  System Updated  {0}", DateTime.Now.ToShortTimeString());
                    }
                    conn.Close();
                    conn.Dispose();
                }
                
            }

            Thread.Sleep(200);
        }

        static void Main(string[] args)
        {

            // Some biolerplate to react to close window event, CTRL-C, kill, etc
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            //start your multi threaded program here
            Program p = new Program();
            p.Start();

            //hold the console so it doesn’t run off the end
            while (!exitSystem)
            {
                Thread.Sleep(500);
            }




        }

        public void Start()
        {

// Console.WriteLine("Updated {0} Systems", UpdateSystems());
// Console.WriteLine("Updated {0} Factions", UpdateFactions());
            ReadCSVFile();
           
            Parallel.Invoke(() => EDDNListener.Subscribe(), () => HistoryUpdate());
            
        }

        public static void ReadCSVFile()
        {
            FileStream fs;
            if (!File.Exists(@"backup.jsonl"))
            {
                fs = File.Create(@"backup.jsonl");
                fs.Close();
                
            }

            fs = File.OpenRead(@"backup.jsonl");
            StreamReader reader = new StreamReader(fs);

            while (reader.Peek() >= 0)
            {
                EDDNListener.Response.Enqueue(JsonConvert.DeserializeObject<EDDNStream>(reader.ReadLine()));
            }
            fs.Close();
            fs.Dispose();
            File.Delete(@"backup.jsonl");



        }
    }
}





