using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SterlingDB.Model;
using Wintellect.Sterling.Database;

namespace SterlingDB.Data_Access_Layer
{
    public class DBGateway : BaseDatabaseInstance
    {
        public override string Name
        {
            get
            {
                return "MedicineDb";
            }
        }
        protected override List<ITableDefinition> RegisterTables()
        {
            return new List<ITableDefinition>()
            {
                CreateTableDefinition<Medicine, int>(
                    m => m.MedicineId)
            };
        }
    }
}
