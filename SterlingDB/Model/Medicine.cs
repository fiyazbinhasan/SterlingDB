using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace SterlingDB.Model
{
    public class Medicine : ViewModelBase
    {
        private int _medicineId;
        private string _name;
        private string _manufacturer;
        private string _origName;
        private string _details;
        private string _dosage;
        private ObservableCollection<string> _available;
        private ObservableCollection<string> _classification;


        public int MedicineId
        {
            get { return _medicineId; }
            set
            {
                if (_medicineId != value)
                {
                    _medicineId = value;
                    this.RaisePropertyChanged(() => MedicineId);
                }
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    this.RaisePropertyChanged(() => Name);
                }
            }
        }

        public string Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                if (_manufacturer != value)
                {
                    _manufacturer = value;
                    this.RaisePropertyChanged(()=> Manufacturer);
                }
            }
        }

        public string OrigName 
        {
            get { return _origName; }
            set
            {
                if (_origName != value)
                {
                    _origName = value;
                    this.RaisePropertyChanged(() => OrigName);
                }
            }
        }

        public string Details
        {
            get { return _details; }
            set
            {
                if (_details != value)
                {
                    _details = value;
                    this.RaisePropertyChanged(() => Details);
                }
            }
        }

        public string Dosage
        {
            get { return _dosage; }
            set
            {
                if (_dosage != value)
                {
                    _dosage = value;
                    this.RaisePropertyChanged(() => Dosage);
                }
            }
        }

        public ObservableCollection<string> Available
        {
            get { return _available; }
            set
            {
                if (_available != value)
                {
                    _available = value;
                    this.RaisePropertyChanged(() => Available);
                }
            }
        }

        public ObservableCollection<string> Classification
        {
            get { return _classification; }
            set
            {
                if (_classification != value)
                {
                    _classification = value;
                    this.RaisePropertyChanged(() => Classification);
                }
            }
        }

    }
}
