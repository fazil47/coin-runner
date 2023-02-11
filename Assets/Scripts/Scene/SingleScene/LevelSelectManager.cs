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
        [SerializeField] private SelectableLevel selectableLevelPrefab;

        private void Start() {
            for (int i = 0; i < LevelManager.LevelSceneNames.Count; i++) {
                var selectableLevel = Instantiate(selectableLevelPrefab, levelSelectGrid.transform);
                selectableLevel.SetLevelName((i + 1).ToString());
                selectableLevel.SetLevelSceneName(LevelManager.LevelSceneNames[i]);
            }
        }
    }
}