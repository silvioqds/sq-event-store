using EventStore.Client;
using Microsoft.AspNetCore.Mvc;
using SQEventStoreDB.API.Helpers;
using SQEventStoreDB.Domain.Account;
using SQEventStoreDB.Domain.Events.BankAccount;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace SQEventStoreDB.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{Version}/[controller]")]
    public class BankAccountController : ControllerBase
    {

        private readonly ILogger<BankAccountController> _logger;
        private readonly EventStoreClient _eventStoreClient;
        public BankAccountController(ILogger<BankAccountController> logger, EventStoreClient eventStoreClient)
        {
            _logger = logger;
            _eventStoreClient = eventStoreClient;
        }

        [HttpPost]
        public async Task<object> PostAsync([FromBody]BankAccount bankAccount)
        {
            try
            {
                var bankAccountOpened = new BankAccountOpenedEvent(bankAccount.AccountId, bankAccount.OwnerId);

                byte[] utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(bankAccountOpened);
                var eventData = new EventData(Uuid.NewUuid(), nameof(BankAccountOpenedEvent), utf8Bytes);
                var writeResult = await this._eventStoreClient.AppendToStreamAsync(StreamNameHelper.GetStreamName<BankAccount>(), StreamState.Any, new[] { eventData });

                return writeResult.NextExpectedStreamRevision.ToUInt64();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [ProducesResponseType(typeof(List<BankAccountOpenedEvent>), 200)]
        [ProducesErrorResponseType(typeof(ErrorResponse))]
        [HttpGet("events")]
        public async Task<ActionResult<List<BankAccountOpenedEvent>>> GetEvents()
        {
            try
            {
                var readResult = _eventStoreClient.ReadStreamAsync(Direction.Forwards, StreamNameHelper.GetStreamName<BankAccount>(), StreamPosition.Start);

                if (await readResult.ReadState.ConfigureAwait(false) == ReadState.StreamNotFound)
                    return Ok(new List<BankAccountOpenedEvent>());

                var events = await readResult
                    .Select(@event => JsonSerializer.Deserialize<BankAccountOpenedEvent>(@event.Event.Data.ToArray()))
                    .ToListAsync()
                    .ConfigureAwait(false);

                return Ok(events);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {               
                    Message = "Erro ao recuperar eventos do Event Store.",
                    Details =  ex.Message 
                });
            }
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
