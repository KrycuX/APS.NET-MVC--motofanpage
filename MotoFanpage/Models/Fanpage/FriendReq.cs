using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MotoFanpage.Models.Fanpage
{
    public class FriendReq
    {
        public int ID { get; set; }
        public int IdWysylajacego { get; set; }
        public int IdOdbierajacego { get; set; }

        public int Status { get; set; }
    }
}