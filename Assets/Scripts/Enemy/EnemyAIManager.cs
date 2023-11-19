using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class EnemyAIManager : MonoBehaviour
{
    [SerializeField]
    int attackDamage;
    public List<Transform> targetPoints;

    public Transform _targetEnemy;

    public enum PositionState { State, Defance, Attack }

    public Transform hitPoint;

    public NavMeshAgent agent;

    public Animator animator;

    public Transform playerBase;

    public float health = 100;

    bool targetIsBase = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        agent = GetComponent<NavMeshAgent>();
    }


    public IEnumerator SetupSpawn(float time, Transform target)
    {
        yield return new WaitForSeconds(time);

        targetIsBase = true;

        GotoTarget(target);

    }


    public void GotoTarget(Transform point)
    {
        agent.SetDestination(point.position);

        agent.isStopped = false;

        animator.SetBool("isRun", true);

        StartCoroutine(CheckArriveRoutine());

    }

    public bool CheckArrive(float range)
    {
        if (agent.hasPath && agent.remainingDistance < range)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public IEnumerator CheckArriveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (CheckArrive(0.2f))
            {
                animator.SetBool("isRun", false);

                animator.SetBool("isAttack", true);

                agent.isStopped = true;

                break;

            }

            else if (targetIsBase && targetPoints.Count > 0)
            {

                FindClosestEnemy();

                break;

            }


        }

    }




    public void Attack()
    {
        if (_targetEnemy != null)
        {
            if (CheckArrive(0.2f))
            {
                _targetEnemy.GetComponent<IDamageable>().TakeDamage(attackDamage);
            }

            else
            {
                FindClosestEnemy();
            }


        }
    }


    void FindClosestEnemy()
    {


        foreach (Transform enemy in targetPoints)
        {
            if (Vector3.Distance(transform.position, enemy.position) < Vector3.Distance(transform.position, _targetEnemy.position))
            {
                _targetEnemy = enemy;
            }
        }

        if (Vector3.Distance(transform.position, playerBase.position) < Vector3.Distance(transform.position, _targetEnemy.position))
        {
            GotoTarget(playerBase);
            targetIsBase = true;
        }
        else
        {
            GotoTarget(_targetEnemy);
            targetIsBase = false;
        }


    }















}
