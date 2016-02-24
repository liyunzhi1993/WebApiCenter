using MongoDB;
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
    public class UsersCore
    {

        private static UsersCore instance;

        public static UsersCore GetInstance()
        {
            if (instance == null)
            {
                instance = new UsersCore();
            }
            return instance;
        }
        public Users Login(Users user)
        {
            return MongodbHelper.GetOne<Users>("Users", Query.And(Query.EQ("account", user.account),Query.EQ("password",user.password)));
        }
    }
}
