using AutoMapper;
using HackathonDataAnalysis.Application.Readings.Commands;
using HackathonDataAnalysis.Application.Readings.Handlers;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Enums;
using HackathonDataAnalysis.Domain.Interfaces;
using HackathonDataAnalysis.Domain.Models;
using HackathonDataAnalysis.NewRelicEvent.Interfaces;
using HackathonDataAnalysis.Plots.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace HackathonDataAnalysis.Application.Test.Readings.Handlers;

public class CreateReadingHandlerTest
{
    private readonly Mock<IReadingRepository> _repository = new();
    private readonly Mock<IRuleRepository> _ruleRepository = new();
    private readonly Mock<IPlotService> _plotService = new();
    private readonly Mock<IRabbitMqPublisher> _bus = new();
    private readonly Mock<INewRelicEventPublisher> _publisher = new();
    private readonly Mock<IConfiguration> _configuration = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<ILogger<CreateReadingHandler>> _logger = new();
    private readonly CreateReadingHandler _handler;
    
    public CreateReadingHandlerTest()
        => _handler = new CreateReadingHandler(_repository.Object, _ruleRepository.Object, _plotService.Object, _bus.Object, _publisher.Object, _configuration.Object, _unitOfWork.Object, _mapper.Object, _logger.Object);
    
    [Fact]
    public async Task Handle_WhenPlotNotFound_ShouldReturnError()
    {
        // Arrange
        var plotId = Guid.NewGuid();
        var request = new CreateReadingRequest
        {
            Date = DateTime.Now,
            PlotId = plotId,
            SoilMoisture = 10,
            Temperature = 20,
            Precipitation = 5
        };
        _plotService.Setup(x => x.FindAsync(plotId, CancellationToken.None)).Returns(Task.FromResult<PlotDto?>(null));
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Plot not found", result.Exception.Message);
    }
    
    [Fact]
    public async Task Handle_WhenThereRules_ShouldReturnSuccess()
    {
        // Arrange
        var plotId = Guid.NewGuid();
        var request = new CreateReadingRequest
        {
            Date = DateTime.Now,
            PlotId = plotId,
            SoilMoisture = 10,
            Temperature = 20,
            Precipitation = 5
        };
        var reading = new Reading
        {
            PlotId = plotId,
            Date = DateTime.Now,
            PlotName = "Test Plot",
            SoilMoisture = 10,
            Temperature = 20,
            Precipitation = 5
        };
        var rule = new Rule(plotId, "Test Rule", SensorType.SoilMoisture, Operator.EqualTo, 10, 1, "Test Status", "Test Message");
        _plotService.Setup(x => x.FindAsync(plotId, CancellationToken.None)).Returns(Task.FromResult<PlotDto?>(new PlotDto()));
        _ruleRepository.Setup(x => x.FindByPlotAsync(plotId, CancellationToken.None)).Returns(Task.FromResult<IEnumerable<Rule>>(new List<Rule> { rule }));
        _repository.Setup(x => x.FindByPlotAndSinceAsync(plotId, It.IsAny<DateTime>(), CancellationToken.None)).Returns(Task.FromResult<IEnumerable<Reading>>(new List<Reading> { reading }));
        _bus.Setup(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _bus.Verify(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task Handle_WhenValid_ShouldReturnSuccess()
    {
        // Arrange
        var plotId = Guid.NewGuid();
        var request = new CreateReadingRequest
        {
            Date = DateTime.Now,
            PlotId = plotId,
            SoilMoisture = 10,
            Temperature = 20,
            Precipitation = 5
        };
        _plotService.Setup(x => x.FindAsync(plotId, CancellationToken.None)).Returns(Task.FromResult<PlotDto?>(new PlotDto()));
        _ruleRepository.Setup(x => x.FindByPlotAsync(plotId, CancellationToken.None)).Returns(Task.FromResult<IEnumerable<Rule>>(new List<Rule>()));
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _repository.Verify(x => x.Add(It.IsAny<Reading>()), Times.Once);
    }
}