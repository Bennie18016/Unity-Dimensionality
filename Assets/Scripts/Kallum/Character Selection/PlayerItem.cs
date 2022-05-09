    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using Photon.Pun;
    using Photon.Realtime;
    using System.Linq;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public Text playerName;
    public GameObject leftArrowButton;
    public GameObject rightArrowButton;
    public GameObject readyButton;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public Image PlayerAvatar;
    public Sprite[] avatars;
    Player player;

    private void Start()
    {
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);

        readyButton.GetComponent<Button>().onClick.AddListener(() => Ready());
        CheckAllPlayersReady();

    }
    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;
        UpdatePlayerItem(player);
    }

    public void ApplyLocalChanges()
    {
        leftArrowButton.SetActive(true);
        rightArrowButton.SetActive(true);
        readyButton.SetActive(true);
    }

    public void OnClickLeftArrow()
    {
        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = avatars.Length - 1;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;

        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);

    }

    public void OnClickRightArrow()
    {
        if ((int)playerProperties["playerAvatar"] == avatars.Length - 1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;

        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
            UpdatePlayerReady(targetPlayer);

            if(PhotonNetwork.IsMasterClient){
                CheckAllPlayersReady();
            }
        }
        
    }

    //Bennie
    private void UpdatePlayerReady(Player targetPlayer)
    {
        if(player.CustomProperties.ContainsKey("Ready")){
            playerProperties["Ready"] = (bool)player.CustomProperties["Ready"];
        } else{
            playerProperties["Ready"] = (bool)false;
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("playerAvatar"))
        {
            PlayerAvatar.sprite = avatars[(int)player.CustomProperties["playerAvatar"]];
            playerProperties["playerAvatar"] = (int)player.CustomProperties["playerAvatar"];
        }
        else
        {
            playerProperties["playerAvatar"] = 0;
        }
    }

    //Bennie
    public void Ready(){
        playerProperties["Ready"] = (bool)true;
        readyButton.GetComponent<Button>().onClick.RemoveAllListeners();
        readyButton.GetComponent<Button>().onClick.AddListener(() => UnReady());
        readyButton.GetComponentInChildren<Text>().text = "Unready!";

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);

        if(!PhotonNetwork.IsMasterClient) return;

        CheckAllPlayersReady();
    }

    //Bennie
    private void CheckAllPlayersReady()
    {
        var lobbyPlayers = PhotonNetwork.PlayerList;

        if(lobbyPlayers.All(p => p.CustomProperties.ContainsKey("Ready") && (bool)p.CustomProperties["Ready"])){
            GameObject.Find("LobbyManager").GetComponent<LobbyManager>().ShowStartButton();
        } else {
            GameObject.Find("LobbyManager").GetComponent<LobbyManager>().HideStartButton();
        }

    }

    //Bennie
    public void UnReady(){
        playerProperties["Ready"] = false;
        readyButton.GetComponent<Button>().onClick.RemoveAllListeners();
        readyButton.GetComponent<Button>().onClick.AddListener(() => Ready());
        readyButton.GetComponentInChildren<Text>().text = "Ready!";

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);

        if(!PhotonNetwork.IsMasterClient) return;

        CheckAllPlayersReady();
        
    }

}
