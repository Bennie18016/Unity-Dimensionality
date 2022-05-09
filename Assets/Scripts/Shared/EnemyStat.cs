using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace AI
{

    public class EnemyStat : MonoBehaviour
    {
        private int maxHealth = 100;
        public  int health;

        public PhotonView PV;

        void Start()
        {
            health = maxHealth;
            PV = GetComponent<PhotonView>();
        }

        void Update()
        {
            if (health <= 0){
                Death();
            }
        }

        private void Death()
        {
            PhotonNetwork.Destroy(gameObject);
        }

        [PunRPC]
        public void TakeDamage(int damage){
            health -= damage;
        }

    }
}