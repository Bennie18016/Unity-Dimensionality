using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PlayerController;

public class RockStat : MonoBehaviour
{
    float reach;
    GameObject text;

    private void Start()
    { 
        text = GameObject.Find("Click F");
        reach = 1.5f;
    }

    private void Update()
    {
        if(DistanceFromRock(ClosestPlayer()) < reach && ClosestPlayer().GetComponent<PhotonView>().IsMine)
        {
            text.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                Pickup();
            }
        }
        else if(DistanceFromRock(ClosestPlayer()) > reach && ClosestPlayer().GetComponent<PhotonView>().IsMine)
        {
            text.SetActive(false);
        }
    }

    private GameObject ClosestPlayer()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 enemyPos = transform.position;

        foreach (GameObject target in targets)
        {
            Vector3 diff = target.transform.position - enemyPos;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = target;
                distance = curDistance;
            }
        }
        return closest;
    }

    private float DistanceFromRock(GameObject player)
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }

    private void Pickup()
    {
        PhotonNetwork.Destroy(gameObject);
        ClosestPlayer().GetComponent<Inventory>().rock++;
    }
}