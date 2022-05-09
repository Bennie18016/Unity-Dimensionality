using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class DemoLeave : MonoBehaviour
{
    float time;
    float maxTime;
    void Start()
    {
        maxTime = 10;
    }

    // Update is called once per frame
    void Update()
    {
        time += 1 * Time.deltaTime;

        if (time >= maxTime)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                PhotonNetwork.LeaveRoom();
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
                SceneManager.LoadScene(0);
            }
        }
    }
}
