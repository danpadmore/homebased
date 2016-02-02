using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace Homebase.Business.Infrastructure
{
    public static class StorageHelper
    {
        public static async Task<T> Load<T>(string filename) where T : class, new()
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException(nameof(filename));

            var file = await GetStorageFileForRead(filename);

            using (var stream = await file.OpenStreamForReadAsync())
            {
                var serializer = new DataContractJsonSerializer(typeof(T));

                return serializer.ReadObject(stream) as T
                    ?? new T();
            }
        }

        public static async Task<bool> Save<T>(string filename, T instance) where T : class, new()
        {
            if (string.IsNullOrEmpty(filename)) throw new ArgumentNullException(nameof(filename));
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            var file = await GetStorageFileForWrite(filename);

            using (var stream = await file.OpenStreamForWriteAsync())
            {
                var serializer = new DataContractJsonSerializer(instance.GetType());
                serializer.WriteObject(stream, instance);

                return true;
            }
        }

        private async static Task<StorageFile> GetStorageFileForRead(string filename)
        {
            var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();
            var settingsFile = files.SingleOrDefault(f => f.Name == filename);

            if (settingsFile == null)
                settingsFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename);

            return settingsFile;
        }

        private async static Task<StorageFile> GetStorageFileForWrite(string filename)
        {
            return await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
        }
    }
}
