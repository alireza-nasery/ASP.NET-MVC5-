using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Last
{

    public class Repository<T> : IRepository<T> where T : BaseEntiry
    {
        readonly Context _context;
        public Repository(Context context)
        {
            _context = context;
        }

        public void Close()
        {
            _context.Close();
        }

        public void Open()
        {
            _context.Open();
        }
        public void Create(string create, T baseEntity)
        {
            _context.Execute<T>(create, baseEntity);
        }

        public void Delete(string delete, T baseEntity)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<T> Read(string read)
        {
            return _context.Query<T>(read);
        }


        public void Update(string update, T baseEntity)
        {
            throw new NotImplementedException();
        }
    }
}
