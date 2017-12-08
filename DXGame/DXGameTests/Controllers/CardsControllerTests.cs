using Microsoft.VisualStudio.TestTools.UnitTesting;
using DXGame.Controllers;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using System.Web;
using System.Web.Http;
using System.Net;
using System.Net.Http;

using Moq;
using Moq.Protected;
using Unity;
using DXGame.Models;
using DXGame.Models.Entities;
using DXGame.Providers;

using Newtonsoft.Json;

namespace DXGame.Controllers.Tests
{
    [TestClass()]
    public class CardsControllerTests
    {
        HttpClient _client;

        [TestInitialize]
        public void Init()
        {
            var config = new HttpConfiguration();

            config.EnableCors();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var container = new UnityContainer();
            var mockRepo = CreateMockRepository();
            var mockPathProvider = CreateMockPathProvider();
            container.RegisterInstance(mockRepo.Object);
            container.RegisterInstance(mockPathProvider.Object);
            container.RegisterType<IFilenameProvider, FilenameProvider>();
            container.RegisterType<INewIDProvider, NewIDProvider>();
            config.DependencyResolver = new UnityResolver(container);

            var server = new HttpServer(config);
            _client = new HttpClient(server);
        }

        [TestMethod()]
        public void GetCardsTest()
        {
            var response = _client.GetAsync("http://test/api/cards").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var obj = JsonConvert.DeserializeObject(content, typeof(IEnumerable<Card>));

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(3, (obj as IEnumerable<Card>).Count());
            Assert.AreEqual(1, (obj as IEnumerable<Card>).First().ID);
            Assert.AreEqual(3, (obj as IEnumerable<Card>).Last().ID);
        }

        [TestMethod()]
        public void GetCardTest_Existing()
        {
            var response = _client.GetAsync("http://test/api/cards/1").Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var obj = JsonConvert.DeserializeObject(content, typeof(Card));

            Assert.IsTrue(response.IsSuccessStatusCode);
            Assert.AreEqual(1, (obj as Card).ID);
            Assert.AreEqual("Content/Cards/Card_ID-0000000001.jpg", (obj as Card).URL);
        }
        
        [TestMethod()]
        public void GetCardTest_NotExisting()
        {
            var response = _client.GetAsync("http://test/api/cards/100").Result;
            Assert.IsFalse(response.IsSuccessStatusCode);
        }

        [TestMethod()]
        public void AddNewCardTest_OneFile()
        {
            System.Diagnostics.Trace.WriteLine("ELO ELO");
            var file = new MemoryStream();
            file.Write(new byte[100], 0, 100);

            var form = new MultipartFormDataContent();
            form.Add(new StreamContent(file), "image", "testImage.jpg");

            var response = _client.PostAsync("http://test/api/cards", form).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var createdCards = JsonConvert.DeserializeObject(content, typeof(List<Card>)) as List<Card>;
            
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(1, createdCards.Count);
            Assert.AreEqual(4, createdCards.First().ID);
            StringAssert.Matches(createdCards.First().URL, new Regex("Content/Cards/Card_ID-0000000004.jpg"));
        }

        [TestMethod()]
        public void AddNewCardTest_MultipleFiles()
        {
            var form = new MultipartFormDataContent();
            form.Add(new StreamContent(GetTestJPG()), "image1", "testImage1.jpg");
            form.Add(new StreamContent(GetTestJPG()), "image2", "testImage2.jpg");
            form.Add(new StreamContent(GetTestJPG()), "image3", "testImage3.jpg");

            var response = _client.PostAsync("http://test/api/cards", form).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var createdCards = JsonConvert.DeserializeObject(content, typeof(List<Card>)) as List<Card>;

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(3, createdCards.Count);
            Assert.AreEqual(4, createdCards.First().ID);
            Assert.AreEqual(6, createdCards.Last().ID);
            StringAssert.Matches(createdCards.First().URL, new Regex("Content/Cards/Card_ID-0000000004.jpg"));
            StringAssert.Matches(createdCards.Last().URL, new Regex("Content/Cards/Card_ID-0000000006.jpg"));
        }

        private Mock<ICardsRepository> CreateMockRepository()
        {
            var cards = new List<Card>
            {
                    new Card() { ID = 1, URL = "Content/Cards/Card_ID-0000000001.jpg" },
                    new Card() { ID = 2, URL = "Content/Cards/Card_ID-0000000002.jpg" },
                    new Card() { ID = 3, URL = "Content/Cards/Card_ID-0000000003.jpg" },
            };
            var mock = new Mock<ICardsRepository>();
            mock.Setup(m => m.Cards).Returns(cards);
            mock.Setup(m => m.AddAsync(It.IsAny<string>())).Returns(async (string url) => {
                await Task.Yield();
                var maxID = mock.Object.Cards.Max(c => c.ID);
                var card = new Card() { ID = maxID + 1, URL = "Content/Cards/" + new FilenameProvider().GenerateFilename(maxID + 1, ".jpg") };
                (mock.Object.Cards as List<Card>).Add(card);

                return card;
            });
            mock.Setup(m => m.FindAsync(It.IsAny<int>())).Returns(async (int id) => {
                await Task.Yield();
                return mock.Object.Cards.FirstOrDefault(c => c.ID == id);
            });
            mock.Setup(m => m.DeleteAsync(It.IsAny<int>())).Returns(async (int id) => {
                await Task.Yield();
                var card = mock.Object.Cards.FirstOrDefault(c => c.ID == id);

                if (card != null)
                {
                    (mock.Object.Cards as List<Card>).Remove(card);
                }

                return card;
            });

            return mock;
        }

        private Mock<IRootPathProvider> CreateMockPathProvider()
        {
            var mock = new Mock<IRootPathProvider>();
            mock.Setup(m => m.GetRoot()).Returns(() => Directory.GetCurrentDirectory());

            return mock;
        }

        private Stream GetTestJPG()
        {
            var file = new MemoryStream();
            file.Write(new byte[100], 0, 100);

            return file;
        }
    }
}