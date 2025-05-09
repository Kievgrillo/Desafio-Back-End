using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Carteira_Digital.Controllers
{
    public class TransactionController
    {
[       [Authorize]
        [ApiController]
        [Route("api/[controller]")]
        public class TransactionController : ControllerBase
        {
            private readonly IMediator _mediator;

            public TransactionController(IMediator mediator)
            {
                _mediator = mediator;
            }

            /// Realiza uma transferência entre carteiras
            [HttpPost("transfer")]
            public async Task<ActionResult<TransactionDto>> Transfer(TransferDto transferDto)
            {
                var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var command = new TransferCommand
                {
                    SenderUserId = senderId,
                    ReceiverUsername = transferDto.ReceiverUsername,
                    Amount = transferDto.Amount,
                    Description = transferDto.Description
                };

                var transaction = await _mediator.Send(command);
                return Ok(transaction);
            }

            /// Obtém o histórico de transações com filtro por período
            [HttpGet("history")]
            public async Task<ActionResult<List<TransactionDto>>> GetHistory(
                [FromQuery] DateTime? startDate,
                [FromQuery] DateTime? endDate)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var query = new TransactionHistoryQuery
                {
                    UserId = userId,
                    StartDate = startDate,
                    EndDate = endDate
                };

                var transactions = await _mediator.Send(query);
                return Ok(transactions);
            }
        }
    }
}
