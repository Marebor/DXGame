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
using DXGame.Services;

namespace DXGame.Controllers
{
    public class CardsController : ApiController
    {
        private readonly string acceptedExtensions = "jpg, jpeg, png, bmp, gif";
        
        private ICardsRepository cardsRepository;
        private IRequestFileService requestFileService;

        public CardsController(ICardsRepository repository, IRequestFileService reqFileService)
        {
            cardsRepository = repository;
            requestFileService = reqFileService;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public IEnumerable<Card> GetCards()
        {
            return cardsRepository.Cards;
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public async Task<IHttpActionResult> GetCard(int id)
        {
            var card = await cardsRepository.FindAsync(id);
            if (card == null)
                return NotFound();

            return Ok(card);
        }
        
        public async Task<IHttpActionResult> PostCard()
        {
            if (HttpContext.Current == null)
            {
                return BadRequest("No context :(");
            }
            var filename = requestFileService.GetFiles().AllKeys.FirstOrDefault();
            return Created("", new Card());
            //if (!acceptedExtensions.Contains(Path.GetExtension(filename)))
            //    return BadRequest($"Invalid file format. Acceptable extensions: {acceptedExtensions}");

            //var file = HttpContext.Current.Request.Files[filename];
            //var card = await cardsRepository.AddAsync(file.FileName, file.InputStream);
            //file.SaveAs(Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath(@"~/"), card.URL));
            
            //return Created(card.URL, card);
        }
        
        public async Task<IHttpActionResult> DeleteCard(int id)
        {
            var card = await cardsRepository.DeleteAsync(id);

            return card != null ? (IHttpActionResult)Ok(card) : NotFound();
        }
    }
}