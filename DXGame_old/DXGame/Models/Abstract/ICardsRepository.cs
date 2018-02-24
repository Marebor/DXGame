using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

using DXGame.Models.Entities;

namespace DXGame.Models.Abstract
{
    public interface ICardsRepository
    {
        IEnumerable<Card> Cards { get; }
        Task<Card> AddAsync(Card card);
        Task<Card> DeleteAsync(int id);
        Task<Card> FindAsync(int id);
        Task<Card> UpdateAsync(Card card);
    }
}
