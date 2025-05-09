using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Carteira_Digital.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WalletController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WalletController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// Obtém o saldo atual da carteira do usuário autenticado
        [HttpGet("balance")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<decimal>> GetBalance()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var query = new BalanceQuery { UserId = userId };
            var balance = await _mediator.Send(query);

            return Ok(balance);
        }

        /// Adiciona saldo à carteira do usuário
        [HttpPost("deposit")]
        public async Task<ActionResult<decimal>> Deposit(DepositDto depositDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var command = new DepositCommand
            {
                UserId = userId,
                Amount = depositDto.Amount
            };

            var newBalance = await _mediator.Send(command);
            return Ok(new { Balance = newBalance });
        }
    }
