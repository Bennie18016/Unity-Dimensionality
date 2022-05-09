using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using PlayerController;

public class Chest : MonoBehaviour
{
    public int wood;
    public int rope;
    public int rock;
    public int gold;

    GameObject text;

    float reach;

    void Start()
    {
        reach = 1.5f;

        text = new GameObject();
        text.name = gameObject.name + " text";
        text.AddComponent<Text>();
        text.GetComponent<Text>().text = "Click F to search chest";
        text.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        GameObject c = GameObject.Find("Canvas");
        text.transform.SetParent(c.transform);
        text.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -75, 0);
        text.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (DistanceFromChest(ClosestPlayer()) < reach && ClosestPlayer().GetComponent<PhotonView>().IsMine)
        {
            text.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                SearchChest();
            }
        }
        else if (DistanceFromChest(ClosestPlayer()) > reach && ClosestPlayer().GetComponent<PhotonView>().IsMine)
        {
            text.SetActive(false);
        }
    }

    private void SearchChest()
    {
        Inventory inv = ClosestPlayer().GetComponent<Inventory>();

        if (ClosestPlayer().GetComponent<PhotonView>().IsMine)
        {
            inv.wood += wood;
            inv.rope += rope;
            inv.rock += rock;
            inv.gold += gold;
        }
        
        Destroy(text);
        PhotonNetwork.Destroy(gameObject);
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

    private float DistanceFromChest(GameObject player)
    {
        return Vector3.Distance(player.transform.position, transform.position);
    }
}
