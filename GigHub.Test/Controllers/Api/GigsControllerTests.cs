
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http.Results;
using FluentAssertions;
using GigHub.Controllers.Api;
using GigHub.Core;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Test.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GigHub.Test.Controllers.Api
{
    [TestClass]
    public class GigsControllerTests
    {
        private GigsController _gigsController;
        private Mock<IGigRepository> _mockRepository;
        private string _userId;

        //public GigsControllerTests() // this is the constructor of the test class
        [TestInitialize]
        public void TestInitialize() // this is the constructor of the test class
        {
            _mockRepository = new Mock<IGigRepository>();

            var mockUoW = new Mock<IUnitOfWork>(); // this is the mock version of the IUnitOfWork
            mockUoW.SetupGet(U => U.Gigs).Returns(_mockRepository.Object);

            _gigsController = new GigsController(mockUoW.Object);
            //_gigsController.User = principal; // sets the controller user to the generic principal object created 
            _userId = "1";
            _gigsController.MockCurrentUser(_userId,"user1@gighub.com");// calls the extension method instead of setting the user in the line above
        }

        [TestMethod]
        public void Cancel_NoGigWithGivenIdExist_ShouldReturnNotFound() // Name of the method + naming convention for action to test + expected result
        {
            var result = _gigsController.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_GigIsCanceled_ShouldReturnNotFound()
        {
            var gig = new Gig(); // creates new gig
            gig.Cancel();

            _mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig); //gets the gig with attendees and returns the canceled gig

            var result = _gigsController.Cancel(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void Cancel_UserCancelingAnotherUsersGig_ShouldReturnUnauthorized()
        {
            var gig = new Gig { ArtistId = _userId + "-" }; // creates new gig to test with the fake userId

            _mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig); //gets the gig with attendees and returns the canceled gig

            var result = _gigsController.Cancel(1);

            result.Should().BeOfType<UnauthorizedResult>();
        }

        [TestMethod]
        public void Cancel_ValidRequest_ShouldReturnOk()
        {
            var gig = new Gig { ArtistId = _userId }; // creates new gig to test with the correct userId

            _mockRepository.Setup(r => r.GetGigWithAttendees(1)).Returns(gig); //gets the gig with attendees and returns the canceled gig

            var result = _gigsController.Cancel(1);

            result.Should().BeOfType<OkResult>();
        }
    }
}
