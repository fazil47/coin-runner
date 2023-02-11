using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.ThirdWeb;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Platformer.Scene {
    /// <summary>
    /// Manages the start scene.
    /// </summary>
    public class StartManager : MonoBehaviour {
        [SerializeField] private GameObject startCanvas;
        [SerializeField] private TextMeshProUGUI coinCountText;
        [SerializeField] private Button playButton;
        [SerializeField] private Button characterSelectButton;

        private bool _hasToBeReloaded = true;

        private void Start() {
            
            
#if UNITY_EDITOR
            playButton.onClick.AddListener(() => { AuthenticatedSceneManager.LoadScene("LevelSelect"); });

            characterSelectButton.onClick.AddListener(() => {
                AuthenticatedSceneManager.LoadScene("CharacterSelect");
            });
#else
            playButton.onClick.AddListener(() => {
                if (ThirdWebManager.IsAuthenticated) {
                    AuthenticatedSceneManager.LoadScene("LevelSelect");
                }
            });

            characterSelectButton.onClick.AddListener(() => {
                if (ThirdWebManager.IsAuthenticated) {
                    AuthenticatedSceneManager.LoadScene("CharacterSelect");
                }
            });
#endif
        }

        private async void UpdateCoinBalance() {
            coinCountText.text = await ThirdWebManager.GetCoinBalance();
            _hasToBeReloaded = false;
        }

        private void Update() {
#if UNITY_EDITOR
            startCanvas.SetActive(true);
#else
            if (ThirdWebManager.IsAuthenticated) {
                if (_hasToBeReloaded) {
                    UpdateCoinBalance();
                }
                ThirdWebManager.HideAuthPanel();
                startCanvas.SetActive(true);
            }
            else {
                startCanvas.SetActive(false);
                ThirdWebManager.ShowAuthPanel();
            }
#endif
        }
    }
}