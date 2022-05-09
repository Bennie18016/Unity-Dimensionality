using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using PlayerController;

public class PauseGame : MonoBehaviour
{
    Button back;
    Button disconnect;
    GameObject pause;
    public bool paused;
    CharacterController cc;
    void Start()
    {
        var pausePrefab = Resources.Load("UI/pauseGameObject");

        pause = (GameObject)Instantiate(pausePrefab, Vector3.zero, Quaternion.identity);

        back = pause.transform.GetChild(2).GetComponent<Button>();
        disconnect = pause.transform.GetChild(3).GetComponent<Button>();

        GameObject canvas = GameObject.Find("Canvas");
        pause.transform.SetParent(canvas.transform);

        cc = GetComponent<CharacterController>();

        SetupPause();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenClosePause();
        }
    }

    private void DisableEnablePause()
    {
        PhotonView PV = GetComponent<PhotonView>();
        if (paused && PV.IsMine)
        {
            GetComponent<Movement>().enabled = false;
            GetComponent<Inventory>().enabled = false;
            GetComponent<Crafting>().enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;


        }
        else if (!paused && PV.IsMine)
        {
            GetComponent<Movement>().enabled = true;
            GetComponent<Inventory>().enabled = true;
            GetComponent<Crafting>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OpenClosePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !paused && cc.isGrounded)
        {
            pause.SetActive(true);
            paused = true;
            DisableEnablePause();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            pause.SetActive(false);
            paused = false;
            DisableEnablePause();
        }
    }

    private void SetupPause()
    {
        pause.SetActive(false);

        back.onClick.AddListener(() => ReturnToGame());
        disconnect.onClick.AddListener(() => QuitGame());

        pause.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    private void QuitGame()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
    }

    private void ReturnToGame()
    {
        pause.SetActive(false);
        paused = false;
        DisableEnablePause();
    }
}
