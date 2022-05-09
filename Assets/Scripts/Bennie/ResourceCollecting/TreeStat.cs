using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using PlayerController;

namespace Object{
    public class TreeStat : MonoBehaviour
{
        private int maxDurability = 100;
        public  int durability;
        public PhotonView PV;

        void Start()
        {
            durability = maxDurability;
            PV = GetComponent<PhotonView>();
        }

        void Update()
        {
            if(durability <= 0){
                Death();
            }
        }

        private void Death()
        {
            PhotonNetwork.Destroy(gameObject);
        }

        [PunRPC]
        public void TakeDamage(int damage){
            durability -= damage;
        }
    }

}