using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using Platformer.UI;
using TMPro;
using UnityEngine;

namespace Platformer.Scene {
    /// <summary>
    /// A manager for instantiating the player in a level.
    /// </summary>
    public class LevelPlayerManager : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI coinCountText;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private PostGameUIController postGameUIController;

        // Start is called before the first frame update
        void Start() {
            PlayerController player =
                Instantiate(CharacterManager.CurrentCharacterPrefab, Vector3.zero, Quaternion.identity)
                    .GetComponent<PlayerController>();
            player.SetCoinCountText(coinCountText);

            PlatformerModel model = Simulation.GetModel<PlatformerModel>();
            model.player = player;

            virtualCamera.Follow = player.transform;
            virtualCamera.LookAt = player.transform;

            postGameUIController.InitializePostGameUIController();
        }
    }
}