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


        private void Start() {
            var button = GetComponent<Button>();
            var buttonText = GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = levelName;
            button.onClick.AddListener(() => { AuthenticatedSceneManager.LoadScene(levelSceneName); });
        }
    }
}