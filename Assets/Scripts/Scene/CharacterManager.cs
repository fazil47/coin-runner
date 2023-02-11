using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Scene {
    /// <summary>
    /// A character manager for the entire game.
    /// </summary>
    public class CharacterManager : MonoBehaviour {
        public static GameObject CurrentCharacterPrefab { get; private set; }

        public static List<Tuple<GameObject, Sprite>> Characters => _instance.characterConfig.characters;

        [SerializeField] private CharacterConfig characterConfig;

        private static CharacterManager _instance;

        public static void SetCharacter(GameObject characterPrefab) {
            CurrentCharacterPrefab = characterPrefab;
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

        private void Start() {
            CurrentCharacterPrefab = characterConfig.characters[0].Item1;
        }
    }
}