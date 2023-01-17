using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Platformer.ThirdWeb;

namespace Platformer.Scene {
    /// <summary>
    /// Class for managing the game scenes.
    /// </summary>
    public class AuthenticatedSceneManager : MonoBehaviour {
        private static AuthenticatedSceneManager _instance;

        public static void LoadScene(string sceneName) {
            // TODO: Should I instead throw an exception if not authenticated.
            if (ThirdWebManager.IsAuthenticated) {
                _instance.StartCoroutine(_instance.LoadSceneAsync(sceneName));
            }
            else {
                _instance.StartCoroutine(_instance.LoadSceneAsync("Menu"));
            }
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

        IEnumerator LoadSceneAsync(string sceneName) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone) {
                yield return null;
            }
        }
    }
}