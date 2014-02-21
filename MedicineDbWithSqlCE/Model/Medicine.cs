using System.Collections.ObjectModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GalaSoft.MvvmLight;

namespace MedicineDbWithSqlCE.Model
{
    [Table]
    public class Medicine : ViewModelBase
    {
        private int _medicineId;
        private string _name;
        private string _manufacturer;
        private string _origName;
        private string _details;
        private string _dosage;
        //private ObservableCollection<string> _available;
        private ObservableCollection<string> _classification;

        public Medicine()
        {
            //Available = new ObservableCollection<string>();
            Classification = new ObservableCollection<string>();
        }

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
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
                RaisePropertyChanged("Name");
                RaisePropertyChanging("Name");
            }
        }

        [Column]
        public string Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                if (_manufacturer == value)
                {
                    return;
                }
                _manufacturer = value;
                RaisePropertyChanged("Manufacturer");
                RaisePropertyChanging("Manufacturer");
            }
        }

        [Column]
        public string OrigName
        {
            get { return _origName; }
            set
            {
                if (_origName == value)
                {
                    return;
                }
                _origName = value;
                RaisePropertyChanged("OrigName");
                RaisePropertyChanging("OrigName");
            }
        }

        [Column]
        public string Details
        {
            get { return _details; }
            set
            {
                if (_details == value)
                {
                    return;
                }
                _details = value;
                RaisePropertyChanged("Details");
                RaisePropertyChanging("Details");
            }
        }

        [Column]
        public string Dosage
        {
            get { return _dosage; }
            set
            {
                if (_dosage == value)
                {
                    return;
                }
                _dosage = value;
                RaisePropertyChanged("Dosage");
                RaisePropertyChanging("Dosage");
            }
        }

        [Column(IsVersion = true)]
        private Binary _version;

        public ObservableCollection<string> Classification
        {
            get { return _classification; }
            set
            {
                if (_classification == value)
                {
                    return;
                }
                _classification = value;
                RaisePropertyChanged("Classification");
                RaisePropertyChanging("Classification");
            }
        }

    }
}
