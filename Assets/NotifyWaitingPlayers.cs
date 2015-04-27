using UnityEngine;
using System.Collections;
using HappyFunTimes;

namespace HappyFunTimesExample {

public class NotifyWaitingPlayers : MonoBehaviour {

    // This gives you a chance to send that player's phone
    // a command to tell it to display "The game is full" or
    // whatever you want.
    //
    // Note: You can call PlayerController.ReturnPlayer to eject
    // a player from their slot and get a new player for that slot
    // If you do that this function will be called for the returned
    // player.
    //
    // Simiarly you can call PlayerController.GetNewPlayers to
    // eject all current players in which case this will be called
    // for all players that were player.
    void WaitingNetPlayer(SpawnInfo spawnInfo) {
        // Tell the controller to display full message
        spawnInfo.netPlayer.SendCmd("full");
    }
}

}  // namespace HappyFunTimesExample

