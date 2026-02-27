using AutoMapper;
using HackathonDataAnalysis.Application.Readings.Commands;
using HackathonDataAnalysis.Application.Readings.Handlers;
using HackathonDataAnalysis.Domain.Interfaces;
using HackathonDataAnalysis.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace HackathonDataAnalysis.Application.Test.Readings.Handlers;

public class UpdateReadingHandlerTest
{
    private readonly Mock<IReadingRepository> _repository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<ILogger<UpdateReadingHandler>> _logger = new();
    private readonly UpdateReadingHandler _handler;
    
    public UpdateReadingHandlerTest()
        => _handler = new UpdateReadingHandler(_repository.Object, _unitOfWork.Object, _mapper.Object, _logger.Object);
    
    [Fact]
    public async Task Handle_WhenReadingNotFound_ShouldReturnError()
    {
        // Arrange
        var readingId = Guid.NewGuid();
        var request = new UpdateReadingRequest
        {
            Id = readingId,
            Date = DateTime.Now,
            SoilMoisture = 10,
            Temperature = 20,
            Precipitation = 5
        };
        _repository.Setup(x => x.FindAsync(readingId, CancellationToken.None)).Returns(Task.FromResult<Reading?>(null));
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Reading not found", result.Exception.Message);
    }
    
    [Fact]
    public async Task Handle_WhenValid_ShouldReturnSuccess()
    {
        // Arrange
        var readingId = Guid.NewGuid();
        var request = new UpdateReadingRequest
        {
            Id = readingId,
            Date = DateTime.Now,
            SoilMoisture = 10,
            Temperature = 20,
            Precipitation = 5
        };
        _repository.Setup(x => x.FindAsync(readingId, CancellationToken.None)).Returns(Task.FromResult<Reading?>(new Reading()));
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _repository.Verify(x => x.Update(It.IsAny<Reading>()), Times.Once);
    }
}