using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataStorage
{
    public class FileDataStorage<TObject> where TObject : class, IStorable
    {
        private readonly string _baseFolder;

        public FileDataStorage(string subDirectory = null)
        {
            if (string.IsNullOrWhiteSpace(subDirectory))
            {
                _baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "BudgetsStorage", typeof(TObject).Name);
            }
            else
            {
                _baseFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "BudgetsStorage", typeof(TObject).Name, subDirectory);
            }

            if (!Directory.Exists(_baseFolder))
            {
                Directory.CreateDirectory(_baseFolder);
            }
        }

        public async Task AddOrUpdateAsync(TObject obj)
        {
            if (!Directory.Exists(_baseFolder))
            {
                Directory.CreateDirectory(_baseFolder);
            }

            string stringObject = JsonSerializer.Serialize(obj);
            string filePath = Path.Combine(_baseFolder, obj.Guid.ToString("N"));

            using (StreamWriter streamWriter = new StreamWriter(filePath, false))
            {
                await streamWriter.WriteAsync(stringObject);
            }
        }

        public async Task<TObject> GetAsync(Guid guid)
        {
            string stringObject;
            string filePath = Path.Combine(_baseFolder, guid.ToString("N"));

            if (!File.Exists(filePath))
            {
                return null;
            }

            using (StreamReader streamReader = new StreamReader(filePath))
            {
                stringObject = await streamReader.ReadToEndAsync();
            }

            return JsonSerializer.Deserialize<TObject>(stringObject);
        }

        public async Task<List<TObject>> GetAllAsync()
        {
            List<TObject> res = new List<TObject>();

            var files = Directory.EnumerateFiles(_baseFolder).OrderBy(s => new FileInfo(s).CreationTime);
            foreach (var file in files)
            {
                string stringObject;

                using (StreamReader streamReader = new StreamReader(file))
                {
                    stringObject = await streamReader.ReadToEndAsync();
                }

                res.Add(JsonSerializer.Deserialize<TObject>(stringObject));
            }

            return res;
        }

        // public async Task<List<TObject>> GetSomeAsync(int from, int to)
        // {
        //     List<TObject> res = new List<TObject>();
        //
        //     int current = from;
        //     foreach (var file in Directory.EnumerateFiles(_baseFolder).OrderBy(s => new FileInfo(s).LastWriteTime))
        //     {
        //         if (current >= to)
        //         {
        //             break;
        //         }
        //
        //         string stringObject;
        //
        //         using (StreamReader streamReader = new StreamReader(file))
        //         {
        //             stringObject = await streamReader.ReadToEndAsync();
        //         }
        //
        //         res.Add(JsonSerializer.Deserialize<TObject>(stringObject));
        //
        //         ++current;
        //     }
        //
        //     return res;
        // }

        public async Task<bool> Delete(Guid guid)
        {
            if (guid == Guid.Empty)
            {
                string path = Path.Combine(_baseFolder);
                if (!Directory.Exists(path))
                {
                    return false;
                }

                Directory.Delete(path, true);
                return true;
            }
            else
            {
                string path = Path.Combine(_baseFolder, guid.ToString("N"));
                if (!File.Exists(path))
                {
                    return false;
                }

                File.Delete(path);
                return true;
            }
        }
    }
}
