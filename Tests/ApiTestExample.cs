using RestSharp;
using Q.Models;
using Q.Services;

namespace Q.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class ApiTestExample
    {
        private DogService _dogService;

        [SetUp]
        public void Setup()
        {
            // Initialize with your API base URL
            _dogService = new DogService("https://dogapi.dog/api/v2");            
        }

        [Test]
        public void GetDogById_ShouldReturnDog1()
        {
            string dogId = "68f47c5a-5115-47cd-9849-e45d3c378f12";

            RestResponse response = _dogService.GetById(dogId);
            Dog dog = _dogService.GetById(response, dogId);

            string expectedName = "Caucasian Shepherd Dog";
            string actualName = dog.Attributes.Name;
            string actualDogId = dog.Id.ToString();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode.ToString(), Is.EqualTo("OK"));
                Assert.That(actualName, Is.EqualTo(expectedName), "The breed name is not correct.");
                Assert.That(actualDogId, Is.EqualTo(dogId), $"The dog id is not correct.");
            });                      
        }

        [Test]
        public void GetDogById_ShouldReturnDog2()
        {
            string dogId = "4ddbe251-72af-495e-8e9d-869217e1d92a";

            RestResponse response = _dogService.GetById(dogId);
            Dog dog = _dogService.GetById(response, dogId);

            string expectedName = "Bouvier des Flandres";
            string actualName = dog.Attributes.Name;
            string actualDogId = dog.Id.ToString();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode.ToString(), Is.EqualTo("OK"));
                Assert.That(actualName, Is.EqualTo(expectedName), "The breed name is not correct.");
                Assert.That(actualDogId, Is.EqualTo(dogId), $"The dog id is not correct.");
            });
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up resources
            _dogService = null;
        }
    }
}