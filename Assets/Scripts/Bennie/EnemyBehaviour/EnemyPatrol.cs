using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class EnemyPatrol : MonoBehaviour
    {
        float waypointTolerance = 1.1f;
        [SerializeField] DrawWaypoints patrolPath;
        EnemyPattern mover;
        int currentWaypointIndex = 0;

        private void Start()
        {
            mover = GetComponent<EnemyPattern>();
        }

        private void Update()
        {
            PatrolBehaviour();
        }


        private void PatrolBehaviour()
        {
            if (AtWaypoint())
            {
                CycleWaypoint();
            }
            Vector3 nextPosition = GetCurrentWaypoint();
            if (!mover.ChasePlayer())
            {
                mover.MoveEnemy(nextPosition);
            }

        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }


    }
}

