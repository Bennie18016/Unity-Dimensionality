using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class EnemyCombat : EnemyPattern
{

    float attackDistance = 5f;
    public float dist;
    GameObject plyrObj;

    float lastAttack;
    float attackCool;

    int damage;

    void Start()
    {
        attackCool = 5;
        lastAttack = 5;
        damage = 5;
    }

    void Update()
    {
        if (lastAttack > attackCool)
        {
            plyrObj = ClosestPlayer();

            if (DistanceFromPlayer(plyrObj) < attackDistance)
            {
                plyrObj.GetComponent<PlayerHealth>().TakeDamage(damage);
                lastAttack = 0;
            }
        }
        lastAttack += 1 * Time.deltaTime;
    }

    private bool InRadius()
    {
        if (DistanceFromPlayer(plyrObj) < attackDistance)
        {
            return true;
        }
        return false;
    }

    private float DistanceFromPlayer(GameObject target)
    {
        return Vector3.Distance(target.transform.position, transform.position);
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
}
