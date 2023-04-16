using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemySO enemySO;

    private EnemySO enemy;

    private NavMeshAgent _agent;

    private GameObject _targetToFollow;

    private void Awake()
    {
        _targetToFollow = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
        enemy = enemySO.ShallowCopy();


    }

    void Start()
    {
       

        
    }

    // Update is called once per frame
    void Update()
    {        
        if (_targetToFollow)
        {
            _agent.SetDestination(_targetToFollow.transform.position);
        }
    }


    void FollowTarget(GameObject target)
    {
        _targetToFollow = target;
    }

    
}

