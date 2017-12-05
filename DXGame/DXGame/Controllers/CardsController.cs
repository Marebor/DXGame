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
using System.Web.Http.Description;
using DXGame.Models;
using DXGame.Models.Entities;

namespace DXGame.Controllers
{
    public class CardsController : ApiController
    {
        private readonly string acceptedExtensions = "jpg, jpeg, png, bmp, gif";
        
        private ICardsRepository cardsRepository;
        
        public CardsController(ICardsRepository repository)
        {
            cardsRepository = repository;
        }

        public IEnumerable<Card> GetCards()
        {
            return cardsRepository.Cards;
        }
        
        public async Task<IHttpActionResult> GetCard(int id)
        {
            var card = await cardsRepository.FindAsync(id);
            if (card == null)
                return NotFound();

            return Ok(card.URL);
        }
        
        public async Task<IHttpActionResult> PostCard()
        {
            var filename = HttpContext.Current.Request.Files.AllKeys.FirstOrDefault();
            if (!acceptedExtensions.Contains(Path.GetExtension(filename)))
                return BadRequest($"Invalid file format. Acceptable extensions: {acceptedExtensions}");

            var file = HttpContext.Current.Request.Files[filename];
            var card = await cardsRepository.AddAsync(file.FileName, file.InputStream);
            
            return Created(card.URL, card);
        }
        
        public async Task<IHttpActionResult> DeleteCard(int id)
        {
            var card = await cardsRepository.DeleteAsync(id);

            return card != null ? (IHttpActionResult)Ok(card) : NotFound();
        }
    }
}