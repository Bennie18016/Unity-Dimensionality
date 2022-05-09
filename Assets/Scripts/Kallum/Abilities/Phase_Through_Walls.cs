using PlayerController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Abilities{
    public class Phase_Through_Walls : MonoBehaviour
{

    public float cooldowntime = 1;
    public float nextPhaseTime = 0;
    public float currentDashTime = 0.0f;
    float dashCD = 5.0f; 
    float phaseCD = 5.0f;
    PhotonView PV;



    Movement moveScript;

    public float dashSpeed;
    public float dashTime;

    void Start()
    {
        moveScript = GetComponent< Movement>();

        PV = GetComponent<PhotonView>();

    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            dashCD -= Time.deltaTime;
            if (Input.GetKeyDown(KeyCode.E))
            {
                dashStart();
                PV.RPC("dashStart", RpcTarget.AllBuffered);
            }

            if (dashCD <= 0)
            {
                Physics.IgnoreLayerCollision(0, 8, false); //re enables the collider
            }
        }
    }

    [PunRPC]
    void dashStart()
    {
        if (dashCD <= 0)
        {
            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            moveScript.cc.Move(moveScript.moveDirection * dashSpeed * Time.deltaTime); //dashes
            Physics.IgnoreLayerCollision(0, 8);

            dashCD = 3f;

            yield return null; //returns a null value
        }
    }
}
}
