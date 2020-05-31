using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Last
{
    public class Context
    {
        readonly string _constr = @"Data Source=DESKTOP-R9G6Q1A\SQLEXPRESS;Initial Catalog=WebMarket.com_db;Integrated Security=True";
        readonly SqlConnection _con;
        public Context()
        {
            _con = new SqlConnection(_constr);
        }
        public void Close()
        {
            _con.Close();
        }
        public void Open()
        {
            _con.Open();
        }
        public void Execute<T>(string sql, T entity)
        {
            _con.Execute(sql, entity);
        }
        public IEnumerable<T> Query<T>(string sql)
        {
            return _con.Query<T>(sql);
        }
    }
}
