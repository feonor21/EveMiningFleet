using EveMiningFleet.API.Controllers;
using EveMiningFleet.API.Models;
using EveMiningFleet.API.Services;
using EveMiningFleet.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using static EveMiningFleet.API.Services.FleetService;

namespace EveMiningFleet.API.Test.Controllers
{
    [TestFixture]
    public class FleetControllerTest
    {
        private EveMiningFleetContext eveMiningFleetContext;
        private FleetController mycontroller;

        [SetUp]
        public void Setup()
        {
            
            var options = new DbContextOptionsBuilder<EveMiningFleetContext>()
                .UseInMemoryDatabase(databaseName: "EveMiningFleetInMemory"+Guid.NewGuid().ToString())
                .Options;
            eveMiningFleetContext = new EveMiningFleetContext(options);
            


            mycontroller = new FleetController(eveMiningFleetContext);

        }

        
        [Test]
        public void Get_WithoutTokenAndTypeViewAndId_ShouldReturnBadRequest()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);
            // Act
            var result = mycontroller.Get(null, null);

            // Assert            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public void Get_WithTokenWithoutTypeViewAndId_ShouldReturnBadRequest()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);
            TestUtility.CorrectToken( mycontroller, TestUtility.johndoeId);
            // Act
            var result = mycontroller.Get(null, null);

            // Assert            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public void Get_WithTokenAndTypeViewAndId_ShouldReturnBadRequest()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);
            TestUtility.CorrectToken( mycontroller, TestUtility.johndoeId);

            // Act
            var result = mycontroller.Get(typeview.viewPublic, TestUtility.fleetIdTestPublicId);

            // Assert            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public void Get_WithoutTokenWithTypeViewAndId_ShouldReturnBadRequest()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);

            // Act
            var result = mycontroller.Get(typeview.viewPublic, TestUtility.fleetIdTestPublicId);

            // Assert            
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }


        [Test]
        public void Get_WithIdAndToken_ShouldReturnFleetModel()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);
            TestUtility.CorrectToken( mycontroller, TestUtility.johndoeId);

            // Act
            var result = mycontroller.Get(null, TestUtility.fleetid);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var resultType = result as OkObjectResult;
            Assert.IsInstanceOf<List<FleetModel>>(resultType.Value);
            var convertedResult = resultType.Value as List<FleetModel>;
            Assert.IsTrue(convertedResult.Count()==1);
        }
        [Test]
        public void Get_WithIdAndUnauthorizedToken_ShouldReturnUnauthorized()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);
            TestUtility.CorrectToken( mycontroller, TestUtility.CharacterIdTestPrivateId);

            // Act
            var result = mycontroller.Get(null, TestUtility.fleetid);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
        }
        [Test]
        public void Get_WithIdAndEmptyToken_ShouldReturnUnauthorized()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);

            // Act
            var result = mycontroller.Get(null, TestUtility.fleetid);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
        }


        [Test]
        public void Get_WithTypeViewButNoData_ShouldReturnNoContent()
        {
            // Arrange
            TestUtility.CorrectToken( mycontroller, TestUtility.johndoeId);

            // Act
            var result = mycontroller.Get(typeview.viewPublic, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<NoContentResult>(result);
        }


        [Test]
        public void Get_WithPublicViewAndCorrectToken_ShouldReturnFleetModels()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);
            TestUtility.CorrectToken( mycontroller, TestUtility.johndoeId);

            // Act
            var result = mycontroller.Get(typeview.viewPublic, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var resultType = result as OkObjectResult;
            Assert.IsInstanceOf<List<FleetModel>>(resultType.Value);
            var convertedResult = resultType.Value as List<FleetModel>;
            Assert.IsTrue(convertedResult.Count()>0);
        }
        [Test]
        public void Get_WithPublicViewAndUnauthorizedToken_ShouldReturnFleetModels()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);
            TestUtility.CorrectToken( mycontroller, TestUtility.CharacterIdTestPrivateId);

            // Act
            var result = mycontroller.Get(typeview.viewPublic, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var resultType = result as OkObjectResult;
            Assert.IsInstanceOf<List<FleetModel>>(resultType.Value);
            var convertedResult = resultType.Value as List<FleetModel>;
            Assert.IsTrue(convertedResult.Count()>0);
        }
        [Test]
        public void Get_WithPublicViewAndEmptyToken_ShouldReturnFleetModels()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);
            // Act
            var result = mycontroller.Get(typeview.viewPublic, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var resultType = result as OkObjectResult;
            Assert.IsInstanceOf<List<FleetModel>>(resultType.Value);
            var convertedResult = resultType.Value as List<FleetModel>;
            Assert.IsTrue(convertedResult.Count()>0);
        }


        [Test]
        public void Get_WithCoorpoViewAndCorectToken_ShouldReturnFleetModels()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);
            TestUtility.CorrectToken( mycontroller, TestUtility.johndoeId);

            // Act
            var result = mycontroller.Get(typeview.viewCorporation, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var resultType = result as OkObjectResult;
            Assert.IsInstanceOf<List<FleetModel>>(resultType.Value);
            var convertedResult = resultType.Value as List<FleetModel>;
            Assert.IsTrue(convertedResult.Count()>0);
            Assert.IsTrue(convertedResult.First().Character.Id == TestUtility.CharacterIdTestCoorpoId);
            Assert.IsTrue(convertedResult.First().Id == TestUtility.fleetIdTestCoorpoId);
        }   
        [Test]
        public void Get_WithCoorpoViewAndEmptyToken_ShouldReturnUnauthorized()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);

            // Act
            var result = mycontroller.Get(typeview.viewCorporation, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
        }


        [Test]
        public void Get_WithAllianceViewAndCorrectToken_ShouldReturnFleetModels()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);
            TestUtility.CorrectToken( mycontroller, TestUtility.johndoeId);

            // Act
            var result = mycontroller.Get(typeview.viewAlliance, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var resultType = result as OkObjectResult;
            Assert.IsInstanceOf<List<FleetModel>>(resultType.Value);
            var convertedResult = resultType.Value as List<FleetModel>;
            Assert.IsTrue(convertedResult.Count()>0);
            Assert.IsTrue(convertedResult.First().Character.Id == TestUtility.CharacterIdTestAllianceId);
            Assert.IsTrue(convertedResult.First().Id == TestUtility.fleetIdTestAllianceId);
        }
        [Test]
        public void Get_WithAllianceViewAndEmptyToken_ShouldReturnUnauthorized()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);

            // Act
            var result = mycontroller.Get(typeview.viewAlliance, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
        }


        [Test]
        public void Get_WithPrivateViewAndCorectToken_ShouldReturnFleetModels()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);
            TestUtility.CorrectToken( mycontroller, TestUtility.johndoeId);

            // Act
            var result = mycontroller.Get(typeview.viewPrivate, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var resultType = result as OkObjectResult;
            Assert.IsInstanceOf<List<FleetModel>>(resultType.Value);
            var convertedResult = resultType.Value as List<FleetModel>;
            Assert.IsTrue(convertedResult.Count()>0);
            Assert.IsTrue(convertedResult.First().Character.Id == TestUtility.johndoeId);
            Assert.IsTrue(convertedResult.First().Id == TestUtility.fleetid);
        }
        [Test]
        public void Get_WithPrivateViewAndIncorectToken_ShouldReturnUnauthorized()
        {
            // Arrange
            TestUtility.populateFleet(eveMiningFleetContext);

            // Act
            var result = mycontroller.Get(typeview.viewPrivate, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
        }




        [Test]
        public void CreateFleet_WithOutTokenAndIncorrectModel_ShouldReturnUnauthorized()
        {
        }
        [Test]
        public void CreateFleet_WithTokenAndIncorrectModel_ShouldBadRequest()
        {
        }
        [Test]
        public void CreateFleet_WithInvalidTokenAndCorrectModel_ShouldReturnUnauthorized()
        {
        }
        [Test]
        public void CreateFleet_WithTokenAndCorrectModel_ShouldFleetModel()
        {
        }
   





    }
}
