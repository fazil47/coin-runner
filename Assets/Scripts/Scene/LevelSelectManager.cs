using System.Collections;
using System.Collections.Generic;
using Platformer.UI;
using UnityEngine;

namespace Platformer.Scene {
    /// <summary>
    /// Manages the level select scene.
    /// </summary>
    public class LevelSelectManager : MonoBehaviour {
        [SerializeField] private GameObject levelSelectGrid;

        // Start is called before the first frame update
        private void Start() {
            // TODO: Use a scene object to store the levels.
            // var selectableLevels = levelSelectGrid.GetComponentsInChildren<SelectableLevel>();
            // foreach (var selectableLevel in selectableLevels) {
            //     
            // }
        }
    }
}