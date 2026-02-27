using AutoMapper;
using HackathonDataAnalysis.Application.Rules.Commands;
using HackathonDataAnalysis.Application.Rules.Handlers;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Enums;
using HackathonDataAnalysis.Domain.Interfaces;
using HackathonDataAnalysis.Domain.Models;
using HackathonDataAnalysis.Plots.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace HackathonDataAnalysis.Application.Test.Rules.Handlers;

public class CreateRuleHandlerTest
{
    private readonly Mock<IRuleRepository> _repository = new();
    private readonly Mock<IPlotService> _plotService = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<ILogger<CreateRuleHandler>> _logger = new();
    private readonly CreateRuleHandler _handler;
    
    public CreateRuleHandlerTest()
        => _handler = new CreateRuleHandler(_repository.Object, _plotService.Object, _unitOfWork.Object, _mapper.Object, _logger.Object);
    
    [Fact]
    public async Task Handle_WhenPlotNotFound_ShouldReturnError()
    {
        // Arrange
        var plotId = Guid.NewGuid();
        var request = new CreateRuleRequest
        {
            PlotId = plotId,
            DurationMinutes = 1,
            Message = "Test Message",
            Name = "Test Name",
            Operator = Operator.EqualTo,
            SensorType = SensorType.SoilMoisture,
            Status = "Test Status",
            Threshold = 10
        };
        _plotService.Setup(x => x.FindAsync(plotId, CancellationToken.None)).Returns(Task.FromResult<PlotDto?>(null));
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Plot not found", result.Exception.Message);
    }
    
    [Fact]
    public async Task Handle_WhenValid_ShouldReturnSuccess()
    {
        // Arrange
        var plotId = Guid.NewGuid();
        var request = new CreateRuleRequest
        {
            PlotId = plotId,
            DurationMinutes = 1,
            Message = "Test Message",
            Name = "Test Name",
            Operator = Operator.EqualTo,
            SensorType = SensorType.SoilMoisture,
            Status = "Test Status",
            Threshold = 10
        };
        _plotService.Setup(x => x.FindAsync(plotId, CancellationToken.None)).Returns(Task.FromResult<PlotDto?>(new PlotDto()));
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _repository.Verify(x => x.Add(It.IsAny<Rule>()), Times.Once);
    }
}