using Xunit;
using Moq;
using TwitchLMChatBot.Application.Services;
using TwitchLMChatBot.Application.Abstractions;
using TwitchLMChatBot.Models;
using System.Collections.Generic;
using TwitchLMChatBot.Application;

namespace TwitchLMChatBot.Tests
{
    public class AccessControlServiceTests
    {
        private readonly AccessControlService _service;
        private readonly Mock<IAccessControlRepository> _repositoryMock;

        public AccessControlServiceTests()
        {
            _repositoryMock = new Mock<IAccessControlRepository>();
            _service = new AccessControlService(_repositoryMock.Object);
        }

        [Fact]
        public void Get_ReturnsFirstAccessControl_WhenExists()
        {
            // Arrange
            var expectedAccessControl = new AccessControl { Unrestricted = true };
            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl> { expectedAccessControl });

            // Act
            var result = _service.Get();

            // Assert
            Assert.Equal(expectedAccessControl, result);
        }

        [Fact]
        public void Get_ReturnsNull_WhenNoAccessControlExists()
        {
            // Arrange
            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl>());

            // Act
            var result = _service.Get();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Set_UpdatesExistingAccessControl_WhenExists()
        {
            // Arrange
            var existingAccessControl = new AccessControl { Unrestricted = false, Followers = false, Subscribers = false, Moderators = false };
            var setRequest = new SetAccessControlRequest { Unrestricted = true, Followers = true, Subscribers = true, Moderators = true };

            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl> { existingAccessControl });

            // Act
            _service.Set(setRequest);

            // Assert
            _repositoryMock.Verify(repo => repo.Update(It.Is<AccessControl>(a =>
                a.Unrestricted == setRequest.Unrestricted &&
                a.Followers == setRequest.Followers &&
                a.Subscribers == setRequest.Subscribers &&
                a.Moderators == setRequest.Moderators
            )), Times.Once);

            _repositoryMock.Verify(repo => repo.Insert(It.IsAny<AccessControl>()), Times.Never);
        }

        [Fact]
        public void Set_InsertsNewAccessControl_WhenNoneExists()
        {
            // Arrange
            var setRequest = new SetAccessControlRequest { Unrestricted = true, Followers = true, Subscribers = true, Moderators = true };

            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl>());

            // Act
            _service.Set(setRequest);

            // Assert
            _repositoryMock.Verify(repo => repo.Insert(It.Is<AccessControl>(a =>
                a.Unrestricted == setRequest.Unrestricted &&
                a.Followers == setRequest.Followers &&
                a.Subscribers == setRequest.Subscribers &&
                a.Moderators == setRequest.Moderators
            )), Times.Once);

            _repositoryMock.Verify(repo => repo.Update(It.IsAny<AccessControl>()), Times.Never);
        }

        [Theory]
        [InlineData(true, false, false, false, true, false, false, false, false, true)] // Unrestricted access
        [InlineData(false, true, true, false, false, false, false, false, false, true)]// Follower access
        [InlineData(false, false, false, true, true, false, false, false, false, true)] // Subscriber access
        [InlineData(false, false, false, false, false, true, true, false, false, true)] // Moderator access
        [InlineData(false, false, false, false, false, false, false, true, true, true)] // Moderator access
        public void Check_ReturnsCorrectAccess(
            bool unrestricted, bool followers, bool isFollower, bool subscribers, bool isSubscriber, bool moderators, bool isModerator,
          bool vips, bool isVip,
            bool expectedResult)
        {
            // Arrange
            var accessControl = new AccessControl
            {
                Unrestricted = unrestricted,
                Followers = followers,
                Subscribers = subscribers,
                Moderators = moderators,
                Vips = vips,
            };

            var checkRequest = new CheckAccessRequest
            {
                IsFollower = isFollower,
                IsSubscriber = isSubscriber,
                IsModerator = isModerator,
                IsVip = isVip
            };

            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl> { accessControl });

            // Act
            var result = _service.Check(checkRequest);

            // Assert
            Assert.Equal(expectedResult, result);
        }



        [Fact]
        public void Check_ReturnsTrue_WhenUnrestrictedAccess()
        {
            // Arrange
            var accessControl = new AccessControl { Unrestricted = true };
            var checkRequest = new CheckAccessRequest();

            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl> { accessControl });

            // Act
            var result = _service.Check(checkRequest);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Check_ReturnsTrue_WhenFollowerAccess()
        {
            // Arrange
            var accessControl = new AccessControl { Followers = true };
            var checkRequest = new CheckAccessRequest { IsFollower = true };

            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl> { accessControl });

            // Act
            var result = _service.Check(checkRequest);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Check_ReturnsFalse_WhenFollowerAccessIsFalse()
        {
            // Arrange
            var accessControl = new AccessControl { Followers = false };
            var checkRequest = new CheckAccessRequest { IsFollower = true };

            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl> { accessControl });

            // Act
            var result = _service.Check(checkRequest);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Check_ReturnsTrue_WhenSubscriberAccess()
        {
            // Arrange
            var accessControl = new AccessControl { Subscribers = true };
            var checkRequest = new CheckAccessRequest { IsSubscriber = true };

            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl> { accessControl });

            // Act
            var result = _service.Check(checkRequest);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Check_ReturnsFalse_WhenSubscriberAccessIsFalse()
        {
            // Arrange
            var accessControl = new AccessControl { Subscribers = false };
            var checkRequest = new CheckAccessRequest { IsSubscriber = true };

            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl> { accessControl });

            // Act
            var result = _service.Check(checkRequest);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Check_ReturnsTrue_WhenModeratorAccess()
        {
            // Arrange
            var accessControl = new AccessControl { Moderators = true };
            var checkRequest = new CheckAccessRequest { IsModerator = true };

            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl> { accessControl });

            // Act
            var result = _service.Check(checkRequest);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Check_ReturnsFalse_WhenModeratorAccessIsFalse()
        {
            // Arrange
            var accessControl = new AccessControl { Moderators = false };
            var checkRequest = new CheckAccessRequest { IsModerator = true };

            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl> { accessControl });

            // Act
            var result = _service.Check(checkRequest);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Check_ReturnsTrue_WhenVipAccess()
        {
            // Arrange
            var accessControl = new AccessControl { Vips = true };
            var checkRequest = new CheckAccessRequest { IsVip = true };

            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl> { accessControl });

            // Act
            var result = _service.Check(checkRequest);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void Check_ReturnsFalse_WhenVipAccessIsFalse()
        {
            // Arrange
            var accessControl = new AccessControl { Vips = false };
            var checkRequest = new CheckAccessRequest { IsVip = true };

            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl> { accessControl });

            // Act
            var result = _service.Check(checkRequest);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void Check_ReturnsFalse_WhenNoAccessConditionsMet()
        {
            // Arrange
            var accessControl = new AccessControl();
            var checkRequest = new CheckAccessRequest();

            _repositoryMock.Setup(repo => repo.FindAll()).Returns(new List<AccessControl> { accessControl });

            // Act
            var result = _service.Check(checkRequest);

            // Assert
            Assert.False(result);
        }

    }
}