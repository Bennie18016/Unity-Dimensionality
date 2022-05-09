using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class Compass : MonoBehaviourPun
{
    RawImage compass;
    Transform playerTransform;

    PhotonView PV;
    GameObject compasObj;

    void Start()
    {
        var compassPrefab = Resources.Load("UI/compassGameObject");
        compasObj = (GameObject)Instantiate(compassPrefab, Vector3.zero, Quaternion.identity);
        GameObject c = GameObject.Find("Canvas");
        compasObj.transform.SetParent(c.transform);
        compasObj.GetComponent<RectTransform>().anchoredPosition = new Vector4(0, -35, 0);

        compass = compasObj.GetComponent<RawImage>();
        PV = gameObject.GetComponent<PhotonView>();

        if (!PV.IsMine)
        {
            compasObj.SetActive(false);
        }

    }

    void Update()
    {
        playerTransform = gameObject.transform;
        compass.uvRect = new Rect(playerTransform.localEulerAngles.y / 360f, 0, 1, 1);
    }
}
