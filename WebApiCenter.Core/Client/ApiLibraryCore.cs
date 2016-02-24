using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiCenter.Core.Common;
using WebApiCenter.Models.Client;

namespace WebApiCenter.Core.Client
{
    public class ApiLibraryCore
    {
        private static ApiLibraryCore instance;

        public static ApiLibraryCore GetInstance()
        {
            if (instance == null)
            {
                instance = new ApiLibraryCore();
            }
            return instance;
        }
        public List<ApiLibrary> GetApiLibraryList()
        {
            return MongodbHelper.GetAll<ApiLibrary>("ApiLibrary");
        }

        public void UpdateApiLibrary(ApiLibrary apiLibrary)
        {
            MongodbHelper.UpdateOne<ApiLibrary>("ApiLibrary", apiLibrary);
        }

        public ApiLibrary GetApiLibrary(string id)
        {
            return MongodbHelper.GetOne<ApiLibrary>("ApiLibrary", id);
        }
    }
}
