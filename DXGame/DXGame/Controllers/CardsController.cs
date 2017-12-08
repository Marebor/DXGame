using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using DXGame.Models;
using DXGame.Models.Entities;
using DXGame.Providers;
using DXGame.Extensions;

namespace DXGame.Controllers
{
    public class CardsController : ApiController
    {
        private readonly string acceptedExtensions = " .jpeg .png .bmp .gif";
        
        private ICardsRepository _cardsRepository;
        private IRootPathProvider _rootPathProvider;
        private IFilenameProvider _filenameProvider;

        public CardsController(
            ICardsRepository repository, 
            IRootPathProvider pathProvider,
            IFilenameProvider filenameProvider
            )
        {
            _cardsRepository = repository;
            _rootPathProvider = pathProvider;
            _filenameProvider = filenameProvider;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IEnumerable<Card> GetCards()
        {
            return _cardsRepository.Cards;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> GetCard(int id)
        {
            var card = await _cardsRepository.FindAsync(id);
            if (card == null)
                return NotFound();

            return Ok(card);
        }

        public async Task<IHttpActionResult> PostCard()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = new MultipartMemoryStreamProvider();
            provider = await Request.Content.ReadAsMultipartAsync(provider);

            var cards = new List<Card>();
            var storagePath = Path.Combine(_rootPathProvider.GetRoot(), "Content", "Cards");
            if (!Directory.Exists(storagePath)) Directory.CreateDirectory(storagePath);
            foreach (var file in provider.Contents)
            {
                var card = await _cardsRepository.AddAsync(new Card());
                var filename = new string(file.Headers.ContentDisposition.FileName.Except(new char[] { '\\' }).ToArray());
                var index = filename.LastIndexOf('.');
                var extension = index > -1 ? new string(filename.Skip(index).ToArray()) : string.Empty;
                if (!acceptedExtensions.Contains(new string(extension.ToArray()))) continue;
                var name = _filenameProvider.GenerateFilename(card.ID, extension);

                using (var fs = File.Create(Path.Combine(storagePath, name)))
                {
                    await (await file.ReadAsStreamAsync()).CopyToAsync(fs);
                }

                card.URL = "Content/Cards/" + name;
                card = await _cardsRepository.UpdateAsync(card);
                cards.Add(card);
            }

            return cards.Count > 0 ? (IHttpActionResult)Created("Content/Cards/", cards) : BadRequest("Bad file format");
        }

        public async Task<IHttpActionResult> DeleteCard(int id)
        {
            var card = await _cardsRepository.DeleteAsync(id);

            if (card != null) File.Delete(Path.Combine(_rootPathProvider.GetRoot(), card.URL));

            return card != null ? (IHttpActionResult)Ok(card) : NotFound();
        }
    }
}