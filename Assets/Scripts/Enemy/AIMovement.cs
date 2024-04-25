using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(GridObject))]
public class AIMovement : MonoBehaviour
{
    public float moveSpeed = 2;
    public float runSpeed = 4;

    public float fov = 160f;
    public float detectionRange = 20f;
    public float detectionTime = 1f;
    //public Vector3 startPosition;
    public WaypointGroup waypoints;

    [SerializeField]
    private Transform _visionLocation;
    public Transform VisionLocation { get => _visionLocation; }
    public AudioHear AudioHear { get; private set; }
    public int CurrentWaypoint { get; set; }

    [SerializeField]
    private LayerMask _losMask;
    [SerializeField]
    private LayerMask _detectionMask;

    public LayerMask LOSMask => _losMask;
    public LayerMask DetectionMask => _detectionMask;

    //private Rigidbody _rigidbody;
    //public Rigidbody Body { get => _rigidbody; }
    public Animator animator;
    private NavMeshAgent _agent;
    public NavMeshAgent Agent { get => _agent; }
    private BehaviourTree<AIMovement> _behaviourTree;
    public GridObject GridObject { get; private set; }

    public Transform chasedPlayer;

    public Grid currentGrid;

    public List<uint> GridsPlayerCaught { get; private set; } = new();

    public List<AK.Wwise.Event> spottedPlayerSounds;
    public List<AK.Wwise.Event> chasingPlayerSounds;

    // Start is called before the first frame update
    void Start()
    {
        //_rigidbody = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        AudioHear = GetComponent<AudioHear>();
        GridObject = GetComponent<GridObject>();

        _behaviourTree = new(new Selector<AIMovement>()
        {
            nodes = new()
            {
                new Sequence<AIMovement>()
                {
                    nodes = new()
                    {
                        new AnnouncementDecisionNode(),

                        new VisualDetectionNode()
                        {
                            fov = fov,
                            detectionRange = detectionRange,
                            detectionTime = detectionTime,
                        },
                        new MovementNode()
                        {
                            patrolSpeed = runSpeed,
                            minDistance = 1f
                        },
                        new CaughtNode()
                    }
                },
                new Sequence<AIMovement>()
                {
                    nodes = new()
                    {
                        new AnnouncementDecisionNode(),
                        new AudioDetectionNode()
                        {  },
                        new MovementNode()
                        {
                            patrolSpeed = moveSpeed,
                        }
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
                        new SearchGridNode(),                        
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
        if(GameManager.Instance.IsPaused)
        {
            Agent.enabled = false;
            return;
        }
        Agent.enabled = true;

        _behaviourTree.Update(this);
    }
}
