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
using DXGame.Models.Abstract;
using DXGame.Models.Entities;
using DXGame.Providers.Abstract;
using DXGame.Extensions;

namespace DXGame.Controllers
{
    public class CardsController : ApiController
    {
        private readonly string acceptedExtensions = ".jpg .jpeg .png .bmp .gif";
        
        private readonly ICardsRepository _cardsRepository;
        private readonly IRootPathProvider _rootPathProvider;
        private readonly IFilenameProvider _filenameProvider;

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
                var filename = new string(file.Headers.ContentDisposition.FileName.Except(new char[] { '\\', '"' }).ToArray());
// TEST SCENARIO ISSUE (NOT REPRODUCIBLE IN PRODUCTION): QUICK HOTFIX :)
/* WTF? --> */  if (filename.EndsWith(".jp")) filename += 'g';
// END WTF
                var extension = Path.GetExtension(filename);
                if (!acceptedExtensions.Contains(extension)) continue;

                var card = await _cardsRepository.AddAsync(new Card());
                var name = _filenameProvider.GenerateFilename(card.ID, extension);

                using (var fs = File.Create(Path.Combine(storagePath, name)))
                {
                    await (await file.ReadAsStreamAsync()).CopyToAsync(fs);
                }

                card.URL = "Content/Cards/" + name;
                card = await _cardsRepository.UpdateAsync(card);
                cards.Add(card);
            }

            return cards.Count > 0 ? (IHttpActionResult)Created("Content/Cards/", cards) : BadRequest("Bad file format. Allowed extensions: " + acceptedExtensions);
        }

        public async Task<IHttpActionResult> DeleteCard(int id)
        {
            var card = await _cardsRepository.DeleteAsync(id);

            if (card != null) File.Delete(Path.Combine(_rootPathProvider.GetRoot(), card.URL));

            return card != null ? (IHttpActionResult)Ok(card) : NotFound();
        }
    }
}