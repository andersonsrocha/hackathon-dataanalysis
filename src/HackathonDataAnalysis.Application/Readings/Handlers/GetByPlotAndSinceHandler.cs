using AutoMapper;
using HackathonDataAnalysis.Application.Readings.Queries;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Interfaces;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Readings.Handlers;

public class GetByPlotAndSinceHandler(IReadingRepository repository, IMapper mapper) : IRequestHandler<GetByPlotAndSinceRequest, Result<IEnumerable<ReadingDto>>>
{
    public async Task<Result<IEnumerable<ReadingDto>>> Handle(GetByPlotAndSinceRequest request, CancellationToken cancellationToken)
        => Result.Success(mapper.Map<IEnumerable<ReadingDto>>(await repository.FindByPlotAndSinceAsync(request.PlotId, request.Since, cancellationToken)));
}