using System;
using UnityEngine;
using Thirdweb;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Platformer.ThirdWeb {
    /// <summary>
    /// Singleton class for managing connection to blockchain using the ThirdWeb SDK
    /// </summary>
    public class ThirdWebManager : MonoBehaviour {
        private static ThirdWebManager _instance;

        [SerializeField] private GameObject authPanelCanvas;
        [SerializeField] private TextMeshProUGUI authStatusText;
        [SerializeField] private Button authButton;

        private ThirdwebSDK _sdk;
        private bool _isAuthenticated;
        private string _connectedAddress;

        public static bool IsAuthenticated => _instance._isAuthenticated;

        public static string ConnectedAddress => _instance._connectedAddress;

        public static void ShowAuthPanel() {
            _instance.authPanelCanvas.SetActive(true);
        }

        public static void HideAuthPanel() {
            _instance.authPanelCanvas.SetActive(false);
        }

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
                _instance = this;

                // Initialize the ThirdWeb SDK and wallet connection event listeners
                _sdk = new ThirdwebSDK("goerli");
                Initialize();

                // Set GameObject and AuthPanel to not be destroyed on scene load
                DontDestroyOnLoad(this.gameObject);
                DontDestroyOnLoad(authPanelCanvas);
            }

            _isAuthenticated = false;
        }

        private void Initialize() {
            authStatusText.text = "Connect Wallet";
            authButton.onClick.AddListener(() => { ConnectWallet(WalletProvider.MetaMask); });
        }

        private async void ConnectWallet(WalletProvider provider) {
            authStatusText.text = "Authenticating...";
            authButton.interactable = false;

            try {
                string address = await _sdk.wallet.Connect(new WalletConnection() {
                    provider = provider,
                    chainId = 5
                });

                authStatusText.text = $"Connected as {address}";
                _isAuthenticated = true;
                _connectedAddress = address;
            }
            catch (System.Exception e) {
                authStatusText.text = $"Error: {e.Message}";
            }
        }
    }
}