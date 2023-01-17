using System.Threading.Tasks;

namespace Thirdweb
{
    /// <summary>
    /// Connect and Interact with a Wallet.
    /// </summary>
    public class Wallet : Routable
    {
        public Wallet() : base($"sdk{subSeparator}wallet")
        {
        }

        /// <summary>
        /// Connect a user's wallet via a given wallet provider
        /// </summary>
        /// <param name="walletConnection">The wallet provider and chainId to connect to. Defaults to the injected browser extension.</param>
        public Task<string> Connect(WalletConnection? walletConnection = null)
        {
            var connection = walletConnection ?? new WalletConnection()
            {
                provider = WalletProvider.Injected,
            }; ;
            return Bridge.Connect(connection);
        }

        /// <summary>
        /// Disconnect the user's wallet
        /// </summary>
        public Task Disconnect()
        {
            return Bridge.Disconnect();
        }

        /// <summary>
        /// Authenticate the user by signing a payload that can be used to securely identify users. See https://portal.thirdweb.com/auth
        /// </summary>
        /// <param name="domain">The domain to authenticate to</param>
        public async Task<LoginPayload> Authenticate(string domain)
        {
            return await Bridge.InvokeRoute<LoginPayload>($"sdk{subSeparator}auth{separator}login", Utils.ToJsonStringArray(domain));
        }

        /// <summary>
        /// Get the balance of the connected wallet
        /// </summary>
        /// <param name="currencyAddress">Optional address of the currency to check balance of</param>
        public async Task<CurrencyValue> GetBalance(string currencyAddress = Utils.NativeTokenAddress)
        {
            return await Bridge.InvokeRoute<CurrencyValue>(getRoute("balance"), Utils.ToJsonStringArray(currencyAddress));
        }

        /// <summary>
        /// Get the connected wallet address
        /// </summary>
        public async Task<string> GetAddress()
        {
            return await Bridge.InvokeRoute<string>(getRoute("getAddress"), new string[] { });
        }

        /// <summary>
        /// Check if a wallet is connected
        /// </summary>
        public async Task<bool> IsConnected()
        {
            return await Bridge.InvokeRoute<bool>(getRoute("isConnected"), new string[] { });
        }

        /// <summary>
        /// Get the connected chainId
        /// </summary>
        public async Task<int> GetChainId()
        {
            return await Bridge.InvokeRoute<int>(getRoute("getChainId"), new string[] { });
        }

        /// <summary>
        /// Prompt the connected wallet to switch to the giiven chainId
        /// </summary>
        public async Task SwitchNetwork(int chainId)
        {
            await Bridge.SwitchNetwork(chainId);
        }

        /// <summary>
        /// Transfer currency to a given address
        /// </summary>
        public async Task<TransactionResult> Transfer(string to, string amount, string currencyAddress = Utils.NativeTokenAddress)
        {
            return await Bridge.InvokeRoute<TransactionResult>(getRoute("transfer"), Utils.ToJsonStringArray(to, amount, currencyAddress));
        }

        /// <summary>
        /// Prompt the connected wallet to sign the given message
        /// </summary>
        public async Task<string> Sign(string message)
        {
            return await Bridge.InvokeRoute<string>(getRoute("sign"), Utils.ToJsonStringArray(message));
        }

        /// <summary>
        /// Recover the original wallet address that signed a message
        /// </summary>
        public async Task<string> RecoverAddress(string message, string signature)
        {
            return await Bridge.InvokeRoute<string>(getRoute("recoverAddress"), Utils.ToJsonStringArray(message, signature));
        }

        /// <summary>
        /// Send a raw transaction from the connected wallet
        /// </summary>
        public async Task<TransactionResult> SendRawTransaction(TransactionRequest transactionRequest)
        {
            return await Bridge.InvokeRoute<TransactionResult>(getRoute("sendRawTransaction"), Utils.ToJsonStringArray(transactionRequest));
        }

        /// <summary>
        /// Prompt the user to fund their wallet using one of the thirdweb pay providers (defaults to Coinbase Pay).
        /// </summary>
        /// <param name="options">The options like wallet address to fund, on which chain, etc</param>
        public async Task FundWallet(FundWalletOptions options)
        {
            if (options.address == null)
            {
                options.address = await GetAddress();
            }
            await Bridge.FundWallet(options);
        }
    }

    public struct WalletConnection
    {
        public WalletProvider provider;
        public int chainId;
    }

    public class WalletProvider
    {
        private WalletProvider(string value) { Value = value; }

        public static string Value { get; private set; }

        public static WalletProvider MetaMask { get { return new WalletProvider("metamask"); } }
        public static WalletProvider CoinbaseWallet { get { return new WalletProvider("coinbaseWallet"); } }
        public static WalletProvider WalletConnect { get { return new WalletProvider("walletConnect"); } }
        public static WalletProvider Injected { get { return new WalletProvider("injected"); } }
        public static WalletProvider MagicAuth { get { return new WalletProvider("magicAuth"); } }

        public override string ToString()
        {
            return Value;
        }
    }
}