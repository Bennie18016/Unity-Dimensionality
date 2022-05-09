using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Abilities;

namespace AI
{

    public class EnemyPattern : MonoBehaviour
    {
        private NavMeshAgent enemy;
        public Transform go;
        private bool stockholm;
        float chaseDistance = 10f;

        float shadowChaseDistance = 2f;
        Vector3 position;

        void Start()
        {
            enemy = GetComponent<NavMeshAgent>();
            position = transform.position;
        }

        void Update()
        {
            //if (stockholm)
            //{
            //    go = ClosestEnemy().transform;
            //    enemy.SetDestination(go.position);
            //} else { 
            //    go = ClosestPlayer().transform;
            //    enemy.SetDestination(go.position);
            //}

            if (stockholm && ChaseEnemy())
            {
                go = ClosestEnemy().transform;
                MoveEnemy(go.transform.position);
                return;
            }
            else if (stockholm && !ChaseEnemy())
            {
                go = ClosestPlayer().transform;
                MoveEnemy(go.transform.position);
                return;
            }

            if (ChasePlayer())
            {
                go = ClosestPlayer().transform;
                MoveEnemy(go.transform.position);
            }
            else if (GetComponent<EnemyPatrol>() != null && !ChasePlayer())
            {
                return;
            }
            else
            {
                MoveEnemy(position);
            }
        }

        public void MoveEnemy(Vector3 move)
        {
            enemy.SetDestination(move);
        }

        private GameObject ClosestPlayer()
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

            GameObject closest = null;
            float distance = Mathf.Infinity;
            Vector3 enemyPos = transform.position;

            foreach (GameObject target in targets)
            {
                Vector3 diff = target.transform.position - enemyPos;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = target;
                    distance = curDistance;
                }
            }
            return closest;
        }

        private GameObject ClosestEnemy()
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");

            GameObject closest = null;
            float distance = Mathf.Infinity;
            Vector3 enemyPos = transform.position;

            foreach (GameObject target in targets)
            {
                if (target == gameObject) continue;
                Vector3 diff = target.transform.position - enemyPos;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance)
                {
                    closest = target;
                    distance = curDistance;
                }
            }
            return closest;
        }

        public bool ChasePlayer()
        {

            if (stockholm)
            {
                return false;
            }
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject target in targets)
            {
                if (target.GetComponent<TwoShadowForm>() != null && target.GetComponent<TwoShadowForm>().shadow)
                {
                    if (DistanceFromTarget(target) < shadowChaseDistance && target.GetComponent<TwoShadowForm>().shadow)
                    {
                        return true;
                    } else if(!target.GetComponent<TwoShadowForm>().shadow && DistanceFromTarget(target) < chaseDistance){
                        return true;
                    }
                }
                else if (DistanceFromTarget(target) < chaseDistance)
                {
                    return true;
                }
            }
            return false;
        }

        public bool ChaseEnemy()
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject target in targets)
            {
                if (DistanceFromTarget(target) < chaseDistance)
                {
                    if (target == gameObject) continue;
                    return true;
                }
            }
            return false;
        }

        private float DistanceFromTarget(GameObject target)
        {
            return Vector3.Distance(target.transform.position, transform.position);
        }

        public void Stockholm()
        {
            stockholm = true;
        }
    }

}