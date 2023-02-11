using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Scene {
    /// <summary>
    /// A manager for the entire game.
    /// </summary>
    public class GameManager : MonoBehaviour {
        public static GameObject SelectedCharacter { get; private set; }

        private static GameManager _instance;

        public static void SetSelectedCharacter(GameObject character) {
            SelectedCharacter = character;
        }

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(gameObject);
            }
            else {
                _instance = this;

                // Set GameObject to not be destroyed on scene load
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}