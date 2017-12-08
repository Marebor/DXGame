using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DXGame.Providers;
using DXGame.Models.Entities;

namespace DXGame.Models
{
    public class CardsRepository : ICardsRepository
    {
        private CardsContext db = new CardsContext();
        public IEnumerable<Card> Cards
        {
            get { return db.Cards; }
        }
        
        public async Task<Card> AddAsync(string url)
        {
            var card = new Card();
            db.Cards.Add(card);
            await db.SaveChangesAsync();

            card.URL = url;
            await db.SaveChangesAsync();

            return card;
        }

        public async Task<Card> DeleteAsync(int id)
        {
            var card = await db.Cards.FindAsync(id);

            if (card != null)
            {
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