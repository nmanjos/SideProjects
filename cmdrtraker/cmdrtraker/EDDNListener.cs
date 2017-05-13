using Ionic.Zlib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace edbgsupdate
{
    public class Header
    {
        public string softwareVersion { get; set; }
        public string gatewayTimestamp { get; set; }
        public string softwareName { get; set; }
        public string uploaderID { get; set; }
    }

    public class Faction
    {
        public string Allegiance { get; set; }
        public string FactionState { get; set; }
        public double Influence { get; set; }
        public string Name { get; set; }
        public string Government { get; set; }

        public override string ToString()
        {

            return Name  +" - " + Influence.ToString() + " - " + FactionState + " - " + Allegiance + " - " +   Government; ;
        }
    }
    public class Message
    {
        public string StarSystem { get; set; }
        public string SystemFaction { get; set; }
        public string timestamp { get; set; }
        public string systemName { get; set; }
    }

    public class MessageJournal
    {
        public string StarSystem { get; set; }
        public string SystemFaction { get; set; }
        public string timestamp { get; set; }
        public string SystemSecurity { get; set; }
        public string SystemAllegiance { get; set; }
        public string PowerplayState { get; set; }
        public string SystemEconomy { get; set; }
        public List<double> StarPos { get; set; }
        public List<Faction> Factions { get; set; }
        public List<string> Powers { get; set; }
        public string @event { get; set; }
        public string SystemGovernment { get; set; }
    }

    public class Commodity
    {
        public int demandBracket { get; set; }
        public string name { get; set; }
        public int buyPrice { get; set; }
        public int meanPrice { get; set; }
        public int stockBracket { get; set; }
        public int demand { get; set; }
        public int sellPrice { get; set; }
        public int stock { get; set; }
    }

    public class MessageComodity
    {
        public List<Commodity> commodities { get; set; }
        public string timestamp { get; set; }
        public string systemName { get; set; }
        public string stationName { get; set; }
    }
    public class MessageOutfitting
    {
        public string systemName { get; set; }
        public List<string> modules { get; set; }
        public string stationName { get; set; }
        public string timestamp { get; set; }
    }

    public class EDDNStreamComodities
    {
        public Header header { get; set; }
        public string schemaRef { get; set; }
        public MessageComodity message { get; set; }
    }

    public class EDDNStreamOutfitting
    {
        public Header header { get; set; }
        public string schemaRef { get; set; }
        public MessageOutfitting message { get; set; }
    }

    public class EDDNStreamJournal
    {
        public Header header { get; set; }
        public string schemaRef { get; set; }
        public MessageJournal message { get; set; }

        
    }

    class EDDNListener
    {
        public static bool m_Active = true;
        public static string m_Adress = "tcp://eddn-relay.elite-markets.net:9500";
        public static Queue<EDDNStreamJournal> ResJournal = new Queue<EDDNStreamJournal>();
        public static Queue<EDDNStreamOutfitting> ResOutfitting = new Queue<EDDNStreamOutfitting>();
        public static Queue<EDDNStreamComodities> ResComodities = new Queue<EDDNStreamComodities>();

        public static List<string> Getbackup()
        {
            List<string> stream =  new List<string>();
 
            foreach (EDDNStreamJournal st in ResJournal)
            {
                stream.Add(JsonConvert.SerializeObject(st));
            }
            foreach (EDDNStreamOutfitting st in ResOutfitting)
            {
                stream.Add(JsonConvert.SerializeObject(st));
            }
            foreach (EDDNStreamComodities st in ResComodities)
            {
                stream.Add(JsonConvert.SerializeObject(st));
            }

            return stream;
        }


        public static void Subscribe()
        {

            m_Active = true;

            using (var ctx = ZContext.Create())
            {
                using (var socket = new ZSocket(ctx, ZSocketType.SUB))
                {
                    socket.SubscribeAll();

                    socket.Connect(m_Adress);

                    while (m_Active)
                    {
                        ZFrame frame = socket.ReceiveFrame();
                        var decompressedFileStream = new MemoryStream();
                        if (frame != null)
                        {
                            using (var ms = new MemoryStream())
                            {
                                frame.CopyTo(ms);
                                ms.Position = 0;
                                using (var stream = new ZlibStream(ms, CompressionMode.Decompress))  
                                using (var sr = new StreamReader(stream))
                                {
                                    string msg = sr.ReadToEnd();
                                    if (msg.Contains("Journal"))
                                    {
                                        EDDNStreamJournal Data = EDDNData(msg);
                                        ResJournal.Enqueue(Data);
                                    }
                                }
                            }
                        }
                        Thread.Sleep(100);
                    }

                }
            }
            m_Active = false;
        }
        public static EDDNStream EDDNData(string str)
        {
            EDDNStream Data = new EDDNStream();

            if (!string.IsNullOrEmpty(str) || !string.IsNullOrWhiteSpace(str))
            {
                Data = JsonConvert.DeserializeObject<EDDNStream>(str);
                Thread.Sleep(150);
            }
            return Data;
        }

    }
}
