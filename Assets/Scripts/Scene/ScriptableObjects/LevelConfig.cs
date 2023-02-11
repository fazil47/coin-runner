using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Scene {
    /// <summary>
    /// Scriptable object that contains the configuration for levels.
    /// </summary>
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "ScriptableObjects/LevelConfig", order = 1)]
    public class LevelConfig : ScriptableObject {
        public List<string> LevelSceneNames => levelSceneNames;

        [SerializeField] private List<string> levelSceneNames;

        public string GetLevelSceneName(int levelIndex) {
            return levelSceneNames[levelIndex];
        }

        public string GetNextLevelSceneName(string currentLevelSceneName) {
            int levelIndex = GetLevelIndex(currentLevelSceneName);
            if (levelIndex == -1) {
                return null;
            }

            if (levelIndex + 1 >= levelSceneNames.Count) {
                return null;
            }

            return levelSceneNames[levelIndex + 1];
        }

        private int GetLevelIndex(string currentLevelSceneName) {
            for (int i = 0; i < levelSceneNames.Count; i++) {
                if (levelSceneNames[i] == currentLevelSceneName) {
                    return i;
                }
            }

            return -1;
        }
    }
}