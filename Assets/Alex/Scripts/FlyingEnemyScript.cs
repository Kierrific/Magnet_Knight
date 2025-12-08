using Pathfinding;
using UnityEngine;


public class FlyingEnemyScript : MonoBehaviour
{
    public enum States
    {
        Roaming = 0,
        Chasing = 1,
        Attacking = 2
    }

    public enum PathFindingTargets
    {
        Random = 0,
        Player = 1,
        Retreat = 2,
        Chase = 3,
    }

    public enum AgroStates
    {
        Far = 0, 
        Close = 1, 
        Middle = 2, 
    }

    [Tooltip("The instance of the StatsScript attached to the player (Should define its self in script but you should still set it in inspector)")][SerializeField] private StatsScript _stats;
    [Tooltip("The distance in which the enemy can detect the player")][SerializeField] private float _detectRange;
    [Tooltip("Set this to the layer that the player is on")][SerializeField] private LayerMask _playerLayer;
    [Tooltip("Set this to the layer that the environment is on")][SerializeField] private LayerMask _worldLayer;
    [Tooltip("Set this to the layer of the player and the environment is on.")][SerializeField] private LayerMask _combinedLayers;
    [Tooltip("The size of the BoxCast that checks if the enemy will run into a wall")][SerializeField] private Vector2 _wallCastSize;
    [Tooltip("How often the enemy will look for the player")][SerializeField] private float _detectCooldown;
    [Tooltip("After how long of not seeing the player will the enemy return to a roaming state.")][SerializeField] private float _playerUndetected = 3f; //Change name later (E)
    [Tooltip("How often the enemy can attack in seconds.")][SerializeField] private float _attackCooldown;
    [Tooltip("How close an enemy needs to be during pathfinding to a waypoint to move to the next waypoint.")][SerializeField] private float _nextWaypointDistance = 0.5f;
    [Tooltip("How far the enemy will roam.")][SerializeField] private float _roamDistance;
    [Tooltip("The distance at which AI behavior starts, the lower the number the greater performance impact.")][SerializeField] private float _maxDistance = 30f;

    //Pathfinding Variables
    //-------------------------------------
    Path path;
    private int currentWaypoint = 0;
    private bool _pathEnded = false;
    [Tooltip("Set this to the seeker script attached to the enemy.")][SerializeField] private Seeker _seeker;
    //-------------------------------------

    private float _detectTimer;
    private Vector3 _direction = Vector2.down;
    private float _timeSinceSeen = 0f;
    private float _attackTimer;
    private Rigidbody2D _enemyRB2D;
    private SpriteRenderer _enemyRenderer;
    private States _currentState = States.Roaming;
    private Vector2 _wallCenterPosition; //The center position of the boxcast that checks if the enemy is going to run into a wall
    private GameObject _player;
    private float _attackDuration = 1f; //How long each attack last
    private bool _attacking = false; //Whether or not the enemey is currently attacking
    //private bool _attacked = false; //Whether or not the enemy has attacked this attack pattern
    private bool _renameMeLater = false; //Whether or not the retreat path finding has been set yet or not
    private Vector3 lastSeenPos; //The last position the enemy saw the player at
    private float agroSwapTimer; //The actual variable keeping track of the agroSwapDuration
    private float agroSwapDuration = 0.5f; //The minimum amount of time for the enemy to swap from one state to another
    private AgroStates currentAgroState; //The current agro state the enemy is in (based on distance)
    private float _circleDir = 1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
