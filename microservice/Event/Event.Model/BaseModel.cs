using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Event.Model
{
    public class BaseModel
    {
        public string Id { set; get; }

        [JsonIgnore]
        public string TableName { set; get; }

        public BaseModel(string tableName)
        {
            TableName = tableName;
        }
    }
}
