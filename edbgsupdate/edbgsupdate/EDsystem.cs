﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edbgsupdate
{
    public class MinorFactionPresence
    {
        public int minor_faction_id { get; set; }
        public int? state_id { get; set; }
        public double? influence { get; set; }
        public string state { get; set; }
    }

    public class EDsystem
    {
        public int id { get; set; }
        public int edsm_id { get; set; }
        public string name { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public Int64 population { get; set; }
        public bool is_populated { get; set; }
        public int government_id { get; set; }
        public string government { get; set; }
        public int allegiance_id { get; set; }
        public string allegiance { get; set; }
        public int state_id { get; set; }
        public string state { get; set; }
        public int security_id { get; set; }
        public string security { get; set; }
        public int primary_economy_id { get; set; }
        public string primary_economy { get; set; }
        public string power { get; set; }
        public string power_state { get; set; }
        public int? power_state_id { get; set; }
        public bool needs_permit { get; set; }
        public Int64 updated_at { get; set; }
        public string simbad_ref { get; set; }
        public int controlling_minor_faction_id { get; set; }
        public string controlling_minor_faction { get; set; }
        public int reserve_type_id { get; set; }
        public string reserve_type { get; set; }
        public List<MinorFactionPresence> minor_faction_presences { get; set; }
    }
}
