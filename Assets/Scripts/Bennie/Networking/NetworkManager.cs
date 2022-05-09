using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Join Game Button in the scene
    public Button button;

    // Max amount of players that a room can hold
    private int maxPlayers = 4;
    void Start()
    {
        /*Gets the button component from the gameobject
        Makes the button uninteractable
        Adds the function "JoinGame" as a listener
        Gets the text component from the child of gameobjects component
        Makes the text say "Connecting To Server..."
        Automatically starts connecting you to the server*/

        button.GetComponent<Button>().interactable = false;
        button.GetComponent<Button>().onClick.AddListener(() => JoinGame());
        button.GetComponentInChildren<Text>().text = "Connecting To Server...";
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {

    }

    // OnConnectedToMaster is called when you to the server and is ready for matchmaking 
    public override void OnConnectedToMaster()
    {
        //Changes the text of the button to "Join Game" and makes it interactable
        button.GetComponentInChildren<Text>().text = "Join Game";
        button.GetComponent<Button>().interactable = true;
    }

    /* Function "JoinGame" which was added to the button
    "PhotonNetwork.JoinRandomRoom" makes you join a random room*/
    private void JoinGame()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    // OnJoinRandomFailed is called if it fails to connect to a room
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // Starts the function "CreateRoom"
        CreateRoom();
    }

    private void CreateRoom()
    {
        /*Gets a random number between 0 and 10,000 and calls it "roomName"
        Create a new RoomOptions called roomOps
        IsVisible determines if the room is visible in the lobby list and stops from being able to join from "JoinRandomRoom"
        IsOpen determines if you can join the room at all
        MaxPlayers is simply how many people can join the room. Must be in a "byte" format
        "CreateRoom" simply creates the room called "Room {random number generated in roomName}" with the options above*/
        int roomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)maxPlayers };
        PhotonNetwork.CreateRoom("Room" + roomName, roomOps);
    }


    // OnCreateRoomFailed is called if it fails to create a room
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // Tries again
        CreateRoom();
    }
}
