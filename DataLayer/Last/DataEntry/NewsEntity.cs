using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Last
{
    public class NewsEntity : BaseEntiry
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string PublishDate { get; set; }
    }
}
