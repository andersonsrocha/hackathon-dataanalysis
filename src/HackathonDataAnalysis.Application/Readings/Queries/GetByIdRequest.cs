using HackathonDataAnalysis.Domain.Dto;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Readings.Queries;

public sealed class GetByIdRequest(Guid id) : IRequest<Result<ReadingDto>>
{
    public Guid Id { get; init; } = id;
}