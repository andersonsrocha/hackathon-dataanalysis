using AutoMapper;
using HackathonDataAnalysis.Application.Readings.Commands;
using HackathonDataAnalysis.Domain.Dto;
using HackathonDataAnalysis.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using OperationResult;

namespace HackathonDataAnalysis.Application.Readings.Handlers;

public class UpdateReadingHandler(IReadingRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateReadingHandler> logger) : IRequestHandler<UpdateReadingRequest, Result<ReadingDto>>
{
    public async Task<Result<ReadingDto>> Handle(UpdateReadingRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating a reading with id: {Id}", request.Id);
        
        logger.LogInformation("Checking if reading with id {Id} exists", request.Id);
        var reading = await repository.FindAsync(request.Id, cancellationToken);
        if (reading is null)
            return Result.Error<ReadingDto>(new Exception("Reading not found"));
        
        reading.Update(request.Date, request.SoilMoisture, request.Temperature, request.Precipitation);
        repository.Update(reading);
        await unitOfWork.CommitAsync(cancellationToken);
        
        logger.LogInformation("Reading updated successfully.");
        return Result.Success(mapper.Map<ReadingDto>(reading));
    }
}