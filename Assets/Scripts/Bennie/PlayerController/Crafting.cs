using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;

namespace PlayerController
{
    public class Crafting : MonoBehaviour
    {

        GameObject crafting;

        GameObject craftRopeButton;

        Inventory i;

        public bool craftActive;
        private void Start()
        {
            i = GetComponent<Inventory>();
            var craftPrefab = Resources.Load("UI/craftingGameObject");
            crafting = (GameObject)Instantiate(craftPrefab, Vector3.zero, Quaternion.identity);
            GameObject c = GameObject.Find("Canvas");
            crafting.transform.SetParent(c.transform);

            SetupCrafting();
        }

        private void Update()
        {
            OpenCloseCrafting();
        }

        private void DisableEnableCrafting()
        {
            PhotonView PV = GetComponent<PhotonView>();
            if (craftActive && PV.IsMine)
            {
                GetComponent<Movement>().enabled = false;
                GetComponent<Inventory>().enabled = false;
                GetComponent<PauseGame>().enabled = false;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;


            }
            else if (!craftActive && PV.IsMine)
            {
                GetComponent<Movement>().enabled = true;
                GetComponent<Inventory>().enabled = true;
                GetComponent<PauseGame>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void OpenCloseCrafting()
        {
            if (Input.GetKeyDown(KeyCode.C) && !craftActive && i.cc.isGrounded)
            {
                crafting.SetActive(true);
                craftActive = true;
                DisableEnableCrafting();
            }
            else if (Input.GetKeyDown(KeyCode.C) && craftActive)
            {
                crafting.SetActive(false);
                craftActive = false;
                DisableEnableCrafting();
            }
        }

        private void SetupCrafting()
        {
            crafting.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            crafting.SetActive(false);

            craftRopeButton = crafting.transform.GetChild(2).gameObject;
            craftRopeButton.GetComponentInChildren<Text>().text = ("Wood + Wood");
            craftRopeButton.GetComponent<Button>().onClick.AddListener(() => CraftRope());
        }

        public void CraftRope()
        {
            if (i.wood >= 2)
            {
                i.wood -= 2;
                i.rope++;
            }
        }
    }
}