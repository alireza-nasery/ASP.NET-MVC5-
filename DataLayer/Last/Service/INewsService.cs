using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Last
{
    public interface INewsService
    {
        bool AddNews(NewsViewModel viewModel);
    }
}
