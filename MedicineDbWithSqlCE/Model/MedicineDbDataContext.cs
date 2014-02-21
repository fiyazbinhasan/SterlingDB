using System.Data.Linq;

namespace MedicineDbWithSqlCE.Model
{
    public class MedicineDbDataContext : DataContext
    {
        public MedicineDbDataContext(string connectionString): base(connectionString) { }
        public Table<Medicine> Medicines;
        public Table<Classification> Classifications;
    }
}
