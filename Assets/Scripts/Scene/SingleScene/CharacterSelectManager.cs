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
                bool ownsCharacter = await ThirdWebManager.GetAirdropCharacter();
                if (ownsCharacter) {
                    getCharacterPanel.SetActive(false);
                }
            });

            doneButton.onClick.AddListener(() => { AuthenticatedSceneManager.LoadScene("Start"); });

            // TODO: Using await in the if statement below blocks the game. Use a waiting screen until the result is known.
            bool isAirdropCharacterOwner = await ThirdWebManager.IsAirdropCharacterOwner();

            if (isAirdropCharacterOwner) {
                Debug.Log("You own the airdrop character.");
                getCharacterPanel.SetActive(false);
            }
            else {
                getCharacterPanel.SetActive(true);
            }

            var selectableCharacters = new List<SelectableCharacter>();

            // AirDrop Character.
            GameObject selectableAirdropCharacter =
                Instantiate(selectableCharacterPrefab, characterSelectGrid.transform);
            // Airdrop character is set to owned no matter what.
            selectableAirdropCharacter.GetComponent<SelectableCharacter>().SetCharacter(
                CharacterManager.AirdropCharacter.Item1,
                CharacterManager.AirdropCharacter.Item2, 0, 0, true);
            selectableCharacters.Add(selectableAirdropCharacter.GetComponent<SelectableCharacter>());

            // Characters that can be bought.
            foreach (Tuple<GameObject, Sprite, int, int> character in CharacterManager.Characters) {
                GameObject selectableCharacter = Instantiate(selectableCharacterPrefab, characterSelectGrid.transform);
                bool isOwned = await ThirdWebManager.IsCharacterOwner(character.Item4);
                selectableCharacter.GetComponent<SelectableCharacter>().SetCharacter(character.Item1, character.Item2,
                    character.Item3, character.Item4, isOwned);
                selectableCharacters.Add(selectableCharacter.GetComponent<SelectableCharacter>());
            }

            foreach (SelectableCharacter selectableCharacter in selectableCharacters) {
                if (selectableCharacter.CharacterPrefab == CharacterManager.CurrentCharacterPrefab) {
                    selectableCharacter.SelectCharacter();
                }

                // For deselecting all other characters when a character is selected.
                selectableCharacter.AddListener(() => {
                    if (selectableCharacter.IsOwned) {
                        foreach (var otherSelectableCharacter in selectableCharacters) {
                            if (otherSelectableCharacter != selectableCharacter) {
                                otherSelectableCharacter.DeselectCharacter();
                            }
                        }
                    }
                });
            }
        }
    }
}