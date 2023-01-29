using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.ThirdWeb;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Platformer.Scene {
    /// <summary>
    /// Manages the start scene.
    /// </summary>
    public class StartManager : MonoBehaviour {
        [SerializeField] private GameObject startCanvas;
        [SerializeField] private Button playButton;

        private void Start() {
            playButton.onClick.AddListener((() => {
                if (ThirdWebManager.IsAuthenticated) {
                    AuthenticatedSceneManager.LoadScene("CharacterSelect");
                }
            }));
        }

        private void Update() {
            if (ThirdWebManager.IsAuthenticated) {
                ThirdWebManager.HideAuthPanel();
                startCanvas.SetActive(true);
            }
            else {
                startCanvas.SetActive(false);
            }
        }
    }
}