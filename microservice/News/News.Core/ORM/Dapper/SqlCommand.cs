using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.Core.ORM.Dapper
{
    public class SqlCommand
    {
        public string Sql { set; get; }

        public DynamicParameters Parameters { set; get; }

        public SqlCommand()
        {
            Parameters = new DynamicParameters();
        }

        public SqlCommand(string baseSqlString) : base()
        {
            Sql = baseSqlString;
        }
    }
}
