using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCenter.Core.Common;
using WebApiCenter.Models.Client;

namespace WebApiCenter.Core.Client
{
    public class ClientsCore
    {
        private BsonDocument doc = new BsonDocument();

        private static ClientsCore instance;

        public static ClientsCore GetInstance()
        {
            if (instance==null)
            {
                instance = new ClientsCore();
            }
            return instance;
        }
        public List<Clients> GetClientList()
        {
            return MongodbHelper.GetAll<Clients>("Clients");
        }

        public void InsertClient(Clients client)
        {
            MongodbHelper.InsertOne<Clients>("Clients",client);
        }

        public void UpdateClient(Clients client)
        {
            MongodbHelper.UpdateOne<Clients>("Clients", client);
        }

        public void Del(string id)
        {
            MongodbHelper.Delete("Clients", id);
        }

        public Clients GetClient(string id)
        {
            return MongodbHelper.GetOne<Clients>("Clients", id);
        }

        public Clients GetClientByClientid(string clientid)
        {
            return MongodbHelper.GetOne<Clients>("Clients", Query.EQ("clientid", clientid));
        }
    }
}
