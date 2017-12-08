using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DXGame.Models;
using DXGame.Models.Entities;

namespace DXGame.Providers
{
    public class NewIDProvider : INewIDProvider
    {
        private ICardsRepository _cardsRepository;

        public NewIDProvider(ICardsRepository cardsRepository)
        {
            _cardsRepository = cardsRepository;
        }
        public int GetID()
        {
            int max;

            checked
            {
                max = _cardsRepository.Cards.Max(c => c.ID) + 1;
            }

            return max;
        }
    }
}