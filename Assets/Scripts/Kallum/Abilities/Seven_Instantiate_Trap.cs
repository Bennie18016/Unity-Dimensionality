using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Seven_Instantiate_Trap : MonoBehaviour
{
    public GameObject trap;
    public GameObject projectile;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            GameObject projectileObject = PhotonNetwork.Instantiate("Trap",new Vector3 (projectile.transform.position.x, projectile.transform.position.y, projectile.transform.position.z), Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
