using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIMovement : MonoBehaviour
{
    public float moveSpeed = 2;
    public float runSpeed = 4;
    //public Vector3 startPosition;
    public WaypointGroup waypoints;

    public int CurrentWaypoint { get; set; }

    //private Rigidbody _rigidbody;
    //public Rigidbody Body { get => _rigidbody; }
    public Animator animator;
    private NavMeshAgent _agent;
    public NavMeshAgent Agent { get => _agent; }
    private BehaviourTree<AIMovement> _behaviourTree;

    // Start is called before the first frame update
    void Start()
    {
        //_rigidbody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();

        _behaviourTree = new(new Selector<AIMovement>()
        {
            nodes = new()
            {
                new Sequence<AIMovement>()
                {
                    nodes = new()
                    {
                        new AnnouncementDecisionNode(),
                        new VisualDetectionNode(),
                        new MovementNode()
                        {
                            patrolSpeed = runSpeed,
                        },
                        new CaughtNode()
                    }
                },
                new Sequence<AIMovement>()
                {
                    nodes = new()
                    {
                        new Successor<AIMovement>()
                        {
                            node = new MovementNode()
                            {
                                patrolSpeed = moveSpeed,
                            },
                        },
                        new FindWaypointNode(),                        
                    }
                }
            }
        });

        _agent.enabled = true;
        _agent.Warp(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        _behaviourTree.Update(this);
    }
}
