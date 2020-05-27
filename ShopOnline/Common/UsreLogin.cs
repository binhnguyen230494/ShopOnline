using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopOnline
{
    [Serializable]
    public class UsreLogin
    {
        
        public long UserID { set; get; }
        public string UserName { set; get; }
    }
}