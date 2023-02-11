using System;
using System.Collections;
using System.Collections.Generic;
using Platformer.ThirdWeb;
using Platformer.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Scene {
    /// <summary>
    /// Manages the character select scene.
    /// </summary>
    public class CharacterSelectManager : MonoBehaviour {
        [SerializeField] private GameObject selectableCharacterPrefab;
        [SerializeField] private GameObject characterSelectGrid;
        [SerializeField] private GameObject getCharacterPanel;
        [SerializeField] private Button doneButton;
        [SerializeField] private Button getCharacterButton;


        // Start is called before the first frame update
        private async void Start() {
            // TODO: Deal with warning about async lambda.
            getCharacterButton.onClick.AddListener(async () => {
                bool ownsCharacter = await ThirdWebManager.GetCharacter();
                if (ownsCharacter) {
                    getCharacterPanel.SetActive(false);
                }
            });

            doneButton.onClick.AddListener(() => { AuthenticatedSceneManager.LoadScene("Start"); });

            // TODO: Using await in the if statement below blocks the game. Use a waiting screen until the result is known.
#if !UNITY_EDITOR
            if (await ThirdWebManager.IsCharacterOwner()) {
                Debug.Log("You own the basic character.");
                getCharacterPanel.SetActive(false);
            } else {
                getCharacterPanel.SetActive(true);
            }
#endif

            var selectableCharacters = new List<SelectableCharacter>();
            foreach (Tuple<GameObject, Sprite> character in CharacterManager.Characters) {
                GameObject selectableCharacter = Instantiate(selectableCharacterPrefab, characterSelectGrid.transform);
                selectableCharacter.GetComponent<SelectableCharacter>().SetCharacter(character.Item1, character.Item2);
                selectableCharacters.Add(selectableCharacter.GetComponent<SelectableCharacter>());
            }


            // var selectableCharacters = characterSelectGrid.GetComponentsInChildren<SelectableCharacter>();
            // selectableCharacters[0].SelectCharacter();
            foreach (SelectableCharacter selectableCharacter in selectableCharacters) {
                if (selectableCharacter.CharacterPrefab == CharacterManager.CurrentCharacterPrefab) {
                    selectableCharacter.SelectCharacter();
                }

                // For deselecting all other characters when a character is selected.
                selectableCharacter.AddListener(() => {
                    foreach (var otherSelectableCharacter in selectableCharacters) {
                        if (otherSelectableCharacter != selectableCharacter) {
                            otherSelectableCharacter.DeselectCharacter();
                        }
                    }
                });
            }
        }
    }
}