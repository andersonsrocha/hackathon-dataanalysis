using AutoMapper;
using HackathonDataAnalysis.Application.Rules.Queries;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Interfaces;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Rules.Handlers;

public class GetAllHandler(IRuleRepository repository, IMapper mapper) : IRequestHandler<GetAllRequest, Result<IEnumerable<RuleDto>>>
{
    public async Task<Result<IEnumerable<RuleDto>>> Handle(GetAllRequest request, CancellationToken cancellationToken)
        => Result.Success(mapper.Map<IEnumerable<RuleDto>>(await repository.FindAsync(cancellationToken)));
}