using HackathonDataAnalysis.Application.Readings.Commands;
using HackathonDataAnalysis.Application.Readings.Queries;
using HackathonDataAnalysis.Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HackathonDataAnalysis.Api.Controllers;

[Route("api/[controller]")]
public class ReadingsController(ILogger<ReadingsController> logger, IMediator mediator) : BaseController(logger)
{
    [HttpGet]
    [Authorize("Admin")]
    [Route("{id:Guid}")]
    [ProducesResponseType(typeof(ReadingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id)
        => await Send(mediator.Send(new GetByIdRequest(id)));
    
    [HttpGet]
    [Authorize("Admin")]
    [ProducesResponseType(typeof(ReadingDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get()
        => await Send(mediator.Send(new GetAllRequest()));
    
    [HttpPost]
    [Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] CreateReadingRequest request)
        => await Send(mediator.Send(request));
    
    [HttpPut]
    [Authorize("Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put([FromBody] UpdateReadingRequest request)
        => await Send(mediator.Send(request));
}