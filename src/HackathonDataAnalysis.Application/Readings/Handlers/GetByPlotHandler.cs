using AutoMapper;
using HackathonDataAnalysis.Application.Readings.Queries;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Interfaces;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Readings.Handlers;

public class GetByPlotHandler(IReadingRepository repository, IMapper mapper) : IRequestHandler<GetByPlotRequest, Result<IEnumerable<ReadingDto>>>
{
    public async Task<Result<IEnumerable<ReadingDto>>> Handle(GetByPlotRequest request, CancellationToken cancellationToken)
        => Result.Success(mapper.Map<IEnumerable<ReadingDto>>(await repository.FindByPlotAsync(request.PlotId, cancellationToken)));
}