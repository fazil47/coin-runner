using Platformer.Core;
using Platformer.Gameplay;
using Platformer.Scene;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using UnityEngine.UI;
using Platformer.ThirdWeb;
using System.Threading.Tasks;

namespace Platformer.UI {
    public class PostGameUIController : MonoBehaviour {
        [SerializeField] private GameObject postGameUI;
        [SerializeField] private GameObject claimCoinsUI;
        [SerializeField] private GameObject progressUI;
        [SerializeField] private GameObject successUI;
        [SerializeField] private GameObject failureUI;
        [SerializeField] private Button claimCoinsButton;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button replayButton;
        [SerializeField] private Button nextButton;

        public void RefreshUI() {
            InitializeUI();
        }

        public void InitializePostGameUIController() {
            postGameUI.SetActive(false);

            InitializeUI();

            if (LevelManager.IsLastLevel) {
                nextButton.gameObject.SetActive(false);
            }

            claimCoinsButton.onClick.AddListener(async () => {
                PlayerController player = Simulation.GetModel<PlatformerModel>().player;
                int coinCount = player.Coins;

                if (coinCount > 0) {
                    claimCoinsUI.SetActive(false);
                    progressUI.SetActive(true);

                    bool result = await GetCoin(coinCount.ToString());

                    progressUI.SetActive(false);

                    if (result) {
                        successUI.SetActive(true);
                    }
                    else {
                        failureUI.SetActive(true);
                    }
                }
                else {
                    claimCoinsButton.gameObject.SetActive(false);
                }

                // TODO: Remove this if not needed when replay and next buttons are properly implemented
                player.ResetCoins();
            });

            menuButton.onClick.AddListener(() => { AuthenticatedSceneManager.LoadScene("Start"); });

            replayButton.onClick.AddListener(() => {
                InitializeUI();

                // Simulation.Schedule<PlayerSpawn>(2);
                // postGameUI.SetActive(false);

                LevelManager.ReloadLevel();
            });

            nextButton.onClick.AddListener(() => {
                InitializeUI();

                if (!LevelManager.IsLastLevel) {
                    LevelManager.LoadNextLevel();
                }
            });
        }

        private void InitializeUI() {
            PlayerController player = Simulation.GetModel<PlatformerModel>().player;
            int coinCount = player.Coins;

            if (coinCount > 0) {
                claimCoinsButton.gameObject.SetActive(true);
            }
            else {
                claimCoinsButton.gameObject.SetActive(false);
            }

            claimCoinsUI.SetActive(true);
            progressUI.SetActive(false);
            successUI.SetActive(false);
            failureUI.SetActive(false);
        }

        private async Task<bool> GetCoin(string coinCount) {
            return await ThirdWebManager.GetCoin(coinCount);
        }
    }
}