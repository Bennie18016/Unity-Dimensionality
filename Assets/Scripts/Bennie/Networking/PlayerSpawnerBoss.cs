using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawnerBoss : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

    void Start()
    {
        int randomNumber = Random.Range(0, spawnPoints.Length);

        GameObject playerToSpawn = playerPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["playerAvatar"]];

        Transform spawnPoint = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber];
        PhotonNetwork.Instantiate(playerToSpawn.name, spawnPoint.position, Quaternion.identity);

    }
}
