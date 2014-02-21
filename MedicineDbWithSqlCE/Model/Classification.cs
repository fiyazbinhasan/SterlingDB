using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GalaSoft.MvvmLight;

namespace MedicineDbWithSqlCE.Model
{
    [Table]
    public class Classification : ViewModelBase
    {
        private int _classificationId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int ClassificationId
        {
            get { return _classificationId; }
            set
            {
                if (_classificationId == value)
                {
                    return;
                }
                _classificationId = value;
                RaisePropertyChanging("ClassificationId");
                RaisePropertyChanged("ClassificationId");
            }
        }


        private string _name;

        [Column]
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                {
                    return;
                }
                _name = value;
                RaisePropertyChanging("Name");
                RaisePropertyChanged("Name");
            }
        }

        private int _medicineId;

        [Column]
        public int MedicineId
        {
            get { return _medicineId; }
            set
            {
                if (_medicineId == value)
                {
                    return;
                }
                _medicineId = value;
                RaisePropertyChanging("MedicineId");
                RaisePropertyChanged("MedicineId");
            }
        }
    }
}
