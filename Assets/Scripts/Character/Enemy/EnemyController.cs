using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : MonoBehaviour {

    public float lookRadius = 10f;
    public float faceTargetRotationSpeed = 5f;

    Transform target;
    NavMeshAgent agent;

    Vector3 spawnPosition;
    bool gobackPosition;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPosition = transform.position;
        gobackPosition = false;
    }

    void Update()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(target.position, transform.position);
            if (distance <= lookRadius)
            {
                agent.SetDestination(target.position);

                if (distance <= agent.stoppingDistance)
                {
                    FaceTarget();
                    // Attack the target
                }
            }
            else
            {
                StopFollowingTarget();
            }
        }
        else
        {
            if (spawnPosition != transform.position)
            {
                // Go back original position
                GobackPosition();
            }
            else
            {
                // Walk around specific area
                MoveWalkAround();
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * faceTargetRotationSpeed);
    }

    void MoveWalkAround()
    {
        if (gobackPosition)
        {
            gobackPosition = false;
        }
        else
        {

        }
    }

    void GobackPosition()
    {
        agent.SetDestination(spawnPosition);

        agent.updateRotation = false;
        agent.stoppingDistance = 0f;

        gobackPosition = true;
    }

    public void FollowTarget(Transform newTarget)
    {        
        if (target != null)
        {
            // Decide change player or not
            return;
        }

        target = newTarget;

        agent.stoppingDistance = lookRadius * .8f;
        agent.updateRotation = false;
    }

    void StopFollowingTarget()
    {
        target = null;

        agent.stoppingDistance = 0f;
        agent.updateRotation = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
