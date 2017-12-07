using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DXGame.Models.Entities;

namespace DXGame.Models
{
    public class FolderCardsRepository : ICardsRepository
    {
        private CardsContext db = new CardsContext();
        private string baseURL;
        private string rootFolder = System.Web.Hosting.HostingEnvironment.MapPath(@"~/");
        public IEnumerable<Card> Cards
        {
            get { return db.Cards; }
        }

        public FolderCardsRepository(string directory)
        {
            Directory.CreateDirectory(Path.Combine(rootFolder ?? Directory.GetCurrentDirectory(), directory));
            baseURL = directory;
        }

        public async Task<Card> AddAsync(string filename, Stream content)
        {
            if (content == null || content.Length == 0) return null;

            var card = new Card();
            db.Cards.Add(card);
            await db.SaveChangesAsync();

            //var fullname = Path.Combine(rootFolder, name);

            //using (var fs = new FileStream(fullname, FileMode.CreateNew))
            //{
            //    await content.CopyToAsync(fs);
            //}

            card.URL = baseURL + "/" + GenerateFilename(card.ID, Path.GetExtension(filename));
            await db.SaveChangesAsync();

            return card;
        }

        public async Task<Card> DeleteAsync(int id)
        {
            var card = await db.Cards.FindAsync(id);

            if (card != null)
            {
                File.Delete(Path.Combine(rootFolder, Path.GetFileName(card.URL)));
                db.Cards.Remove(card);
                await db.SaveChangesAsync();
            }
            
            return card;
        }

        public async Task<Card> FindAsync(int id)
        {
            return await db.Cards.FindAsync(id);
        }

        public static string GenerateFilename(int id, string extension)
        {
            var id_formatter = $"D{int.MaxValue.ToString().Length}";
            return $"Card_ID-{id.ToString(id_formatter)}{extension}";
        }
    }
}