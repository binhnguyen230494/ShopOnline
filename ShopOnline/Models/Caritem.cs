using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopOnline.Models
{
    [Serializable]
    public class Caritem
    {
        
        public Product Product { set; get; }
        public int Quantily { set; get; }

    }
}