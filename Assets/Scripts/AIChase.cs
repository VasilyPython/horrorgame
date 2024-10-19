using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChase : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;               // Nav mesh agent component
    public Animator animator;                        // Animator component
    public float startWaitTime = 4;                 // Wait time of every action
    public float timeToRotate = 2;                  // Wait time when the enemy detects near the player without seeing
    public float speedWalk = 6;                     // Walking speed
    public float speedRun = 9;                      // Running speed

    public float viewRadius = 15;                   // Radius of the enemy view
    public float viewAngle = 90;                    // Angle of the enemy view
    public LayerMask playerMask;                    // To detect the player with the raycast
    public LayerMask obstacleMask;                  // To detect the obstacles with the raycast

    public Transform[] waypoints;                   // All the waypoints where the enemy patrols
    int m_CurrentWaypointIndex;                     // Current waypoint index

    Vector3 playerLastPosition = Vector3.zero;      // Last position of the player when near the enemy
    Vector3 m_PlayerPosition;                       // Last position of the player when seen

    float m_WaitTime;                               // Variable for wait time
    float m_TimeToRotate;                           // Variable for rotate wait time
    bool m_playerInRange;                           // If the player is in range of vision
    bool m_PlayerNear;                              // If the player is near, state of hearing
    bool m_IsPatrol;                                // If the enemy is patrolling
    bool m_CaughtPlayer;                            // If the enemy has caught the player

    void Start()
    {
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_playerInRange = false;
        m_PlayerNear = false;
        m_WaitTime = startWaitTime;
        m_TimeToRotate = timeToRotate;

        m_CurrentWaypointIndex = 0;                 
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();        

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;             
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    
    }

    private void Update()
    {
        EnviromentView();                           

        // Check if the enemy should chase or patrol
        if (!m_IsPatrol)
        {
            Chasing();
        }
        else
        {
            Patroling();
        }

        UpdateSpeedBasedOnAnimation();
    }

    private void UpdateSpeedBasedOnAnimation()
    {
        bool isRunning = animator.GetBool("isRunning");
        navMeshAgent.speed = isRunning ? speedRun : speedWalk;
    }

    private void Chasing()
    {
        m_PlayerNear = false;                       

        if (!m_CaughtPlayer)
        {
            Move(navMeshAgent.speed);                
            navMeshAgent.SetDestination(m_PlayerPosition);
            animator.SetBool("isRunning", true);   
        }
        else
        {
            animator.SetBool("isRunning", false);  
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)    
        {
            if (m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                animator.SetBool("isRunning", false); 
                m_TimeToRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    private void Patroling()
    {
        if (m_PlayerNear)
        {
            if (m_TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                m_TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;           
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);    
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime;
                }
            }
        }
    }

    private void OnAnimatorMove() {}

    public void NextPoint()
    {
        m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
        animator.SetBool("isRunning", false);
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeToRotate = timeToRotate;
                animator.SetBool("isRunning", false);
            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        // Check for colliders within the view radius defined by playerMask
        Collider[] playersInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        // Chase any player found in the playerMask layer
        foreach (Collider player in playersInRange)
        {
            Transform playerTransform = player.transform;
            Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;

            // Check the angle to the player
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, playerTransform.position);

                // Ensure there are no obstacles in the way
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    m_playerInRange = true;
                    m_IsPatrol = false;
                    m_PlayerPosition = playerTransform.position;
                    return; // Exit early if we've found a valid player
                }
            }
        }

        // Also check for any GameObject with the "Player" tag
        GameObject taggedPlayer = GameObject.FindGameObjectWithTag("Player");
        if (taggedPlayer != null)
        {
            Vector3 dirToTaggedPlayer = (taggedPlayer.transform.position - transform.position).normalized;
            float dstToTaggedPlayer = Vector3.Distance(transform.position, taggedPlayer.transform.position);

            // Check angle and raycast for the tagged player
            if (Vector3.Angle(transform.forward, dirToTaggedPlayer) < viewAngle / 2 && dstToTaggedPlayer <= viewRadius)
            {
                if (!Physics.Raycast(transform.position, dirToTaggedPlayer, dstToTaggedPlayer, obstacleMask))
                {
                    m_playerInRange = true;
                    m_IsPatrol = false;
                    m_PlayerPosition = taggedPlayer.transform.position;
                }
            }
        }
    }
}
