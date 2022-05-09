using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Seven_Projectile : MonoBehaviour
{
    public GameObject Projectile;
    public GameObject FirePoint;
    public float throwForce = -30;

    PhotonView PV;

    public int traps;

    int maxTraps;


    void Start()
    {
        maxTraps = 3;
        PV = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && PV.IsMine && traps < maxTraps)
        {
            PV.RPC("Shoot", RpcTarget.All);
            traps++;
        }
    }

    [PunRPC]
    private void Shoot()
    {
        GameObject projectileObject = PhotonNetwork.Instantiate("Projectile", FirePoint.transform.position, Quaternion.identity);
        Rigidbody projectileRigidbody = projectileObject.GetComponent<Rigidbody>();
        projectileRigidbody.AddRelativeForce(FirePoint.transform.forward * throwForce, ForceMode.Impulse);
    }
}
