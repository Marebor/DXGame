using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage
{
    public class FileStorageFolder : IFileStorage
    {
        private const string FILENAME_PREFIX = "File_ID-";
        private const int FILENAME_ID_LENGTH = 6;
        private const string DEFAULT_FOLDER_NAME = "FileStorageFolder_Default";

        private object lock_maxID = new object();
        private int _maxID;

        public IEnumerable<File> Files { get; private set; }

        public string Path { get; }

        /// <summary>
        /// Creates new instance of FileStorageFolder with specified folder path
        /// </summary>
        /// <param name="path">Path to storage folder</param>
        public FileStorageFolder(string path)
        {
            Path = path;
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            Files = LoadFiles(Path);
            if (Files.Count() > 0) _maxID = Files.Max(f => f.ID);
        }

        public File AddFile(Stream data, string extension)
        {
            int id;
            lock (lock_maxID)
            {
                _maxID++;
                id = _maxID;
            }
            
            var filename = GenerateFilename(id, extension);
            var path = Path ?? System.IO.Path.Combine(Directory.GetCurrentDirectory(), DEFAULT_FOLDER_NAME);
            var filepath = System.IO.Path.Combine(path, filename);

            using (var fs = new FileStream(filepath, FileMode.CreateNew))
            {
                data.CopyTo(fs);
            }

            var file = new File() { ID = id, Path = filepath };
            (Files as List<File>).Add(file);

            return file;
        }

        public File DeleteFile(int id)
        {
            var file = Files.FirstOrDefault(f => f.ID == id);

            if (file != null)
            {
                System.IO.File.Delete(file.Path);
                (Files as List<File>).Remove(file);
            }

            return file;
        }

        private string GenerateFilename(int id, string extension)
        {
            var id_format = $"D{FILENAME_ID_LENGTH}";
            return $"{FILENAME_PREFIX}{id.ToString(id_format)}.{extension}";
        }

        private int GetIDFromFilename(string filename)
        {
            var name = System.IO.Path.GetFileNameWithoutExtension(filename);

            int id = -1;

            if (name.StartsWith(FILENAME_PREFIX) && name.Length == FILENAME_PREFIX.Length + FILENAME_ID_LENGTH)
            {
                int.TryParse(string.Concat(name.Skip(FILENAME_PREFIX.Length)), out id);
            }

            return id;
        }

        private IEnumerable<File> LoadFiles(string path)
        {
            var collection = new List<File>();
            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                var id = GetIDFromFilename(file);
                if (id > 0) collection.Add(new File() { ID = id, Path = file });
            }

            return collection;
        }
    }
}
