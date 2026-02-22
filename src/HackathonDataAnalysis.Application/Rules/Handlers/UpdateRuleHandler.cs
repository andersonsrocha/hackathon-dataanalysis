using AutoMapper;
using HackathonDataAnalysis.Application.Rules.Commands;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using OperationResult;

namespace HackathonDataAnalysis.Application.Rules.Handlers;

public class UpdateRuleHandler(IRuleRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateRuleHandler> logger) : IRequestHandler<UpdateRuleRequest, Result<RuleDto>>
{
    public async Task<Result<RuleDto>> Handle(UpdateRuleRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating a rule with id: {Id}", request.Id);
        
        logger.LogInformation("Checking if rule with id {Id} exists", request.Id);
        var rule = await repository.FindAsync(request.Id, cancellationToken);
        if (rule is null)
            return Result.Error<RuleDto>(new Exception("Rule not found"));
        
        rule.Update(request.Name, request.SensorType, request.Operator, request.Threshold, request.DurationMinutes, request.Status, request.Message);
        repository.Update(rule);
        await unitOfWork.CommitAsync(cancellationToken);
        
        logger.LogInformation("Rule updated successfully.");
        return Result.Success(mapper.Map<RuleDto>(rule));
    }
}