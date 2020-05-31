using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Last
{
    public interface IRepository<T> where T : BaseEntiry
    {
        IEnumerable<T> Read(string read);
        void Create(string add, T baseEntity);
        void Delete(string delete, T baseEntity);
        void Update(string update, T baseEntity);
        void Open();
        void Close();
    }
}
