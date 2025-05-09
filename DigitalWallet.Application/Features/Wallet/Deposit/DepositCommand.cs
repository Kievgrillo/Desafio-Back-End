using DigitalWallet.Core.Interfaces;
using MediatR;

namespace DigitalWallet.Application.Features.Wallet.Deposit;

public record DepositCommand : IRequest<decimal>
{
    public string UserId { get; init; }
    public decimal Amount { get; init; }
}

public class DepositHandler : IRequestHandler<DepositCommand, decimal>
{
    private readonly IWalletRepository _repository;

    public DepositHandler(IWalletRepository repository)
    {
        _repository = repository;
    }

    public async Task<decimal> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _repository.GetByUserIdAsync(request.UserId);
        wallet.Balance += request.Amount;
        await _repository.UpdateAsync(wallet);
        return wallet.Balance;
    }
}