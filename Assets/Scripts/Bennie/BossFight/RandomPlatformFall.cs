using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RandomPlatformFall : MonoBehaviour
{
    List<GameObject> platforms = new List<GameObject>();
    GameObject disabled;

    bool picked;
    bool reset;

    float maxTime;
    float time;
    float shakeTime;
    float resetTime;
    int turns;

    PhotonView PV;

    void Start()
    {
        platforms.AddRange(GameObject.FindGameObjectsWithTag("Platforms"));

        maxTime = 25;
        shakeTime = 20;
        resetTime = 10;
        time = 20;

        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            time += 1 * Time.deltaTime;

            if (time > maxTime)
            {
                PV.RPC("DisableDisabled", RpcTarget.All);
                ResetTime();
            }

            if (time > shakeTime && !picked)
            {
                int Object = Random.Range(0, 7) + 1;
                PV.RPC("NewDisabled", RpcTarget.All, Object);
            }

            if (time > resetTime && reset)
            {

                PV.RPC("ResetDisabled", RpcTarget.All);
            }

            if (turns == 2)
            {
                maxTime = 18;
                shakeTime = 15;
            }

            else if (turns == 4)
            {
                maxTime = 15;
                shakeTime = 13;
            }

            else if (turns == 6)
            {
                maxTime = 7;
                shakeTime = 5;
                resetTime = 4;
            }

            else if (turns == 13)
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.Play();
                Destroy(this);
            }
        }
    }
    private void ResetTime()
    {
        Debug.Log("Resetting Timer");
        time = 0;
        picked = false;
        reset = true;
    }

    [PunRPC]
    private void DisableDisabled()
    {
        if (disabled != null)
        {
            Debug.Log("Deactivating Platform");
            disabled.SetActive(false);
            disabled.GetComponent<Animator>().SetBool("Shake", false);
        }
    }

    [PunRPC]
    private void NewDisabled(int Object)
    {
        Debug.Log("Picking New Platform");
        picked = true;
        disabled = platforms[Object];
        disabled.GetComponent<Animator>().SetBool("Shake", true);
    }

    [PunRPC]
    private void ResetDisabled()
    {
        Debug.Log("Resetting");
        reset = false;
        if (disabled != null) { disabled.SetActive(true); }
        disabled = null;
        turns++;
    }
}