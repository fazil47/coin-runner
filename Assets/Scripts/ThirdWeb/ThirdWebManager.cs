using System;
using System.Threading.Tasks;
using Platformer.Scene;
using UnityEngine;
using Thirdweb;
using TMPro;
using UnityEngine.Serialization;
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
        private float _coinBalance;

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

        public static async Task<bool> GetAirdropCharacter() {
            TransactionResult result = await _instance._characterContract.ERC1155.Claim("0", 1);
            return result.isSuccessful();
        }

        public static async Task<bool> BuyCharacter(int characterId) {
            TransactionResult result = await _instance._characterContract.ERC1155.Claim(characterId.ToString(), 1);
            return result.isSuccessful();
        }

        public static async Task<bool> IsAirdropCharacterOwner() {
#if !UNITY_EDITOR
            var owned = await _instance._characterContract.ERC1155.GetOwned(ConnectedAddress);

            // TODO: use index from character manager
            int airdropCharacterId = CharacterManager.AirdropCharacter.Item4;
            if (owned.Count > 0) {
                foreach (var item in owned) {
                    if (item.metadata.id == airdropCharacterId.ToString()) {
                        return item.quantityOwned > 0;
                    }
                }
            }

#else
            return true;
#endif

            return false;
        }

        public static async Task<bool> IsCharacterOwner(int characterId) {
#if !UNITY_EDITOR
            var owned = await _instance._characterContract.ERC1155.GetOwned(ConnectedAddress);

            if (owned.Count > 0) {
                foreach (var item in owned) {
                    if (item.metadata.id == characterId.ToString()) {
                        return item.quantityOwned > 0;
                    }
                }
            }
#else
            return true;
#endif

            return false;
        }

        public static float GetCoinBalance() {
            return _instance._coinBalance;
        }

        public static async Task RefreshData() {
            var balance = await _instance._coinContract.ERC20.Balance();
            _instance._coinBalance = float.Parse(balance.displayValue);
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
            }
            catch (System.Exception e) {
                authStatusText.text = $"Error: {e.Message}";
            }
        }

        #endregion
    }
}