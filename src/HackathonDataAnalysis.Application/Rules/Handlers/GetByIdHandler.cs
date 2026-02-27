using AutoMapper;
using HackathonDataAnalysis.Application.Rules.Queries;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Interfaces;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Rules.Handlers;

public class GetByIdHandler(IRuleRepository repository, IMapper mapper) : IRequestHandler<GetByIdRequest, Result<RuleDto>>
{
    public async Task<Result<RuleDto>> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        => Result.Success(mapper.Map<RuleDto>(await repository.FindAsync(request.Id, cancellationToken)));
}