using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Platformer.Scene;
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
        private float _coinBalance;
        private static Dictionary<int, bool> _characterOwnership;

        #endregion

        #region Properties

        public static bool IsAuthenticated => _instance._isAuthenticated;

        public static string ConnectedAddress => _instance._connectedAddress;

        public static bool ShouldDataBeRefreshed { get; set; }

        #endregion

        #region Public Methods

        public static void ShowAuthPanel() {
            _instance.authPanelCanvas.SetActive(true);
        }

        public static void HideAuthPanel() {
            _instance.authPanelCanvas.SetActive(false);
        }

        public static async Task<bool> ClaimCoin(string amount) {
            TransactionResult result = await _instance._coinContract.ERC20.Claim(amount);
            if (result.isSuccessful()) {
                _instance._coinBalance += float.Parse(amount);
                return true;
            }

            return false;
        }

        public static async Task<bool> ClaimAirdropCharacter() {
            TransactionResult result = await _instance._characterContract.ERC1155.Claim("0", 1);
            if (result.isSuccessful()) {
                _characterOwnership[CharacterManager.AirdropCharacter.Item3] = true;
                return true;
            }

            return false;
        }

        public static async Task<bool> BuyCharacter(int characterId) {
            TransactionResult result = await _instance._characterContract.ERC1155.Claim(characterId.ToString(), 1);
            if (result.isSuccessful()) {
                _characterOwnership[characterId] = true;
                return true;
            }

            return false;
        }

        public static bool IsAirdropCharacterOwner() {
#if !UNITY_EDITOR
            int airdropCharacterId = CharacterManager.AirdropCharacter.Item3;
            if (_characterOwnership.ContainsKey(airdropCharacterId)) {
                return _characterOwnership[airdropCharacterId];
            }

            return false;
#endif
            return true;
        }

        public static bool IsCharacterOwner(int characterId) {
#if !UNITY_EDITOR
            if (_characterOwnership.ContainsKey(characterId)) {
                return _characterOwnership[characterId];
            }

            return false;
#endif
            return true;
        }

        public static float GetCoinBalance() {
            return _instance._coinBalance;
        }

        public static async Task RefreshData() {
            var balance = await _instance._coinContract.ERC20.Balance();
            _instance._coinBalance = float.Parse(balance.displayValue);

            List<NFT> owned = await _instance._characterContract.ERC1155.GetOwned(ConnectedAddress);
            foreach (NFT character in owned) {
                _characterOwnership[int.Parse(character.metadata.id)] = character.quantityOwned > 0;
            }
        }

        #endregion

        #region Private Methods

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
                _instance = this;
                _coinBalance = 0;
                _characterOwnership = new Dictionary<int, bool>();
                ShouldDataBeRefreshed = true;
                _isAuthenticated = false;

#if !UNITY_EDITOR
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