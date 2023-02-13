using EveMiningFleet.API.Controllers;
using EveMiningFleet.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EveMiningFleet.API.Test.Controllers
{
    [TestFixture]
    public class DatapriceControllerTest
    {
        private EveMiningFleetContext eveMiningFleetContext;
        private DatapriceController mycontroller;



        [SetUp]
        public void Setup()
        {
            
            var options = new DbContextOptionsBuilder<EveMiningFleetContext>()
                .UseInMemoryDatabase(databaseName: "EveMiningFleetInMemory"+Guid.NewGuid().ToString())
                .Options;
            eveMiningFleetContext = new EveMiningFleetContext(options);

            mycontroller = new DatapriceController(eveMiningFleetContext);

        }

        [Test]
        public void Get_WhenTypeIdIsNull_ShouldReturn200OK()
        {
            //Arrange
            TestUtility.populateoreEtDataprice(eveMiningFleetContext);
            // Act
            var result = mycontroller.Get(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var resultType = result as OkObjectResult;
            Assert.AreEqual(200, resultType.StatusCode);
            Assert.IsInstanceOf<List<Entities.DbSet.DataPrice>>(resultType.Value);
            var convertedResult = resultType.Value as List<Entities.DbSet.DataPrice>;
            Assert.IsTrue(convertedResult.Count()>1);
        }

        [Test]
        public void Get_WhenTypeIdIsNull_ShouldReturn204NoContent()
        {
            // Act
            var result = mycontroller.Get(null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);

        }

        [Test]
        public void Get_WhenTypeIdIsProvided_ShouldReturn200OK()
        {
            //Arrange
            TestUtility.populateoreEtDataprice(eveMiningFleetContext);
            // Act
            var result = mycontroller.Get(TestUtility.arkonorId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var resultType = result as OkObjectResult;
            Assert.AreEqual(200, resultType.StatusCode);
            Assert.IsInstanceOf<List<Entities.DbSet.DataPrice>>(resultType.Value);
            var convertedResult = resultType.Value as List<Entities.DbSet.DataPrice>;
            Assert.IsTrue(convertedResult.Count()==TestUtility.arkonorId);
            Assert.IsTrue(convertedResult.First().Name==TestUtility.arkonorName);
        } 
        [Test]
        public void Get_WhenTypeIdIsProvided_ShouldReturn404NotFound()
        {
            // Act
            var result = mycontroller.Get(TestUtility.arkonorId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        
    }
}
