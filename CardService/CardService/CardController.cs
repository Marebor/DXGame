using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using FileStorage;

namespace CardService
{
    public class CardController : ApiController
    {
        private FileStorageFolder CardsFolder = new FileStorageFolder(Path.Combine(Directory.GetCurrentDirectory(), "Content", "Cards"));

        private const string CARDS_DIRECTORY = "Content/Files";
        
        public IEnumerable<int> Get()
        {
            return CardsFolder.Files.Select(f => f.ID);
        }
        public string Get(int id)
        {
            var card = CardsFolder.Files.FirstOrDefault(f => f.ID == id);
            if (card == null) throw new FileNotFoundException();

            return card.Path;
        }

        public void Post()
        {

        }
    }
}
