using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;
using Object;
using Photon.Pun;

namespace PlayerController
{
    public class MeleeCombat : MonoBehaviour
    {

        Camera myCam;
        GameObject target;

        PhotonView PV;

        float lastAttack;
        private float weaponRange;
        private int weaponDamage;

        public Animator a;

        void Start()
        {
            myCam = GetComponentInChildren<Camera>();
            weaponDamage = 20;
            weaponRange = 5;
            PV = GetComponent<PhotonView>();

            lastAttack = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0) && PV.IsMine)
            {
                if (lastAttack > a.GetCurrentAnimatorStateInfo(0).length)
                {
                    Punch();
                }
            }

            lastAttack += Time.deltaTime;
        }

        private void Punch()
        {
            a.SetTrigger("Stab");
            RaycastHit hit;

            lastAttack = 0;

            if (Physics.Raycast(GetMouseRay(), out hit, Mathf.Infinity))
            {
                if (hit.collider.tag == "Enemy")
                {
                    target = hit.collider.gameObject;
                    if (InRadius())
                    {
                        target.GetComponent<EnemyStat>().PV.RPC("TakeDamage", RpcTarget.AllBuffered, weaponDamage);
                    }
                }
                if (hit.collider.transform.parent != null && hit.collider.transform.parent.tag == "Tree")
                {
                    target = hit.collider.transform.parent.gameObject;
                    if (InRadius())
                    {
                        target.GetComponent<TreeStat>().PV.RPC("TakeDamage", RpcTarget.AllBuffered, weaponDamage);
                        gameObject.GetComponent<Inventory>().wood++;
                    }

                }
            }

        }

        private bool InRadius()
        {
            if (DistanceFromPlayer(target) < weaponRange)
            {
                return true;
            }
            return false;
        }

        private float DistanceFromPlayer(GameObject target)
        {
            return Vector3.Distance(target.transform.position, transform.position);
        }

        private Ray GetMouseRay()
        {
            return myCam.ScreenPointToRay(Input.mousePosition);
        }
    }
}
