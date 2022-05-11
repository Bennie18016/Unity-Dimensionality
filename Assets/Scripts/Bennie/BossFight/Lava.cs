using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Lava : MonoBehaviour
{
    public Camera spectateCam;
    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player"){
            if(other.gameObject.GetComponent<PhotonView>().IsMine){
                GameObject.Find("Canvas").SetActive(false);
                spectateCam.GetComponent<Camera>().enabled = true;
                spectateCam.gameObject.AddComponent<AudioListener>();
            }
            other.gameObject.SetActive(false);
            
        }
    }
}
