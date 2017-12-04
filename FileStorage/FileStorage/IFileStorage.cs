using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage
{
    public interface IFileStorage
    {
        /// <summary>
        /// All files stored in storage
        /// </summary>
        IEnumerable<FileStorage.File> Files { get; }
        /// <summary>
        /// Add a file to storage
        /// </summary>
        /// <param name="data">File content</param>
        /// <param name="extension">Extension of file used for filename creation</param>
        /// <returns>File instance with unique local id</returns>
        FileStorage.File AddFile(Stream data, string extension);
        /// <summary>
        /// Delete file from storage
        /// </summary>
        /// <param name="id">ID of file to delete</param>
        /// <returns>Deleted file instance</returns>
        FileStorage.File DeleteFile(int id);
    }
}
