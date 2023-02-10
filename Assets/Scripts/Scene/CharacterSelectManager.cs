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
        [SerializeField] private GameObject characterSelectGrid;
        [SerializeField] private GameObject getCharacterPanel;
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

#if !UNITY_EDITOR
            if (await ThirdWebManager.IsCharacterOwner()) {
                Debug.Log("You own the basic character.");
                getCharacterPanel.SetActive(false);
            } else {
                getCharacterPanel.SetActive(true);
            }
#endif
            // selectCharacterButton.onClick.AddListener(() => { AuthenticatedSceneManager.LoadScene("Level_1"); });

            var selectableCharacters = characterSelectGrid.GetComponentsInChildren<SelectableCharacter>();
            foreach (var selectableCharacter in selectableCharacters) {
                selectableCharacter.Initialize(() => {
                    SetCharacter(selectableCharacter.CharacterPrefab);
                    AuthenticatedSceneManager.LoadScene("LevelSelect");
                });
            }
        }

        // TODO: Set character in game manager and go to level select scene.
        private void SetCharacter(GameObject characterPrefab) {
            // TODO: Set the selected character.
            Debug.Log(characterPrefab);
        }
    }
}