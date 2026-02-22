using AutoMapper;
using HackathonDataAnalysis.Application.Rules.Commands;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Interfaces;
using HackathonDataAnalysis.Domain.Models;
using HackathonDataAnalysis.Plots.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using OperationResult;

namespace HackathonDataAnalysis.Application.Rules.Handlers;

public class CreateRuleHandler(IRuleRepository repository, IPlotService plotService, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateRuleHandler> logger) : IRequestHandler<CreateRuleRequest, Result<RuleDto>>
{
    public async Task<Result<RuleDto>> Handle(CreateRuleRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating a new rule with plot: {PlotId}", request.PlotId);
        
        logger.LogInformation("Checking if plot with id {PlotId} exists", request.PlotId);
        var plot = await plotService.FindAsync(request.PlotId, cancellationToken);
        if (plot is null)
            return Result.Error<RuleDto>(new Exception("Plot not found"));

        var reading = (Rule)request;
        repository.Add(reading);
        await unitOfWork.CommitAsync(cancellationToken);
        
        logger.LogInformation("Rule created successfully.");
        return Result.Success(mapper.Map<RuleDto>(reading));
    }
}