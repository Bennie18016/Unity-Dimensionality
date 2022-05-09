using Photon.Pun;
using System.IO;
using UnityEngine;

public class GameSetupManager : MonoBehaviour
{
    public GameObject UI;

    //Called when the script is active (after OnEnable)
    void Start()
    {
        // CreatePlayer();
    }


    //void CreatePlayer()
    //{
        // Spawns you in as the prefab "PhotonPlayer" from the folder Resources/PhotonPrefabs
    //    PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer"), Vector3.zero, Quaternion.identity);
    //}



    // Kallums Code
    public void Create9D()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer9D"), Vector3.zero, Quaternion.identity);
        UI.SetActive(false);
    }

    public void Create2D()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer2D"), Vector3.zero, Quaternion.identity);
        UI.SetActive(false);
    }

    public void Create5D()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonPlayer5D"), Vector3.zero, Quaternion.identity);
        UI.SetActive(false);
    }
}
