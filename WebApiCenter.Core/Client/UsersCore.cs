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
        
        public Users Login(Users user)
        {
            return MongodbHelper.GetOne<Users>("Users", Query.And(Query.EQ("account", user.account),Query.EQ("password",user.password)));
        }
    }
}
