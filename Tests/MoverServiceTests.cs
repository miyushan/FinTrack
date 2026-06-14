using FinTrack.Domain;
using FinTrack.ExternalServices.Interfaces;
using FinTrack.Repositories.Interfaces;
using FinTrack.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FinTrack.Test.Services
{
    public class MoverServiceTests
    {
        private readonly Mock<IMoverRepository> _mockRepository;
        private readonly Mock<IStockApiClient> _mockApiClient;
        private readonly MoverService _moverService;

        public MoverServiceTests()
        {
            _mockRepository = new Mock<IMoverRepository>();
            _mockApiClient = new Mock<IStockApiClient>();
            _moverService = new MoverService(_mockRepository.Object, _mockApiClient.Object);
        }

        [Fact]
        public async Task GetTopMoversAsync_WhenMoversDoesExistInDb_ShouldNotCallExternalApi()
        {
            //Arrange
            var topMovers = new List<Mover>
            {
                new Mover { Ticker = "GRAF+", Price = 0.43m },
                new Mover { Ticker = "IVDAW", Price = 0.175m }
            };
            _mockRepository.Setup(r => r.GetTopMoversAsync()).ReturnsAsync(topMovers);

            //Act
            var result = await _moverService.GetTopMoversAsync();

            //Assert
            result.Should().NotBeNull();
            _mockApiClient.Verify(c => c.FetchTopMoversAsync(), Times.Never);
        }
    }
}
