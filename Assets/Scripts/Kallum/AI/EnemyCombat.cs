using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class EnemyCombat : EnemyPattern
{

    float attackDistance = 5f;
    public float dist;
    GameObject plyrObj;

    GameObject stockOBJ;
    GameObject enemyObj;
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
        if (gameObject.GetComponent<EnemyPattern>().stockholm)
        {
            StockholmAttack();
        }
        else
        {
            AttackPlayer();
            AttackStock();
        }


        lastAttack += 1 * Time.deltaTime;
    }

    private void StockholmAttack()
    {
        if (lastAttack > attackCool)
        {
            enemyObj = ClosestEnemy();

            if (DistanceFromTarget(enemyObj) < attackDistance)
            {
                enemyObj.GetComponent<EnemyStat>().TakeDamage(damage);
                lastAttack = 0;
            }
        }
    }

    private void AttackPlayer()
    {
        if (lastAttack > attackCool)
        {
            plyrObj = ClosestPlayer();

            if (DistanceFromTarget(plyrObj) < attackDistance)
            {
                plyrObj.GetComponent<PlayerHealth>().TakeDamage(damage);
                lastAttack = 0;
            }
        }
    }

    private void AttackStock()
    {
        if (lastAttack > attackCool)
        {
            stockOBJ = ClosestEnemy();

            if (DistanceFromTarget(stockOBJ) < attackDistance && stockOBJ.GetComponent<EnemyPattern>().stockholm)
            {
                stockOBJ.GetComponent<EnemyStat>().TakeDamage(damage);
                lastAttack = 0;
            }
        }
    }

    private float DistanceFromTarget(GameObject target)
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

}
