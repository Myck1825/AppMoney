using AppMoney.Controllers;
using AppMoney.Model.Applications.Commands.CreateApplicationCommand;
using AppMoney.Respose.ApiResponse;
using MediatR;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppMoney.Respose.Enums;
using System.ComponentModel.DataAnnotations;
using AppMoney.Model.Applications.Queries.GetApplicationQuery;
using AppMoney.Respose.Applications;
using AppMoney.Model.Applications.Queries.GetListByFilterApplicationQuery;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AppMoney.UnitTests.Controllers
{
    [TestFixture]
    public class ApplicationControllerTests
    {
        private ApplicationController _controller;
        private Mock<IMediator> _mediatorMock;
        private const string IP_ADDRESS = "192.168.1.1";

        [SetUp]
        public void SetUp()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new ApplicationController(_mediatorMock.Object);

            var httpContext = new DefaultHttpContext();
            httpContext.Items[nameof(CreateApplicationCommand.IpAddress)] = IP_ADDRESS;

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        #region CreateAsync application

        [Test]
        public async Task CreateAsync_ShouldReturnSuccessServerResponse_WhenCalled()
        {
            // Arrange
            var command = new CreateApplicationCommand { DepartmentAddress  = nameof(CreateApplicationCommand) };
            var expectedResponse = new ServerResponse<Guid> 
            { 
                Result = Guid.NewGuid(),
                IsSuccess = true,
                Code = Code.Ok
            };
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateAsync(command);

            // Assert
            _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);

            result.Result.Should().Be(expectedResponse.Result);
            result.Code.Should().Be(expectedResponse.Code);
            result.Message.Should().BeNull();
            result.IsSuccess.Should().BeTrue();
        }

        [Test]
        public async Task CreateAsync_ShouldReturnUnsuccessServerResponse_WhenCalled()
        {
            // Arrange
            var command = new CreateApplicationCommand { DepartmentAddress = nameof(CreateApplicationCommand) };
            string message = nameof(Exception);
            var expectedResponse = new ServerResponse<Guid>
            {
                IsSuccess = false,
                Code = Code.BadRequest,
                Message = message
            };
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateAsync(command);

            // Assert
            _mediatorMock.Verify(x => x.Send(command, It.IsAny<CancellationToken>()), Times.Once);

            result.Result.Should().BeEmpty();
            result.Code.Should().Be(expectedResponse.Code);
            result.Message.Should().Be(message);
            result.IsSuccess.Should().BeFalse();
        }

        [Test]
        public async Task CreateAsync_ShouldAssignIpAddressForClientIp_WhenCalled()
        {
            // Arrange
            var command = new CreateApplicationCommand { DepartmentAddress = nameof(CreateApplicationCommand) };
            string message = nameof(Exception);
            var expectedResponse = new ServerResponse<Guid>
            {
                IsSuccess = false,
                Code = Code.BadRequest,
                Message = message
            };
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                          .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateAsync(command);

            // Assert
            command.IpAddress.Should().Be(IP_ADDRESS);

        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task CreateAsync_ShouldThrowExceptionIfDepartamentAddressIsNotValid_WhenCalled(string? departmentAddress)
        {
            // Arrange
            var command = new CreateApplicationCommand { DepartmentAddress = departmentAddress! };

            // Act
            var result = async () => await _controller.CreateAsync(command);

            // Assert
            await result.Should().ThrowAsync<ValidationException>();
            _mediatorMock.Verify(x=>x.Send(command, It.IsAny<CancellationToken>()), Times.Never);

        }
        #endregion

        #region GetAsync by id

        [Test]
        public async Task GetAsync_ShouldReturnSuccessResponse()
        {
            // Arrange
            var appId = Guid.NewGuid();
            var expectedResponse = new ServerResponse<ApplicationResponse> 
            {
                Result = new ApplicationResponse
                {
                    Currency = string.Empty,
                    Status = string.Empty
                },
                IsSuccess = true,
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetByIdApplicationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAsync(appId);

            // Assert
            _mediatorMock.Verify(x => x.Send(It.IsAny<GetByIdApplicationQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public async Task GetAsync_ShouldReturnNullInResponse_IfIdNotExist()
        {
            // Arrange
            var appId = Guid.NewGuid();
            var expectedResponse = new ServerResponse<ApplicationResponse>
            {
                IsSuccess = false,
                Message = nameof(Exception)
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetByIdApplicationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAsync(appId);

            // Assert
            _mediatorMock.Verify(x => x.Send(It.IsAny<GetByIdApplicationQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            result.Result.Should().BeNull();
            result.Should().BeEquivalentTo(expectedResponse);
        }

        #endregion

        #region GetListByFilterAsync

        [Test]
        public async Task GetListByFilterAsync_ShouldReturnSuccessResponse()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            string departamentAddress = nameof(departamentAddress);
            var expectedResponse = new ServerResponse<IReadOnlyList<ApplicationResponse>> 
            { 
                Result = new List<ApplicationResponse>(),
                IsSuccess = true,
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetListByFilterApplicationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetListByFilterAsync(clientId, departamentAddress);

            // Assert
            _mediatorMock.Verify(x => x.Send(It.IsAny<GetListByFilterApplicationQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        public async Task GetListByFilterAsync_ShouldReturnEmptyListInResponse_IfNotFound()
        {
            // Arrange
            var clientId = Guid.NewGuid();
            string departamentAddress = nameof(departamentAddress);
            var expectedResponse = new ServerResponse<IReadOnlyList<ApplicationResponse>>()
            {
                IsSuccess = false,
                Message = nameof(Exception),
                Code = Code.NotFound,
                Result = []
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetListByFilterApplicationQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetListByFilterAsync(clientId, departamentAddress);

            // Assert
            _mediatorMock.Verify(x => x.Send(It.IsAny<GetListByFilterApplicationQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            result.Result.Should().NotBeNull();
            result.Result.Should().BeEmpty();
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Test]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(null)]
        public async Task GetListByFilterAsync_ShouldReturnBadResponse_IfMessageIsNotValid(string? departamentAddress)
        {
            // Arrange
            var clientId = Guid.NewGuid();
            //var expectedResponse = new ServerResponse<IReadOnlyList<ApplicationResponse>>()
            //{
            //    IsSuccess = false,
            //    Message = nameof(Exception),
            //    Code = Code.BadRequest
            //};
            //_mediatorMock.Setup(m => m.Send(It.IsAny<GetListByFilterApplicationQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(expectedResponse);

            // Act
            var result = async () => await _controller.GetListByFilterAsync(clientId, departamentAddress!);

            // Assert
            await result.Should().ThrowAsync<ValidationException>();
            _mediatorMock.Verify(x => x.Send(It.IsAny<GetByIdApplicationQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

    }
}
