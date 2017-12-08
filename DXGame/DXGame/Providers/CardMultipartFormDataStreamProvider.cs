using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using DXGame.Models;
using DXGame.Models.Entities;

namespace DXGame.Providers
{
    public class CardMultipartFormDataStreamProvider : MultipartFormDataStreamProvider
    {
        private INewIDProvider _IDProvider;
        private IFilenameProvider _filenameProvider;

        private int ID_offset = 0;
        public CardMultipartFormDataStreamProvider(string path, INewIDProvider IDProvider, IFilenameProvider filenameProvider) : base(path)
        {
            _IDProvider = IDProvider;
            _filenameProvider = filenameProvider;
        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            var name = _filenameProvider.GenerateFilename(_IDProvider.GetID() + ID_offset, Path.GetExtension(headers.ContentDisposition.FileName));
            headers.ContentDisposition.FileName = name;
            ID_offset++;

            return name;
        }
    }
}