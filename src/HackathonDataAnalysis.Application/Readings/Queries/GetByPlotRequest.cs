using HackathonDataAnalysis.Domain.Dto;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Readings.Queries;

public sealed class GetByPlotRequest(Guid plotId) : IRequest<Result<IEnumerable<ReadingDto>>>
{
    public Guid PlotId { get; init; } = plotId;
}