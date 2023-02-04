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
#if UNITY_EDITOR
            playButton.onClick.AddListener((() => { AuthenticatedSceneManager.LoadScene("CharacterSelect"); }));
#else
            playButton.onClick.AddListener((() => {
                if (ThirdWebManager.IsAuthenticated) {
                    AuthenticatedSceneManager.LoadScene("CharacterSelect");
                }
            }));
#endif
        }

        private void Update() {
#if UNITY_EDITOR
            startCanvas.SetActive(true);
#else
            if (ThirdWebManager.IsAuthenticated) {
                ThirdWebManager.HideAuthPanel();
                startCanvas.SetActive(true);
            }
            else {
                startCanvas.SetActive(false);
            }
#endif
        }
    }
}