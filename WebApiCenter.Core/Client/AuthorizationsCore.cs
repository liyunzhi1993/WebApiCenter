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
    public class AuthorizationsCore
    {
        private static AuthorizationsCore instance;

        public static AuthorizationsCore GetInstance()
        {
            if (instance == null)
            {
                instance = new AuthorizationsCore();
            }
            return instance;
        }

        public void InsertAuthorization(Authorizations authorization)
        {
            MongodbHelper.InsertOne("Authorizations", authorization);
        }

        public void DeleteAuthorization(string id)
        {
            MongodbHelper.Delete("Authorizations", id);
        }

        public Authorizations GetAuthorization(string clientsid)
        {
            return MongodbHelper.GetOne<Authorizations>("Authorizations", Query.EQ("clientsid", clientsid));
        }
    }
}
