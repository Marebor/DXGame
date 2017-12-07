using Microsoft.VisualStudio.TestTools.UnitTesting;
using DXGame.Controllers;
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

using Moq;
using Moq.Protected;
using Unity;
using DXGame.Models;
using DXGame.Models.Entities;
using DXGame.Services;

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
            //var mockReqFileServie = CreateMockRequestFileService();
            container.RegisterInstance(mockRepo.Object);
            //container.RegisterInstance(mockReqFileServie.Object);
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
        public void AddNewCardTest_NonEmptyFile()
        {
            var file = new MemoryStream();
            file.Write(new byte[100], 0, 100);

            var form = new MultipartFormDataContent();
            form.Add(new StreamContent(file), "image", "testImage.jpg");

            var response = _client.PostAsync("http://test/api/cards", form).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            var obj = JsonConvert.DeserializeObject(content, typeof(Card));

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Assert.AreEqual(4, (obj as Card).ID);
            Assert.AreEqual("Content/Cards/Card_ID-0000000004.jpg", (obj as Card).URL);
        }

        private Mock<ICardsRepository> CreateMockRepository()
        {
            var mock = new Mock<ICardsRepository>();
            mock.Setup(m => m.Cards).Returns(new Card[]
            {
                    new Card() { ID = 1, URL = "Content/Cards/Card_ID-0000000001.jpg" },
                    new Card() { ID = 2, URL = "Content/Cards/Card_ID-0000000002.jpg" },
                    new Card() { ID = 3, URL = "Content/Cards/Card_ID-0000000003.jpg" },
            });
            mock.Setup(m => m.AddAsync(It.IsAny<string>(), It.IsAny<Stream>())).Returns(async (string filename, Stream content) => {
                await Task.Yield();
                var maxID = mock.Object.Cards.Max(c => c.ID);
                maxID++;

                return new Card() { ID = maxID, URL = "Content/Cards/" + FolderCardsRepository.GenerateFilename(maxID, ".jpg") };
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

        //private Mock<IRequestFileService> CreateMockRequestFileService()
        //{
        //    var mock = new Mock<IRequestFileService>();
        //    mock.Setup(m => m.GetFiles()).Returns(() =>
        //    {
        //        var content = new MemoryStream();
        //        content.Write(new byte[] { 1, 2, 3 }, 0, 3);

        //        return (content, "testFile.jpg");
        //    });

        //    var test = new Mock<HttpPostedFile>();
        //    test.Setup(m => m.SaveAs(It.IsAny<string>())).Verifiable();

        //    return mock;
        //}
    }
}