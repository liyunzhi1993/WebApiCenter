using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiCenter.Models
{
    public class BaseModel
    {
        [BsonIgnore]
        public string id
        {
            get
            {
                if (_id == ObjectId.Empty)
                    _id = ObjectId.GenerateNewId();
                return _id.ToString();
            }
        }
        [BsonId]
        public ObjectId _id;
    }
}
