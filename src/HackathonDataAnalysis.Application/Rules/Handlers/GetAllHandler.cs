using AutoMapper;
using HackathonDataAnalysis.Application.Readings.Queries;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Interfaces;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Rules.Handlers;

public class GetAllHandler(IReadingRepository repository, IMapper mapper) : IRequestHandler<GetAllRequest, Result<IEnumerable<ReadingDto>>>
{
    public async Task<Result<IEnumerable<ReadingDto>>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        => Result.Success(mapper.Map<IEnumerable<ReadingDto>>(await repository.FindAsync(cancellationToken)));
}