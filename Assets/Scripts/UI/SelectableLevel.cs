using System;
using Platformer.Scene;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.UI {
    [RequireComponent(typeof(Button))]
    public class SelectableLevel : MonoBehaviour {
        [SerializeField] private String levelName;
        [SerializeField] private String levelSceneName;

        public void SetLevelName(string levelName) {
            this.levelName = levelName;
        }

        public void SetLevelSceneName(string levelSceneName) {
            this.levelSceneName = levelSceneName;
        }

        private void Start() {
            var button = GetComponent<Button>();
            var buttonText = GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = levelName;
            button.onClick.AddListener(() => { LevelManager.LoadLevel(levelSceneName); });
        }
    }
}