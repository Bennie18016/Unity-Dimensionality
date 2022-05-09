using Photon.Pun;
using UnityEngine;

public class RoomJoinManager : MonoBehaviourPunCallbacks
{

    // OnEnable is called as soon as the script is enabled and active
    public override void OnEnable()
    {
        // Adds this script as a callback so that it knows you are joining a room from the other script
        PhotonNetwork.AddCallbackTarget(this);
    }

    // OnDisable is when the script is disabled, destroyed or set as unactive
    public override void OnDisable()
    {
        // Removes this script from the callback to avoid errors
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    // Called when you join a room
    public override void OnJoinedRoom()
    {
        StartGame();
    }

    private void StartGame()
    {
        // Loads you into the level with the index of 1
        PhotonNetwork.LoadLevel(1);
    }
}
