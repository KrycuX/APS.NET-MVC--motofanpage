using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotoFanpage.Models
{
    public class Logi
    {
        public int Id { get; set; }
        public string IpAdress { get; set; }
        public int IdUser { get; set; }
        public DateTime Date { get; set; }
        public string Instruction { get; set; }
        public int IdElement { get; set; }
        public string WhatElement { get; set; }

    }
}