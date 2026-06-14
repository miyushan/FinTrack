using FinTrack.Domain;
using FinTrack.ExternalServices.Interfaces;
using FinTrack.Repositories.Interfaces;
using FinTrack.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Xunit;
using FluentAssertions;

namespace FinTrack.Test.Services
{
    public class CompanyServiceTests
    {
        private readonly Mock<ICompanyRepository> _mockRepository;
        private readonly Mock<IStockApiClient> _mockApiClient;
        private readonly CompanyService _companyService;

        public CompanyServiceTests()
        {
            _mockRepository = new Mock<ICompanyRepository>();
            _mockApiClient = new Mock<IStockApiClient>();
            _companyService = new CompanyService(_mockRepository.Object, _mockApiClient.Object);
        }

        [Fact]
        public async Task GetCompanyDetailAsync_WhenCompanyDoesntExistsInDb_ShouldCallExternalApi()
        {
            // Arrange
            var company = new Company { Symbol = "AAPL", Name = "Apple Inc." };
            _mockRepository.Setup(r => r.GetCompanyDetailAsync("AAPL")).ReturnsAsync((Company?)null);
            _mockApiClient.Setup(r => r.FetchCompanyDetailAsync("AAPL")).ReturnsAsync(company);

            // Act
            var result = await _companyService.GetCompanyDetailAsync("AAPL");

            // Assert
            result.Should().NotBeNull();
            result!.Symbol.Should().Be("AAPL");
            _mockApiClient.Verify(c => c.FetchCompanyDetailAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetCompanyDetailAsync_WhenCompanyExistsInDb_ShouldNotCallExternalApi()
        {
            // Arrange
            var company = new Company{ Symbol = "AAPL", Name = "Apple Inc." };
            _mockRepository.Setup(r => r.GetCompanyDetailAsync("AAPL")).ReturnsAsync(company);

            // Act
            var result = await _companyService.GetCompanyDetailAsync("AAPL");

            // Assert
            result.Should().NotBeNull();
            result!.Symbol.Should().Be("AAPL");
            _mockApiClient.Verify(c => c.FetchCompanyDetailAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetCompanyDetailAsync_WhenApiReturnsNull_ShouldNotSaveToDb()
        {
            // Arrange
            _mockRepository.Setup(r => r.GetCompanyDetailAsync("AAPL")).ReturnsAsync((Company?)null);
            _mockApiClient.Setup(c => c.FetchCompanyDetailAsync("AAPL")).ReturnsAsync((Company?)null);

            // Act
            await _companyService.GetCompanyDetailAsync("AAPL");

            // Assert
            _mockRepository.Verify(r => r.SaveCompanyDetailAsync(It.IsAny<Company>()), Times.Never);
        }
    }
}