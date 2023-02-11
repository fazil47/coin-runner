using System;
using System.Collections;
using System.Collections.Generic;
using Platformer.Scene;
using UnityEngine;

namespace Platformer.Scene {
    /// <summary>
    /// A level manager for the entire game.
    /// </summary>
    public class LevelManager : MonoBehaviour {
        public static bool IsInLevel { get; private set; }
        public static string CurrentLevelSceneName { get; private set; }
        public static bool IsLastLevel { get; private set; }

        public static List<string> LevelSceneNames => _instance.levelConfig.LevelSceneNames;

        [SerializeField] private LevelConfig levelConfig;

        private static LevelManager _instance;

        public static void LoadLevel(string levelSceneName) {
            if (levelSceneName == null) {
                Debug.LogError("Level scene name is null");
                return;
            }

            if (!LevelSceneNames.Contains(levelSceneName)) {
                Debug.LogError("Level scene name is not in level config");
                return;
            }

            IsInLevel = true;
            CurrentLevelSceneName = levelSceneName;
            IsLastLevel = _instance.levelConfig.GetNextLevelSceneName(levelSceneName) == null;

            AuthenticatedSceneManager.LoadScene(levelSceneName);
        }

        public static void ReloadLevel() {
            AuthenticatedSceneManager.LoadScene(CurrentLevelSceneName);
        }

        public static void LoadNextLevel() {
            if (IsLastLevel) {
                Debug.LogError("No next level found");
                return;
            }

            string nextLevelSceneName = _instance.levelConfig.GetNextLevelSceneName(CurrentLevelSceneName);

            IsInLevel = true;
            CurrentLevelSceneName = nextLevelSceneName;
            IsLastLevel = _instance.levelConfig.GetNextLevelSceneName(nextLevelSceneName) == null;

            AuthenticatedSceneManager.LoadScene(nextLevelSceneName);
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