using Platformer.Core;
using Platformer.Model;
using Platformer.UI;
using TMPro;

namespace Platformer.Gameplay {
    /// <summary>
    /// Fired when the player is spawned after dying.
    /// </summary>
    public class PostGame : Simulation.Event<PostGame> {
        PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public override void Execute() {
            PostGameUIController postGameUI = model.postGameUI;
            TextMeshProUGUI postGameCoinCount = model.postGameCoinCount;

            postGameUI.gameObject.SetActive(true);
            postGameUI.RefreshUI();
            postGameCoinCount.text = model.player.Coins.ToString();
        }
    }
}