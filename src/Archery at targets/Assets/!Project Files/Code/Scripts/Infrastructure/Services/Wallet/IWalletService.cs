namespace Infrastructure.Services.Wallet
{
    public interface IWalletService
    {
        int GetCoins();
        void AddCoins(int amount);
        bool RemoveCoins(int amount);
        bool CanAfford(int amount);
    }
}