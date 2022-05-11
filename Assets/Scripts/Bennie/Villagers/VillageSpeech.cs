using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayerController;
using Photon.Pun;

namespace Villager
{
    public class VillageSpeech : MonoBehaviour
    {
        GameObject text;
        float reach;

        public GameObject speechBox;

        int speechStage;

        bool speaking;
        public List<String> speech = new List<string>();

        public string villagerName;

        void Start()
        {
            SetupText();
            reach = 3;
        }

        private void SetupText()
        {
            text = new GameObject();
            text.name = gameObject.name + " text";
            text.AddComponent<Text>();
            text.GetComponent<Text>().text = "Click F to speak";
            text.GetComponent<Text>().font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            GameObject c = GameObject.Find("Canvas");
            text.transform.SetParent(c.transform);
            text.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -75, 0);
            text.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {
            if (DistanceFromVillager(ClosestPlayer()) < reach && !speaking && ClosestPlayer().GetComponent<PhotonView>().IsMine)
            {
                text.SetActive(true);
                if (Input.GetKeyDown(KeyCode.F))
                {
                    SpeakToVillager();
                }
            }
            else
            {
                text.SetActive(false);
            }

            if (speaking && Input.GetKeyDown(KeyCode.Return))
            {
                Speak();
            }
        }

        private void Speak()
        {
            if ((speechStage + 1) == speech.Count)
            {
                speaking = false;
                speechStage = 0;
                speechBox.SetActive(false);

                ClosestPlayer().GetComponent<Movement>().enabled = true;
                ClosestPlayer().GetComponent<Inventory>().enabled = true;
                ClosestPlayer().GetComponent<Crafting>().enabled = true;
                ClosestPlayer().GetComponent<PauseGame>().enabled = true;

            }
            else
            {
                speechStage++;
                string speechToDisplay = speech[speechStage];
                speechBox.transform.GetChild(3).GetComponent<Text>().text = speechToDisplay;
            }
        }

        private void SpeakToVillager()
        {
            speechBox.transform.GetChild(2).GetComponent<Text>().text = villagerName;
            speechBox.SetActive(true);

            string speechToDisplay = speech[speechStage];
            speechBox.transform.GetChild(3).GetComponent<Text>().text = speechToDisplay;
            speaking = true;

            ClosestPlayer().GetComponent<Movement>().enabled = false;
            ClosestPlayer().GetComponent<Inventory>().enabled = false;
            ClosestPlayer().GetComponent<Crafting>().enabled = false;
            ClosestPlayer().GetComponent<PauseGame>().enabled = false;
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

        private float DistanceFromVillager(GameObject player)
        {
            return Vector3.Distance(player.transform.position, transform.position);
        }
    }
}
