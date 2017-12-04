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
        private readonly string rootFolder = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Content/Cards");
        private CardsContext db = new CardsContext();
        
        public IQueryable<Card> GetCards()
        {
            return db.Cards;
        }
        
        public async Task<IHttpActionResult> GetCard(int id)
        {
            Card card = await db.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            return Redirect(card.URL);
        }
        
        public async Task<IHttpActionResult> PostCard()
        {
            var filename = HttpContext.Current.Request.Files.AllKeys.FirstOrDefault();
            if (!acceptedExtensions.Contains(Path.GetExtension(filename)))
            {
                return BadRequest($"Invalid file format. Acceptable extensions: {acceptedExtensions}");
            }
            
            var card = new Card();
            db.Cards.Add(card);

            var id_formatter = $"D{int.MaxValue.ToString().Length}";
            var name = $"Card_ID-{card.ID.ToString(id_formatter)}.{Path.GetExtension(filename)}";
            HttpContext.Current.Request.Files[filename].SaveAs(Path.Combine(rootFolder, name));

            card.URL = $"Content/Cards/{name}";
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = card.ID }, card);
        }
        
        public async Task<IHttpActionResult> DeleteCard(int id)
        {
            Card card = await db.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            File.Delete(Path.Combine(rootFolder, Path.GetFileName(card.URL)));
            db.Cards.Remove(card);
            await db.SaveChangesAsync();

            return Ok(card);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}