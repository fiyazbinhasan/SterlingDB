using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Streams;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Reactive;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SterlingDB.Data_Access_Layer;
using SterlingDB.Model;
using Wintellect.Sterling;
using Wintellect.Sterling.IsolatedStorage;

namespace SterlingDB.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private SterlingEngine _engine;
        private ISterlingDatabaseInstance _databaseInstance;

        private ObservableCollection<Medicine> _medicines;

        public ICommand SearchCommand { get; set; }

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
        public MainViewModel()
        {
            Medicines = new ObservableCollection<Medicine>();
            StartSterling();
            //CreateMedicine();
            //SaveMedicine();
            //BackupDatabase();
            LoadMedicines();

            SearchCommand = new RelayCommand<string>(OnSearch);
        }

        private async void BackupDatabase()
        {
            var memStream = new MemoryStream();
            byte[] databaseBuffer;

            using (var binaryWriter = new BinaryWriter(memStream))
            {
                _engine.SterlingDatabase.Backup<DBGateway>(binaryWriter);
                binaryWriter.Flush();
                databaseBuffer = memStream.GetBuffer();

            }

            SaveDataBase(databaseBuffer);
        }

        private async void SaveDataBase(byte[] data)
        {
            StorageFolder myfolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await myfolder.CreateFileAsync("Backup", CreationCollisionOption.ReplaceExisting);

            using (Stream outputStream = await sampleFile.OpenStreamForWriteAsync())
            {
                await outputStream.WriteAsync(data, 0, data.Length);
                // await stream.CopyToAsync(outputStream);
            }
        }

        private void LoadMedicines()
        {
            var medicines = from m in _databaseInstance.Query<Medicine,int>()
                            select m.LazyValue.Value;
            
            Medicines = new ObservableCollection<Medicine>(medicines);
        }

        private void SaveMedicine()
        {
            foreach (var medicine in Medicines)
            {
                var key = _databaseInstance.Save(medicine);
                _databaseInstance.Flush();
            }
        }

        private void CreateMedicine()
        {
            int incrementer = 0;

            using (StreamReader file = File.OpenText(@"Medicines.json"))
            {
                using (var reader = new JsonTextReader(file))
                {
                    var jArray = JArray.Load(reader);
                    foreach (var medicine in jArray)
                    {
                        var medicineObj = medicine.ToObject<Medicine>();
                        medicineObj.MedicineId = incrementer++;
                        Medicines.Add(medicineObj);
                    }
                }
            }
        }

        private async void ResotreDatabase()
        {
            MoveReferenceDatabase();

            byte[] backup = await ReadFile("Backup");

            _engine.SterlingDatabase.Restore<DBGateway>(new BinaryReader(new MemoryStream(backup)));
            _engine.Dispose();

            _engine = new SterlingEngine();
            _engine.Activate();

            _databaseInstance = _engine.SterlingDatabase.RegisterDatabase<DBGateway>(new IsolatedStorageDriver());

        }

        private async Task<byte[]> ReadFile(string backupmedicine)
        {
            byte[] content = null;

            IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;

            IStorageFile storageFile = await applicationFolder.GetFileAsync(backupmedicine);

            IRandomAccessStream accessStream = await storageFile.OpenReadAsync();

            using (Stream stream = accessStream.AsStreamForRead((int)accessStream.Size))
            {
                content = new byte[stream.Length];
                await stream.ReadAsync(content, 0, (int)stream.Length);
            }

            return content;
        }

        private void MoveReferenceDatabase()
        {
            var iso = IsolatedStorageFile.GetUserStoreForApplication();

            using (var input = Application.GetResourceStream(new Uri("Backup", UriKind.Relative)).Stream)
            {
                using (IsolatedStorageFileStream output = iso.CreateFile("MedicineDb"))
                {
                    var readBuffer = new byte[4096];
                    var bytesRead = -1;

                    while ((bytesRead = input.Read(readBuffer, 0, readBuffer.Length)) > 0)
                    {
                        output.Write(readBuffer, 0, bytesRead);
                    }
                }
            }
        }

        private void StartSterling()
        {
            _engine = new SterlingEngine();
            _engine.Activate();
            _databaseInstance = _engine.SterlingDatabase.RegisterDatabase<DBGateway>(new IsolatedStorageDriver());
        }

        private void OnSearch(string obj)
        {
            var medicines = from m in _databaseInstance.Query<Medicine, int>()
                where m.LazyValue.Value.Name.Contains(obj.ToLower())
                select m.LazyValue.Value;


            Medicines = new ObservableCollection<Medicine>(medicines);
        }
    }
}