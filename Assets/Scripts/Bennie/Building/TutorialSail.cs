using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class TutorialSail : MonoBehaviour
{
    List<GameObject> players = new List<GameObject>();

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
        text.GetComponent<Text>().text = "Click f to sail";
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
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Sail();
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

    private void Sail()
    {
        PhotonNetwork.LoadLevel(4);
    }
}
