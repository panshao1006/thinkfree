using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace News.Model
{
    public class BaseModel
    {
        [JsonIgnore]
        public string TableName { set; get; }

        public BaseModel(string tableName)
        {
            TableName = tableName;
        }
    }
}
