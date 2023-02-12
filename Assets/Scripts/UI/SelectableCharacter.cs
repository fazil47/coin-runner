using System;
using System.Collections;
using System.Collections.Generic;
using Platformer.Scene;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Platformer.UI {
    [RequireComponent(typeof(Button))]
    public class SelectableCharacter : MonoBehaviour {
        public GameObject CharacterPrefab => characterPrefab;

        [SerializeField] private GameObject characterPrefab;
        [SerializeField] private Image displayImage;
        [SerializeField] private TextMeshProUGUI selectedStatusText;

        // TODO: Maybe delete this
        private bool _isSelected = false;

        public void SetCharacter(GameObject characterPrefab, Sprite sprite) {
            this.characterPrefab = characterPrefab;
            displayImage.sprite = sprite;
        }

        public void AddListener(UnityAction onClick) {
            var button = GetComponent<Button>();
            button.onClick.AddListener(onClick);
        }

        public void SelectCharacter() {
            _isSelected = true;
            selectedStatusText.text = "Selected";
            CharacterManager.SetCharacter(characterPrefab);
        }

        public void DeselectCharacter() {
            _isSelected = false;
            selectedStatusText.text = "Select";
        }

        private void Start() {
            var button = GetComponent<Button>();
            button.onClick.AddListener(SelectCharacter);
        }
    }
}