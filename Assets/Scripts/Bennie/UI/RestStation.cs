using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using DayNight;

public class RestStation : MonoBehaviour
{

    List<GameObject> players = new List<GameObject>();
    public Transform[] sleepPoints;

    GameObject text;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            players.Add(other.gameObject);
        }
    }

    void Start()
    {
        text = PhotonNetwork.Instantiate("GameObject", Vector3.zero, Quaternion.identity);
        text.name = gameObject.name + " text";
        text.AddComponent<Text>();
        text.GetComponent<Text>().text = "Click z to rest";
        text.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        GameObject c = GameObject.Find("Canvas");
        text.transform.SetParent(c.transform);
        text.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -75, 0);
        text.SetActive(false);
    }

    void Update()
    {
        if (players.Count == PhotonNetwork.PlayerList.Length)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                text.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    gameObject.GetComponent<PhotonView>().RPC("Sleep", RpcTarget.All);
                }
            }

        }
        else
        {
            text.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            players.Remove(other.gameObject);
        }
    }

    [PunRPC]
    private void Sleep()
    {
        GameObject.Find("DayCycle").GetComponent<Day>().day += 0.5f;
        int i = 0;
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerHealth>().MaxHealth();
            player.transform.position = sleepPoints[i].transform.position;
            i++;
        }
    }
}
