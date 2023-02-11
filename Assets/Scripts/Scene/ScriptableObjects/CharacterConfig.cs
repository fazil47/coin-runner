using System;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Scene {
    /// <summary>
    /// Scriptable object that contains the configuration for characters.
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "ScriptableObjects/CharacterConfig", order = 2)]
    public class CharacterConfig : ScriptableObject {
        public List<Tuple<GameObject, Sprite>> characters {
            get {
                List<Tuple<GameObject, Sprite>> characters = new List<Tuple<GameObject, Sprite>>();
                for (int i = 0; i < characterPrefabs.Count; i++) {
                    characters.Add(new Tuple<GameObject, Sprite>(characterPrefabs[i], characterSprites[i]));
                }

                return characters;
            }
        }

        [SerializeField] private List<GameObject> characterPrefabs;
        [SerializeField] private List<Sprite> characterSprites;
    }
}