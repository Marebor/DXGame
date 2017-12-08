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

        public async Task<Card> AddAsync(Card card)
        {
            if (card == null || await FindAsync(card.ID) != null) return null;
            db.Cards.Add(card);
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

        public async Task<Card> UpdateAsync(Card card)
        {
            var entity = await FindAsync(card.ID);
            if (entity != null)
            {
                entity.URL = card.URL;
                db.SaveChanges();
            }
            return entity;
        }
    }
}