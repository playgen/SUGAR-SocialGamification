using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace PlayGen.SGA.DataAccess
{
    public class CodeConfig : DbConfiguration
    {
        public CodeConfig()
        {
            SetProviderServices("System.SaveData.SqlClient", 
                System.Data.Entity.SqlServer.SqlProviderServices.Instance);
        }
    }
}
