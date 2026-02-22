using HackathonDataAnalysis.Domain.Dto;
using MediatR;
using OperationResult;

namespace HackathonDataAnalysis.Application.Readings.Queries;

public sealed class GetAllRequest : IRequest<Result<IEnumerable<ReadingDto>>>;