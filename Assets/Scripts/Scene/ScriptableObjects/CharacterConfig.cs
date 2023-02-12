using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Platformer.Scene {
    /// <summary>
    /// Scriptable object that contains the configuration for characters.
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "ScriptableObjects/CharacterConfig", order = 2)]
    public class CharacterConfig : ScriptableObject {
        public Tuple<GameObject, Sprite, int, int> AirdropCharacter {
            get {
                return new Tuple<GameObject, Sprite, int, int>(airdropCharacterPrefab, airdropCharacterSprite,
                    0, airdropCharacterId);
            }
        }

        public List<Tuple<GameObject, Sprite, int, int>> Characters {
            get {
                var characters = new List<Tuple<GameObject, Sprite, int, int>>();
                for (int i = 0; i < characterPrefabs.Count; i++) {
                    characters.Add(
                        new Tuple<GameObject, Sprite, int, int>(characterPrefabs[i], characterSprites[i],
                            characterCosts[i], characterIds[i]));
                }

                return characters;
            }
        }

        [Header("Character Collection Contract")]
        [Tooltip("The address of the character collection contract.")]
        [SerializeField]
        private string characterCollectionAddress;

        // For the airdrop character
        [Header("Airdrop Character Config")] [Tooltip("The airdrop character prefab.")] [SerializeField]
        private GameObject airdropCharacterPrefab;

        [Tooltip("The airdrop character sprite.")] [SerializeField]
        private Sprite airdropCharacterSprite;

        [Tooltip("The airdrop character address.")] [SerializeField]
        private int airdropCharacterId;


        // For characters that have to be bought
        [Header("Character Config")]
        [
            Tooltip(
                "The list of character prefabs. The order of the prefabs in this list should match the order of the other lists.")]
        [
            SerializeField
        ]
        private List<GameObject> characterPrefabs;

        [Tooltip(
            "The list of character sprites. The order of the sprites in this list should match the order of the other lists.")]
        [SerializeField]
        private List<Sprite> characterSprites;

        [Tooltip(
            "The list of character costs. The order of the costs in this list should match the order of the other lists.")]
        [SerializeField]
        private List<int> characterCosts;

        [Tooltip(
            "The list of character addresses. The order of the addresses in this list should match the order of the other lists.")]
        [SerializeField]
        private List<int> characterIds;
    }
}