using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ViewModel
{
    public class ProductView
    {
        public long ID { set; get; }
        public string Name { set; get; }
        public string Images { set; get; }
        public string MetaTitle { get; set; }
        public DateTime? CreatedDate { get; set; }
        public decimal? Price { get; set; }
        public string CateName { get; set; }
        public string CateMetaTitle { get; set; }
    }
}
