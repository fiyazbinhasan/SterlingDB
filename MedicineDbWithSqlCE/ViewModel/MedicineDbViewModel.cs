using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MedicineDbWithSqlCE.Resources;
using Microsoft.Phone.Reactive;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using MedicineDbWithSqlCE.Model;

namespace MedicineDbWithSqlCE.ViewModel
{
    public class MedicineDbViewModel : ViewModelBase
    {
        const string ConnectionString = "Data Source=isostore:/MedicineDatabase.sdf";

        private readonly MedicineDbDataContext _medicineDbDataContext;

        private ObservableCollection<Medicine> _medicines; 
        public ObservableCollection<Medicine> Medicines
        {
            get { return _medicines; }
            set
            {
                if (_medicines == value)
                {
                    return;
                }
                _medicines = value;
                RaisePropertyChanged("Medicines");
                RaisePropertyChanging("Medicines");
            }
        }

        private ObservableCollection<Classification> _classifications;
        public ObservableCollection<Classification> Classifications
        {
            get { return _classifications; }
            set
            {
                if (_classifications == value)
                {
                    return;
                }
                _classifications = value;
                RaisePropertyChanged("Classifications");
                RaisePropertyChanging("Classifications");
            }
        }

        public ICommand SearchCommand { get; set; }

        public MedicineDbViewModel()
        {
            _medicineDbDataContext = new MedicineDbDataContext(ConnectionString);
            
            Medicines = new ObservableCollection<Medicine>();
            Classifications = new ObservableCollection<Classification>();

            SearchCommand = new RelayCommand<string>(OnSearchButtonTap);
            
            //CreateDatabase();

            //ReadJsonAndAddMedicines();


            
            LoadMedicinesFromDatabase();
            LoadClassificationsFromDatabase();
        }

        private void LoadClassificationsFromDatabase()
        {
            var classificationsInDb = from Classification classification
                                      in _medicineDbDataContext.Classifications
                                      select classification;

            Classifications = new ObservableCollection<Classification>(classificationsInDb);
        }

        private void OnSearchButtonTap(string param)
        {
            var searchedMedicine = from Medicine medicine
                                   in _medicineDbDataContext.Medicines
                                   where medicine.Name.Contains(param)
                                   select medicine;

            Medicines = new ObservableCollection<Medicine>(searchedMedicine);
        }

        private void ReadJsonAndAddMedicines()
        {
            using (var file = File.OpenText(@"Medicines.json"))
            {
                using (var reader = new JsonTextReader(file))
                {
                    var jArray = JArray.Load(reader);
                    int incrementer = 1;

                    foreach (var medicineObject in jArray.Select(medicine => medicine.ToObject<Medicine>()))
                    {
                        Medicines.Add(medicineObject);
                        foreach (var classification in medicineObject.Classification)
                        {
                            var classificationObj = new Classification {Name = classification, MedicineId = incrementer};
                            Classifications.Add(classificationObj);
                        }
                        incrementer = incrementer + 1;
                    }
                }
                AddMedicines();
                AddClassifications();
            }
        }

        private void CreateDatabase()
        {
            using (var medicineDb = new MedicineDbDataContext(ConnectionString))
            {
                if (medicineDb.DatabaseExists() == false)
                {
                    medicineDb.CreateDatabase();
                    medicineDb.SubmitChanges();
                }   
            }
        }

        public void SaveChangesToDB()
        {
            _medicineDbDataContext.SubmitChanges();
        }

        public void LoadMedicinesFromDatabase()
        {
            var medicinesInDb = from Medicine medicine 
                                in _medicineDbDataContext.Medicines
                                select medicine;

            Medicines = new ObservableCollection<Medicine>(medicinesInDb);
        }

        public void AddMedicines()
        {
            try
            {
                _medicineDbDataContext.Medicines.InsertAllOnSubmit(Medicines);
                _medicineDbDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AddClassifications()
        {
            try
            {
                _medicineDbDataContext.Classifications.InsertAllOnSubmit(Classifications);
                _medicineDbDataContext.SubmitChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public IList<Medicine> GetRelatedMedicines(Medicine medicine)
        {
            var commonClassification = (from c in Classifications
                where c.MedicineId == medicine.MedicineId
                select c).FirstOrDefault();

            var medicineWithSimilarClassification = Classifications.Where(classification => commonClassification != null && classification.Name.Contains(commonClassification.Name)).ToList();

            var relatedMedicines = from c in medicineWithSimilarClassification
                join m in Medicines on c.MedicineId equals m.MedicineId
                select m;

            return relatedMedicines.ToList();

        }
    }
}
