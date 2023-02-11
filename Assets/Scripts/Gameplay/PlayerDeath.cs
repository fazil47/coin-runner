using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Platformer.Core;
using Platformer.Mechanics;
using Platformer.Model;
using UnityEngine;
using Platformer.ThirdWeb;

namespace Platformer.Gameplay {
    /// <summary>
    /// Fired when the player has died.
    /// </summary>
    /// <typeparam name="PlayerDeath"></typeparam>
    public class PlayerDeath : Simulation.Event<PlayerDeath> {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override async void Execute() {
            PlayerController player = model.player;
            if (player.health.IsAlive) {
                player.health.Die();
                model.virtualCamera.m_Follow = null;
                model.virtualCamera.m_LookAt = null;
                // player.collider.enabled = false;
                player.controlEnabled = false;

                if (player.audioSource && player.ouchAudio) {
                    player.audioSource.PlayOneShot(player.ouchAudio);
                }

                player.animator.SetTrigger("hurt");
                player.animator.SetBool("dead", true);
                
                Simulation.Schedule<PostGame>(2);
            }
        }
    }
}