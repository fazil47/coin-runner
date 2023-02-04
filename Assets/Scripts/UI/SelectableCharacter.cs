using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Platformer.UI {
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class SelectableCharacter : MonoBehaviour {
        public GameObject CharacterPrefab => characterPrefab;

        [SerializeField] private GameObject characterPrefab;

        public void Initialize(UnityAction onClick) {
            var button = GetComponent<Button>();

            // var image = GetComponent<Image>();
            // image.sprite = sprite;

            button.onClick.AddListener(onClick);
        }
    }
}