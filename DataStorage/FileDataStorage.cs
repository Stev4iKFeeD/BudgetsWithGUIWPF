using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataStorage
{
    public class FileDataStorage<TObject> where TObject : class, IStorable
    {
        private static string _baseFolder;

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

            foreach (var file in Directory.EnumerateFiles(_baseFolder))
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
    }
}
