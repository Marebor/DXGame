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
            Directory.CreateDirectory(Path.Combine(rootFolder, directory));
            baseURL = directory;
        }

        public async Task<Card> AddAsync(HttpPostedFile file)
        {
            var card = new Card();
            db.Cards.Add(card);

            var id_formatter = $"D{int.MaxValue.ToString().Length}";
            var name = $"Card_ID-{card.ID.ToString(id_formatter)}.{Path.GetExtension(file.FileName)}";
            file.SaveAs(Path.Combine(rootFolder, baseURL, name));

            card.URL = Path.Combine(baseURL, card.ID.ToString());
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
    }
}