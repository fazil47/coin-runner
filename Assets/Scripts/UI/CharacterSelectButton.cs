using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.UI {
    [RequireComponent(typeof(Button))]
    [RequireComponent(typeof(Image))]
    public class CharacterSelectButton : MonoBehaviour {
        public void Initialize(Sprite sprite, string name, int index) {
            var button = GetComponent<Button>();
            var image = GetComponent<Image>();

            image.sprite = sprite;
            button.onClick.AddListener(() => {
                // TODO: Set the selected character.
            });
        }
    }
}