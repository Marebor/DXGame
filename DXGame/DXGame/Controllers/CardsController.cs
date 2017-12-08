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

namespace DXGame.Controllers
{
    public class CardsController : ApiController
    {
        private readonly string acceptedExtensions = "jpg, jpeg, png, bmp, gif";
        
        private ICardsRepository _cardsRepository;
        private IRootPathProvider _rootPathProvider;

        public CardsController(ICardsRepository repository, IRootPathProvider pathProvider)
        {
            _cardsRepository = repository;
            _rootPathProvider = pathProvider;
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
            var storagePath = Path.Combine(_rootPathProvider.GetRoot(), "Content", "Cards");
            var injector = Configuration.DependencyResolver;
            var provider = new CardMultipartFormDataStreamProvider(
                storagePath, 
                injector.GetService(typeof(INewIDProvider)) as INewIDProvider, 
                injector.GetService(typeof(IFilenameProvider)) as IFilenameProvider
                );
            
            provider = await Request.Content.ReadAsMultipartAsync(provider);

            var cards = new List<Card>();
            foreach (var file in provider.FileData)
            {
                var card = await _cardsRepository.AddAsync("Content/Cards/" + file.LocalFileName);
                cards.Add(card);
            }

            return Created("Content/Cards/", cards);
        }
        
        public async Task<IHttpActionResult> DeleteCard(int id)
        {
            var card = await _cardsRepository.DeleteAsync(id);

            return card != null ? (IHttpActionResult)Ok(card) : NotFound();
        }
    }
}