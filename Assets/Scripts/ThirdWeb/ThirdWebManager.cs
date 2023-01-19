using System;
using System.Threading.Tasks;
using UnityEngine;
using Thirdweb;
using TMPro;
using UnityEngine.UI;

namespace Platformer.ThirdWeb {
    /// <summary>
    /// Singleton class for managing connection to a blockchain using the ThirdWeb SDK
    /// </summary>
    public class ThirdWebManager : MonoBehaviour {
        #region Static Fields

        private static ThirdWebManager _instance;

        #endregion

        #region Serialized Fields

        [SerializeField] private String coinContractAddress;
        [SerializeField] private String characterContractAddress;
        [SerializeField] private GameObject authPanelCanvas;
        [SerializeField] private TextMeshProUGUI authStatusText;
        [SerializeField] private Button authButton;

        #endregion

        #region Private Fields

        private ThirdwebSDK _sdk;
        private bool _isAuthenticated;
        private string _connectedAddress;
        private Contract _coinContract;
        private Contract _characterContract;

        #endregion

        #region Properties

        public static bool IsAuthenticated => _instance._isAuthenticated;

        public static string ConnectedAddress => _instance._connectedAddress;

        #endregion

        #region Public Methods

        public static void ShowAuthPanel() {
            _instance.authPanelCanvas.SetActive(true);
        }

        public static void HideAuthPanel() {
            _instance.authPanelCanvas.SetActive(false);
        }

        public static async Task<bool> GetCoin(string amount) {
            TransactionResult result = await _instance._coinContract.ERC20.Claim(amount);
            return result.isSuccessful();
        }

        #endregion

        #region Private Methods

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
                _instance = this;

                // Initialize the ThirdWeb SDK and wallet connection event listeners
                _sdk = new ThirdwebSDK("Mumbai");
                _coinContract = _sdk.GetContract(coinContractAddress);
                _characterContract = _sdk.GetContract(characterContractAddress);

                InitializeAuthPanel();

                // Set GameObject and AuthPanel to not be destroyed on scene load
                DontDestroyOnLoad(this.gameObject);
                DontDestroyOnLoad(authPanelCanvas);
            }

            _isAuthenticated = false;
        }

        private void InitializeAuthPanel() {
            authStatusText.text = "Connect Wallet";
            authButton.onClick.AddListener(() => { ConnectWallet(WalletProvider.MetaMask); });
        }

        private async void ConnectWallet(WalletProvider provider) {
            authStatusText.text = "Authenticating...";
            authButton.interactable = false;

            try {
                string address = await _sdk.wallet.Connect(new WalletConnection() {
                    provider = provider,
                    chainId = 80001
                });

                authStatusText.text = $"Connected as {address}";
                _isAuthenticated = true;
                _connectedAddress = address;
            }
            catch (System.Exception e) {
                authStatusText.text = $"Error: {e.Message}";
            }
        }

        #endregion
    }
}