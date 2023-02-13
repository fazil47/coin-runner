using Platformer.Scene;
using Platformer.ThirdWeb;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Platformer.UI {
    [RequireComponent(typeof(Button))]
    public class SelectableCharacter : MonoBehaviour {
        public GameObject CharacterPrefab => _characterPrefab;
        public bool IsOwned => _isOwned;

        [SerializeField] private Image displayImage;
        [SerializeField] private GameObject selectedStatusPanel;
        [SerializeField] private TextMeshProUGUI selectedStatusText;
        [SerializeField] private GameObject buyStatusPanel;
        [SerializeField] private TextMeshProUGUI buyStatusText;
        [SerializeField] private GameObject costPanel;
        [SerializeField] private TextMeshProUGUI costText;

        private GameObject _characterPrefab;
        private int _id;
        private int _cost;

        private bool _isOwned;

        public void SetCharacter(GameObject characterPrefab, Sprite sprite, int cost, int id, bool isOwned) {
            _characterPrefab = characterPrefab;
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

                if (_cost > ThirdWebManager.GetCoinBalance()) {
                    GetComponent<Button>().interactable = false;
                }
            }
        }

        public void AddListener(UnityAction onClick) {
            var button = GetComponent<Button>();
            button.onClick.AddListener(onClick);
        }

        public void SelectCharacter() {
            selectedStatusText.text = "Selected";
            CharacterManager.SetCharacter(_characterPrefab);
        }

        public void DeselectCharacter() {
            selectedStatusText.text = "Select";
        }

        private async void BuyCharacter() {
            if (!_isOwned) {
                costPanel.SetActive(false);
                buyStatusPanel.SetActive(true);
                buyStatusText.text = "Buying...";

                bool result = await ThirdWebManager.BuyCharacter(_id);

                if (result) {
                    _isOwned = true;
                    costPanel.SetActive(false);
                    buyStatusPanel.SetActive(false);
                    selectedStatusPanel.SetActive(true);

                    var button = GetComponent<Button>();
                    button.onClick.RemoveListener(BuyCharacter);
                    button.onClick.AddListener(SelectCharacter);
                }
                else {
                    Debug.Log("Failed to buy character");
                    buyStatusText.text = "Failed";
                }
            }
            else {
                Debug.Log("Character already owned");
                selectedStatusPanel.SetActive(true);
                buyStatusPanel.SetActive(false);
                costPanel.SetActive(false);
            }
        }

        private void Start() {
            buyStatusPanel.SetActive(false);

            var button = GetComponent<Button>();
            if (_isOwned) {
                button.onClick.AddListener(SelectCharacter);
            }
            else {
                button.onClick.AddListener(BuyCharacter);
            }
        }
    }
}