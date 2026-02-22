using HackathonDataAnalysis.Domain.Dto;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Rules.Queries;

public sealed class GetByIdRequest(Guid id) : IRequest<Result<ReadingDto>>
{
    public Guid Id { get; init; } = id;
}