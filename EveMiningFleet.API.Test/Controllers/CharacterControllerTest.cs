using EveMiningFleet.API.Controllers;
using EveMiningFleet.API.Models;
using EveMiningFleet.API.Services;
using EveMiningFleet.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace EveMiningFleet.API.Test.Controllers
{
    [TestFixture]
    public class CharacterControllerTest
    {
        private EveMiningFleetContext eveMiningFleetContext;
        private CharacterController mycontroller;



        [SetUp]
        public void Setup()
        {
            
            var options = new DbContextOptionsBuilder<EveMiningFleetContext>()
                .UseInMemoryDatabase(databaseName: "EveMiningFleetInMemory"+Guid.NewGuid().ToString())
                .Options;
            eveMiningFleetContext = new EveMiningFleetContext(options);

            mycontroller = new CharacterController(eveMiningFleetContext);

        }

 
        [Test]
        public void Get_WithBothIdAndNameAreNull_ShouldReturn400BadRequest()
        {
            // Act
            var result = mycontroller.Get(null, null);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

        }
        [Test]
        public void Get_WithBothIdAndNameAreProvided_ShouldReturn400BadRequest()
        {
            //Arrange
            TestUtility.populateCharacter(eveMiningFleetContext);

            // Act
            var result = mycontroller.Get(1, "test");

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
        [Test]
        public void Get_WithIdIsProvided_ShouldReturn200Ok()
        {
            //Arrange
            TestUtility.populateCharacter(eveMiningFleetContext);
            // Act
            var result = mycontroller.Get(TestUtility.johndoeId, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var resultType = result as OkObjectResult;
            Assert.IsInstanceOf<CharacterModel>(resultType.Value);
            var convertedResult = resultType.Value as CharacterModel;
            Assert.That(convertedResult.Id, Is.EqualTo(TestUtility.johndoeId));
            Assert.That(convertedResult.Name, Is.EqualTo(TestUtility.johndoename));
        }
        [Test]
        public void Get_WithNameIsProvided_ShouldReturn200Ok()
        {
            //Arrange
            TestUtility.populateCharacter(eveMiningFleetContext);

            // Act
            var result = mycontroller.Get(null, TestUtility.johndoename);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var resultType = result as OkObjectResult;
            Assert.IsInstanceOf<List<CharacterModel>>(resultType.Value);
            var convertedResult = resultType.Value as List<CharacterModel>;
            Assert.IsTrue(convertedResult.Count()>=1);
            Assert.IsTrue(convertedResult.Any(x=>x.Id==TestUtility.johndoeId));
        }
        [Test]
        public void Get_WithIdIsNotFound_ShouldReturn404NotFound()
        {
            // Act
            var result = mycontroller.Get(TestUtility.johndoeId, null);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }
        [Test]
        public void Get_WithNameIsNotFound_ShouldReturn404NotFound()
        {
            // Act
            var result = mycontroller.Get(null, TestUtility.johndoename);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }


        [Test]
        public void GetMyCharacters_WithCalledWithValidToken_ShouldReturns200Ok()
        {
            //Arrange
            TestUtility.populateCharacter(eveMiningFleetContext);
            TestUtility.CorrectToken( mycontroller, TestUtility.johndoeId);

            // Act
            var result = mycontroller.GetMyCharacters();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var resultType = result as OkObjectResult;
            Assert.IsInstanceOf<List<CharacterModel>>(resultType.Value);
            var convertedResult = resultType.Value as List<CharacterModel>;
            Assert.IsTrue(convertedResult.Count()>1);
            Assert.IsTrue(convertedResult.Any(x=>x.Id==TestUtility.johndoeId));
        }
        [Test]
        public void GetMyCharacters_WithCalledWithoutValidToken_ShouldReturns401Unauthorized()
        {
            //Arrange
            TestUtility.populateCharacter(eveMiningFleetContext);

            // Act
            var result = mycontroller.GetMyCharacters();

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
        }


        [Test]
        public void SetMain_WithCalledWithValidTokenWithGoodId_ShouldReturns200Ok()
        {
            Environment.SetEnvironmentVariable("JWTToken", "dsadsadsadsadsadsa");
            //Arrange
            TestUtility.populateCharacter(eveMiningFleetContext);
            TestUtility.CorrectToken( mycontroller, TestUtility.johndoeId);

            // Act
            var result = mycontroller.SetMain(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<OkObjectResult>(result);
            var resultType = result as OkObjectResult;

            Assert.IsInstanceOf<string>(resultType.Value);
            var convertedResult = resultType.Value as string;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(convertedResult) as JwtSecurityToken;


            Assert.IsTrue(jwtToken.Claims.Any(x => x.Type == TokenService.characterIdClaimKey && x.Value == "2"));

            Assert.IsTrue(eveMiningFleetContext.characters.Any(x=>x.Id==TestUtility.johndoeId && x.CharacterMainId==2));
            Assert.IsTrue(eveMiningFleetContext.characters.Any(x=>x.Id==1 && x.CharacterMainId==2));
            Assert.IsTrue(eveMiningFleetContext.characters.Any(x=>x.Id==2 && x.CharacterMainId==2));
            Assert.IsTrue(eveMiningFleetContext.characters.Any(x=>x.Id==3 && x.CharacterMainId==2));
        }

        [Test]
        public void SetMain_WithCalledWithValidTokenWithoutGoodId_ShouldReturn400BadRequest()
        {
            //Arrange
            TestUtility.populateCharacter(eveMiningFleetContext);
            TestUtility.CorrectToken( mycontroller, TestUtility.johndoeId);

            // Act
            var result = mycontroller.SetMain(TestUtility.CharacterIdTestPrivateId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public void SetMain_WithCalledWithoutValidToken_ShouldReturns401Unauthorized()
        {
            // Act
            var result = mycontroller.SetMain(TestUtility.johndoeId);

            // Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result);
        }

    }
}
