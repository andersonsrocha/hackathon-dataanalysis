using AutoMapper;
using HackathonDataAnalysis.Application.Readings.Queries;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Interfaces;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Rules.Handlers;

public class GetByIdHandler(IReadingRepository repository, IMapper mapper) : IRequestHandler<GetByIdRequest, Result<ReadingDto>>
{
    public async Task<Result<ReadingDto>> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        => Result.Success(mapper.Map<ReadingDto>(await repository.FindAsync(request.Id, cancellationToken)));
}