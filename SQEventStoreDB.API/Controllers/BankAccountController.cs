using MediatR;
using Microsoft.AspNetCore.Mvc;
using SQEventStoreDB.Application.DTO;
using SQEventStoreDB.Application.UseCases.BankAccount.Commands;

namespace SQEventStoreDB.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{Version}/[controller]")]
    public class BankAccountController : ControllerBase
    {

        private readonly ILogger<BankAccountController> _logger;
        private readonly IMediator _mediator;
        public BankAccountController(ILogger<BankAccountController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("create-account")]
        [ProducesResponseType(typeof(BankAccountDTO), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        public async Task<IActionResult> PostAsync([FromBody] BankAccountDTO bankAccount)
        {
            try
            {
                var result = await _mediator.Send(new BankAccountOpenedCommand(Guid.NewGuid(), Guid.NewGuid()));
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
       
        public class ErrorResponse
        {
            public string Message { get; set; }
            public string? Details { get; set; }

            public ErrorResponse() { }

            public ErrorResponse(string message, string? details = null)
            {
                Message = message;
                Details = details;
            }
        }
    }
}
