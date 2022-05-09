using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using PlayerController;
using Photon.Pun;

namespace Abilities
{
    public class FourTimeManipulation : MonoBehaviour
    {
        bool active;

        GameObject[] Enemies;

        GameObject[] Players;

        float lastTimeManipulate;

        float coolTimeManipulate;

        float enemySpeed;
        float playerSpeed;
        float playerSprint;

        PhotonView PV;
        void Start()
        {
            lastTimeManipulate = 30;
            coolTimeManipulate = 30;

            enemySpeed = 0.5f;
            playerSpeed = GetComponent<Movement>().speed;
            playerSprint = GetComponent<Movement>().sprint;
            PV = GetComponent<PhotonView>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) && lastTimeManipulate > coolTimeManipulate)
            {

                if (PV.IsMine)
                {
                    active = true;
                    PV.RPC("GetValues", RpcTarget.AllBuffered);
                    PV.RPC("SpeedPlayers", RpcTarget.AllBuffered);
                    PV.RPC("SlowEnemies", RpcTarget.AllBuffered);
                    lastTimeManipulate = 0;
                }
            }

            if (active && lastTimeManipulate > 15)
            {
                ReturnNormal();
            }

            if (active)
            {
                PV.RPC("GetValues", RpcTarget.AllBuffered);
                PV.RPC("SlowEnemies", RpcTarget.AllBuffered);
            }
            lastTimeManipulate += 1 * Time.deltaTime;
        }

        [PunRPC]
        private void ReturnNormal()
        {
            foreach (GameObject Player in Players)
            {
                Movement move = Player.GetComponent<Movement>();
                move.speed = playerSpeed;
                move.sprint = playerSprint;
            }
            foreach (GameObject Enemy in Enemies)
            {
                Enemy.GetComponent<NavMeshAgent>().speed = enemySpeed;
            }
            active = false;
        }

        [PunRPC]
        private void GetValues()
        {
            Enemies = GameObject.FindGameObjectsWithTag("Enemy");
            Players = GameObject.FindGameObjectsWithTag("Player");
        }

        [PunRPC]
        private void SpeedPlayers()
        {
            foreach (GameObject Player in Players)
            {
                Movement move = Player.GetComponent<Movement>();
                float speedINC = move.speed / 2;
                float sprintINC = move.sprint / 2;
                move.speed += speedINC;
                move.sprint += sprintINC;
            }
        }

        [PunRPC]
        private void SlowEnemies()
        {
            foreach (GameObject Enemy in Enemies)
            {
                NavMeshAgent nav = Enemy.GetComponent<NavMeshAgent>();
                nav.speed = enemySpeed / 2;
            }
        }
    }
}
