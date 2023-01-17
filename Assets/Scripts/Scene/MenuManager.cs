using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.ThirdWeb;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Platformer.Scene {
    public class MenuManager : MonoBehaviour {
        [SerializeField] private GameObject menuCanvas;
        [SerializeField] private Button playButton;

        private void Start() {
            playButton.onClick.AddListener((() => {
                if (ThirdWebManager.IsAuthenticated) {
                    AuthenticatedSceneManager.LoadScene("Level_1");
                }
            }));
        }

        void Update() {
            if (ThirdWebManager.IsAuthenticated) {
                ThirdWebManager.HideAuthPanel();
                menuCanvas.SetActive(true);
            }
            else {
                menuCanvas.SetActive(false);
            }
        }
    }
}