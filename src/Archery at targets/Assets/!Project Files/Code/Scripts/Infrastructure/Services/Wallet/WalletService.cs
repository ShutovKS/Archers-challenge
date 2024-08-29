using Infrastructure.Services.Progress;

namespace Infrastructure.Services.Wallet
{
    public class WalletService : IWalletService
    {
        private readonly IProgressService _progressService;

        public WalletService(IProgressService progressService)
        {
            _progressService = progressService;
        }

        public int GetCoins() => _progressService.Get().coins;

        public void AddCoins(int amount)
        {
            var progressData = _progressService.Get();
            progressData.coins += amount;
            _progressService.Set(progressData);
        }

        public bool RemoveCoins(int amount)
        {
            if (!CanAfford(amount))
            {
                return false;
            }

            var progressData = _progressService.Get();
            progressData.coins -= amount;
            _progressService.Set(progressData);

            return true;
        }

        public bool CanAfford(int amount) => GetCoins() >= amount;
    }
}