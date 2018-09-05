using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ORM.Dapper
{
    public class SqlCommand
    {
        public string Sql { set; get; }

        public DynamicParameters Parameters { set; get; }

        //public T Operator{ get; set; }

        public SqlCommand()
        {
            Parameters = new DynamicParameters();
        }

        public SqlCommand(string baseSqlString)
        {
            Sql = baseSqlString;
            Parameters = new DynamicParameters();
        }
    }
}
