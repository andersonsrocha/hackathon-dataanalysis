using HackathonDataAnalysis.Domain.Dto;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Readings.Queries;

public sealed class GetByPlotAndSinceRequest(Guid plotId, DateTime since) : IRequest<Result<IEnumerable<ReadingDto>>>
{
    public Guid PlotId { get; init; } = plotId;
    public DateTime Since { get; init; } = since;
}