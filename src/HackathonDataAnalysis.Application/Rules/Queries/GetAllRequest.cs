using HackathonDataAnalysis.Domain.Dto;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Rules.Queries;

public sealed class GetAllRequest : IRequest<Result<IEnumerable<RuleDto>>>;