using System;
using System.Web.Http.Results;
using FluentAssertions;
using GigHub.Controllers.Api;
using GigHub.Core;
using GigHub.Core.Dtos;
using GigHub.Core.Models;
using GigHub.Core.Repositories;
using GigHub.Test.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GigHub.Test.Controllers.Api
{
    [TestClass]
    public class AttendancesControllerTests
    {
        private AttendancesController _controller;
        private Mock<IAttendanceRepository> _mockRepository;
        private string _userId;

        [TestInitialize]
        public void TestInitialize() // this is the constructor of the test class
        {
            _mockRepository = new Mock<IAttendanceRepository>(); 

            var mockUoW = new Mock<IUnitOfWork>(); // this is the mock version of the IUnitOfWork
            mockUoW.SetupGet(u => u.Attendances).Returns(_mockRepository.Object);

            _controller = new AttendancesController(mockUoW.Object);
            _userId = "1";
            _controller.MockCurrentUser(_userId, "user1@domain.com"); // calls the extension method instead of setting the user in the line above
        }

        [TestMethod]
        public void Attend_UserAttendingAGigForWhichHeHasAnAttendance_ShouldReturnBadRequest() // naming convention for the tests = name of the method +  action to test + expected result
        {
            var attendance = new Attendance(); // creates the attendance to mock
            _mockRepository.Setup(r => r.GetAttendance(1, _userId)).Returns(attendance);

            var result = _controller.Attend(new AttendanceDto { GigId = 1 });  //gets the attendance with GigId 1 

            result.Should().BeOfType<BadRequestErrorMessageResult>();
        }

        [TestMethod]
        public void Attend_ValidRequest_ShouldReturnOk() // name of the method + name of the action to test + expected result
        {
            var result = _controller.Attend(new AttendanceDto { GigId = 1 });

            result.Should().BeOfType<OkResult>();
        }

        [TestMethod]
        public void DeleteAttendance_NoAttendanceWithGivenIdExists_ShouldReturnNotFound() // Attendance has already been deleted
        {
            var result = _controller.DeleteAttendance(1);

            result.Should().BeOfType<NotFoundResult>();
        }

        [TestMethod]
        public void DeleteAttendance_ValidRequest_ShouldReturnOk()
        {
            var attendance = new Attendance();
            _mockRepository.Setup(r => r.GetAttendance(1, _userId)).Returns(attendance);

            var result = _controller.DeleteAttendance(1);

            result.Should().BeOfType<OkNegotiatedContentResult<int>>();
        }

        [TestMethod]
        public void DeleteAttendance_ValidRequest_ShouldReturnTheIdOfDeletedAttendance()
        {
            var attendance = new Attendance();
            _mockRepository.Setup(r => r.GetAttendance(1, _userId)).Returns(attendance);

            var result = (OkNegotiatedContentResult<int>)_controller.DeleteAttendance(1);

            result.Content.Should().Be(1);
        }
    }
}
