using AutoMapper;
using HackathonDataAnalysis.Application.Rules.Commands;
using HackathonDataAnalysis.Application.Rules.Handlers;
using HackathonDataAnalysis.Domain.Enums;
using HackathonDataAnalysis.Domain.Interfaces;
using HackathonDataAnalysis.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace HackathonDataAnalysis.Application.Test.Rules.Handlers;

public class UpdateRuleHandlerTest
{
    private readonly Mock<IRuleRepository> _repository = new();
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IMapper> _mapper = new();
    private readonly Mock<ILogger<UpdateRuleHandler>> _logger = new();
    private readonly UpdateRuleHandler _handler;
    
    public UpdateRuleHandlerTest()
        => _handler = new UpdateRuleHandler(_repository.Object, _unitOfWork.Object, _mapper.Object, _logger.Object);
    
    [Fact]
    public async Task Handle_WhenRuleNotFound_ShouldReturnError()
    {
        // Arrange
        var ruleId = Guid.NewGuid();
        var request = new UpdateRuleRequest
        {
            Id = ruleId,
            DurationMinutes = 1,
            Message = "Test Message",
            Name = "Test Name",
            Operator = Operator.EqualTo,
            SensorType = SensorType.SoilMoisture,
            Status = "Test Status",
            Threshold = 10
        };
        _repository.Setup(x => x.FindAsync(ruleId, CancellationToken.None)).Returns(Task.FromResult<Rule?>(null));
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Rule not found", result.Exception.Message);
    }
    
    [Fact]
    public async Task Handle_WhenValid_ShouldReturnSuccess()
    {
        // Arrange
        var ruleId = Guid.NewGuid();
        var request = new UpdateRuleRequest
        {
            Id = ruleId,
            DurationMinutes = 1,
            Message = "Test Message",
            Name = "Test Name",
            Operator = Operator.EqualTo,
            SensorType = SensorType.SoilMoisture,
            Status = "Test Status",
            Threshold = 10
        };
        _repository.Setup(x => x.FindAsync(ruleId, CancellationToken.None)).Returns(Task.FromResult<Rule?>(new Rule()));
        
        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _repository.Verify(x => x.Update(It.IsAny<Rule>()), Times.Once);
    }
}