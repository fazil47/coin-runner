using System.Collections;
using System.Collections.Generic;
using Platformer.ThirdWeb;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.Scene {
    /// <summary>
    /// Manages the character select scene.
    /// </summary>
    public class CharacterSelectManager : MonoBehaviour {
        [SerializeField] private GameObject characterSelectCanvas;
        [SerializeField] private GameObject characterSelectPanel;
        [SerializeField] private Button selectCharacterButton;

        [SerializeField] private GameObject getCharacterPanel;
        [SerializeField] private Button getCharacterButton;

        // Start is called before the first frame update
        private async void Start() {
            // TODO: Deal with warning about async lambda.
            getCharacterButton.onClick.AddListener(async () => { await ThirdWebManager.GetCharacter(); });

            if (await ThirdWebManager.IsCharacterOwner()) {
                Debug.Log("You own a character.");
                getCharacterPanel.SetActive(false);
            }
            else {
                getCharacterPanel.SetActive(true);
            }

            // TODO: Handle this better, use LevelSelect scene.
            selectCharacterButton.onClick.AddListener(() => { AuthenticatedSceneManager.LoadScene("Level_1"); });
        }
    }
}