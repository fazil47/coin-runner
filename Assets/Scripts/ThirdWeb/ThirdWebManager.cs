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

        public static async Task<bool> GetCharacter() {
            TransactionResult result = await _instance._characterContract.ERC1155.Claim("0", 1);
            return result.isSuccessful();
        }

        // TODO: This needs to be changed to check if the user owns a particular character
        public static async Task<bool> IsCharacterOwner() {
            var owned = await _instance._characterContract.ERC1155.GetOwned(ConnectedAddress);
            return owned.Count > 0;
        }

        public static async Task<string> GetCoinBalance() {
            var balance = await _instance._coinContract.ERC20.Balance();
            return balance.displayValue;
        }

        #endregion

        #region Private Methods

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
#if !UNITY_EDITOR
                _instance = this;

                // Initialize the ThirdWeb SDK and wallet connection event listeners
                _sdk = new ThirdwebSDK("Mumbai");
                _coinContract = _sdk.GetContract(coinContractAddress);
                _characterContract = _sdk.GetContract(characterContractAddress, "edition-drop");

                // TODO: Got a warning, maybe await this?
                InitializeAuthPanel();

                // Set GameObject and AuthPanel to not be destroyed on scene load
                DontDestroyOnLoad(this.gameObject);
                DontDestroyOnLoad(authPanelCanvas);
#endif
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

                // if (await IsCharacterOwner()) {
                //     Debug.Log("Already owns character");
                // }
                // else {
                //     if (await GetCharacter()) {
                //         Debug.Log("Got character");
                //     }
                //     else {
                //         Debug.Log("Error getting character");
                //     }
                // }
            }
            catch (System.Exception e) {
                authStatusText.text = $"Error: {e.Message}";
            }
        }

        #endregion
    }
}