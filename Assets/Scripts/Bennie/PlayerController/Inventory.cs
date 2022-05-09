using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

namespace PlayerController
{
    public class Inventory : MonoBehaviour
    {
        public int wood;
        public int rock;
        public int rope;
        public int gold;
        GameObject inventory;
        public PhotonView PV;
        public bool invActive;
        public CharacterController cc;


        GameObject woodButton;
        GameObject ropeButton;
        GameObject rockButton;
        GameObject coinsButton;


        private void Start()
        {
            var invPrefab = Resources.Load("UI/inventoryGameObject");
            PV = GetComponent<PhotonView>();
            inventory = (GameObject)Instantiate(invPrefab, Vector3.zero, Quaternion.identity);
            GameObject canvas = GameObject.Find("Canvas");
            inventory.transform.SetParent(canvas.transform);
            cc = GetComponent<CharacterController>();

            SetupInventory();
        }

        private void SetupInventory()
        {
            inventory.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            inventory.SetActive(false);
            woodButton = inventory.transform.GetChild(2).gameObject;
            ropeButton = inventory.transform.GetChild(3).gameObject;
            rockButton = inventory.transform.GetChild(4).gameObject;
            coinsButton = inventory.transform.GetChild(5).gameObject;

            woodButton.GetComponentInChildren<Text>().text = ("Wood - " + wood);
            ropeButton.GetComponentInChildren<Text>().text = ("Rope - " + rope);
            rockButton.GetComponentInChildren<Text>().text = ("Metal - " + rock);
            coinsButton.GetComponentInChildren<Text>().text = ("Coins - " + gold);
        }

        private void Update()
        {
            OpenCloseInventory();
            UpdateInventory();
        }

        private void DisableEnableInventory()
        {
            if (invActive && PV.IsMine)
            {
                GetComponent<Movement>().enabled = false;
                GetComponent<PauseGame>().enabled = false;
                GetComponent<Crafting>().enabled = false;
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;


            }
            else if (!invActive && PV.IsMine)
            {
                GetComponent<Movement>().enabled = true;
                GetComponent<PauseGame>().enabled = true;
                GetComponent<Crafting>().enabled = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void OpenCloseInventory()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !invActive && cc.isGrounded)
            {
                inventory.SetActive(true);
                invActive = true;
                DisableEnableInventory();
            }
            else if (Input.GetKeyDown(KeyCode.Q) && invActive)
            {
                inventory.SetActive(false);
                invActive = false;
                DisableEnableInventory();
            }
        }
        private void UpdateInventory()
        {
            woodButton.GetComponentInChildren<Text>().text = ("Wood - " + wood);
            ropeButton.GetComponentInChildren<Text>().text = ("Rope - " + rope);
            rockButton.GetComponentInChildren<Text>().text = ("Rocks - " + rock);
            coinsButton.GetComponentInChildren<Text>().text = ("Coins - " + gold);
        }
    }
}