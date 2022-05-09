using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using PlayerController;
using Photon.Pun;

public class Odd_Number_Trap : MonoBehaviour
{
    // Start is called before the first frame update
    int numberOfEnemies;

    float activateCool;

    float lastActivate;


    float placed;

    float removetime;
    public List<GameObject> enemies = new List<GameObject>();

    PhotonView PV;

    void Start()
    {
        activateCool = 7;
        lastActivate = 7;

        placed = 0;
        removetime = 101;

        PV = GetComponent<PhotonView>();
    }

    private void RemoveTrap(){
        GameObject.Find("PhotonPlayer7D(Clone)").GetComponent<Seven_Projectile>().traps--;
        PhotonNetwork.Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.tag == "Enemy")
        {
            enemies.Add(col.gameObject);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.transform.tag == "Enemy")
        {
            enemies.Remove(col.gameObject);
        }
    }

    void Update()
    {
        if (numberOfEnemies % 2 == 1 && lastActivate > activateCool)
        {
            PV.RPC("TrapActivate", RpcTarget.AllBuffered);
        }

        if(placed > removetime){
            RemoveTrap();
        }

        lastActivate += 1 * Time.deltaTime;
        placed += 1 * Time.deltaTime;
        numberOfEnemies = enemies.Count;

    }

    [PunRPC]
    public void TrapActivate()
    {
        foreach (GameObject enemy in enemies)
        {
            EnemyStat enemyScript = enemy.GetComponent<EnemyStat>();
            enemyScript.health -= 10;

            if (enemyScript.health <= 0)
            {
                enemies.Remove(enemy);
            }
        }
        lastActivate = 0;
    }


}
