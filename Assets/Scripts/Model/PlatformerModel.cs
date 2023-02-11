using Platformer.Mechanics;
using Platformer.UI;
using TMPro;
using UnityEngine;

namespace Platformer.Model {
    /// <summary>
    /// The main model containing needed data to implement a platformer style 
    /// game. This class should only contain data, and methods that operate 
    /// on the data. It is initialised with data in the GameController class.
    /// </summary>
    [System.Serializable]
    public class PlatformerModel {
        /// <summary>
        /// The virtual camera in the scene.
        /// </summary>
        public Cinemachine.CinemachineVirtualCamera virtualCamera;

        /// <summary>
        /// The main component which controls the player sprite, controlled 
        /// by the user.
        /// </summary>
        public PlayerController player;

        /// <summary>
        /// The spawn point in the scene.
        /// </summary>
        public Transform spawnPoint;

        /// <summary>
        /// A global jump modifier applied to all initial jump velocities.
        /// </summary>
        public float jumpModifier = 1.5f;

        /// <summary>
        /// A global jump modifier applied to slow down an active jump when 
        /// the user releases the jump input.
        /// </summary>
        public float jumpDeceleration = 0.5f;

        // /// <summary>
        // /// The coins earned in a level.
        // /// </summary>
        // public int coinCount = 0;

        /// <summary>
        /// The post game UI for accepting the coins earned as ERC20 tokens.
        /// </summary>
        public PostGameUIController postGameUI;

        /// <summary>
        /// The text component for displaying the coin count in the post game UI.
        /// </summary>
        public TextMeshProUGUI postGameCoinCount;
    }
}