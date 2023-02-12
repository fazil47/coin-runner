using System;
using System.Collections;
using System.Collections.Generic;
using Platformer.Scene;
using Platformer.ThirdWeb;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Platformer.UI {
    [RequireComponent(typeof(Button))]
    public class SelectableCharacter : MonoBehaviour {
        public GameObject CharacterPrefab => characterPrefab;
        public bool IsOwned => _isOwned;

        [SerializeField] private GameObject characterPrefab;
        [SerializeField] private Image displayImage;
        [SerializeField] private GameObject selectedStatusPanel;
        [SerializeField] private TextMeshProUGUI selectedStatusText;
        [SerializeField] private GameObject costPanel;
        [SerializeField] private TextMeshProUGUI costText;

        private int _id;
        private int _cost = 0;
        private bool _isOwned = false;
        private bool _isSelected = false; // TODO: Maybe delete this

        public void SetCharacter(GameObject characterPrefab, Sprite sprite, int cost, int id, bool isOwned) {
            this.characterPrefab = characterPrefab;
            displayImage.sprite = sprite;
            _cost = cost;
            _id = id;
            costText.text = cost.ToString();
            _isOwned = isOwned;

            if (_isOwned) {
                costPanel.SetActive(false);
                selectedStatusPanel.SetActive(true);
            }
            else {
                costPanel.SetActive(true);
                selectedStatusPanel.SetActive(false);
            }
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

        public async void BuyCharacter() {
            if (!_isOwned) {
                bool result = await ThirdWebManager.BuyCharacter(_id);

                if (result) {
                    _isOwned = true;
                    costPanel.SetActive(false);
                    selectedStatusPanel.SetActive(true);

                    var button = GetComponent<Button>();
                    button.onClick.RemoveListener(BuyCharacter);
                    button.onClick.AddListener(SelectCharacter);
                }
                else {
                    Debug.Log("Failed to buy character");
                }
            }
            else {
                Debug.Log("Character already owned");
            }
        }

        private void Start() {
            var button = GetComponent<Button>();
            button.onClick.AddListener(() => {
                if (_isOwned) {
                    SelectCharacter();
                }
                else {
                    BuyCharacter();
                }
            });
        }
    }
}