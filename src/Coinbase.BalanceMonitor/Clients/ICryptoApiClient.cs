using System.Threading.Tasks;

namespace Coinbase.BalanceMonitor.Clients
{
    public interface ICryptoApiClient
    {
        Task<int> GetAccountBalance();
    }
}