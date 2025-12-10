using Pathfinding;
using System.Runtime.CompilerServices;
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
        Position = 4,
    }

    public enum AgroStates
    {
        TopLeft = 0,
        TopRight = 1,
        BottomLeft = 2,
        BottomRight = 3,
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
    [Tooltip("How far in the x and y direction the flying enemy tries to stay offset.")][SerializeField] private float _targetOffset = 6f;
    [Tooltip("The default damage of the flying enemies attack.")] [SerializeField] private int _baseDamage;

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
    private float _attackDurationTimer;
    private float _attackDuration = 1f; //How long each attack last
    private bool _attacking = false; //Whether or not the enemey is currently attacking
    //private bool _attacked = false; //Whether or not the enemy has attacked this attack pattern
    private bool _renameMeLater = false; //Whether or not the retreat path finding has been set yet or not
    private Vector3 lastSeenPos; //The last position the enemy saw the player at
    private float agroSwapTimer; //The actual variable keeping track of the agroSwapDuration
    private float agroSwapDuration = 0.5f; //The minimum amount of time for the enemy to swap from one state to another
    private AgroStates currentAgroState; //The current agro state the enemy is in (based on distance)
    private Vector3 _targetLocation;
    private float _startMovespeed;
    private bool _targetReached = false;

    private void Awake()
    {
        _attackDurationTimer = _attackDuration;
        _enemyRB2D = GetComponent<Rigidbody2D>();
        _enemyRenderer = GetComponent<SpriteRenderer>();
        _detectTimer = _detectCooldown;
        _wallCenterPosition = (Vector2)transform.position + (Vector2)_direction;
        _player = GameObject.FindWithTag("Player");

        if (_player == null)
        {
            Debug.LogWarning("Melee Enemy Script cannot find the player object and will not work");
            Debug.Break();
        }


        if (TryGetComponent<Seeker>(out Seeker tempSeeker))
        {
            if (_seeker != tempSeeker)
            {
                _seeker = tempSeeker;
                Debug.LogWarning("The Seeker component on the enemy should be assigned to the Stats variable in the inspector");

            }
        }
        else
        {
            _seeker = gameObject.AddComponent<Seeker>();
            Debug.LogWarning("Enemy Game Object requires a Seeker script and will not function properly without it.");
            //Debug.Break;
        }

        //Verifies the stats script is set up properly
        if (TryGetComponent<StatsScript>(out StatsScript statsScript))
        {
            if (_stats != statsScript)
            {
                _stats = statsScript;
                Debug.LogWarning("The stats script component on the enemy should be assigned to the Stats variable in the inspector", _stats);
            }
        }
        else
        {
            _stats = gameObject.AddComponent<StatsScript>();
            Debug.LogWarning("Enemy Game Object requires a stats script and will not function properly without it.\nAsk me if you need help with that.", _stats);
        }
    }

    void Start()
    {
        _startMovespeed = _stats.MoveSpeed;
        _seeker.StartPath(transform.position, (Vector3)(Random.insideUnitCircle * _roamDistance) + transform.position, PathComplete);

    }

    void Update()
    {
        if (_stats.Health == 0)
        {
            Die();
        }

        Move();
        if (_currentState == States.Roaming)
        {
            if (_detectTimer > 0f)
            {
                _detectTimer -= Time.deltaTime;
            }
            else
            {
                DetectPlayer();
                _detectTimer = _detectCooldown;
            }
        }
        else if (_currentState == States.Chasing)
        {
            _attackTimer -= Time.deltaTime; 
            if (_attackTimer < 0 && !_attacking && _targetReached)
            {
                _stats.MoveSpeed = 40f;
                Vector3 dir2Player = _player.transform.position - transform.position;
                _direction = dir2Player.normalized;
                _currentState = States.Attacking;
                _attacking = true;
                _attackTimer = 9999999f;
            }
        }
    }


    private void PathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void UpdatePath(PathFindingTargets pft) //Creates a new path depending on specified situation. 
    {
        if (_seeker.IsDone())
        {
            switch (pft)
            {
                case PathFindingTargets.Player:
                    _seeker.StartPath(transform.position, _player.transform.position, PathComplete);
                    break;

                case PathFindingTargets.Random:
                    _seeker.StartPath(transform.position, (Vector3)(Random.insideUnitCircle * _roamDistance) + transform.position, PathComplete);
                    break;

                case PathFindingTargets.Retreat:
                    Vector3 advoidedDir = (_player.transform.position - transform.position).normalized; //Gets the direction from the enemy to the player
                    float ranAngle = Random.Range(90f, 180f) * Mathf.Deg2Rad; //Gets the radian of a random angle from 90-180

                    //Gets the direction of this angle
                    Vector3 ranDir = new(advoidedDir.x * Mathf.Cos(ranAngle) - advoidedDir.y * Mathf.Sin(ranAngle), advoidedDir.x * Mathf.Sin(ranAngle) + advoidedDir.y * Mathf.Cos(ranAngle), 0f);

                    _seeker.StartPath(transform.position, ranDir * 6f + transform.position, PathComplete);
                    break;

                case PathFindingTargets.Chase:
                    _seeker.StartPath(transform.position, lastSeenPos, PathComplete);
                    break;

                case PathFindingTargets.Position:
                    _seeker.StartPath(transform.position, _targetLocation, PathComplete);
                    break;

            }

        }
       
    }
    private void PathFind()
    {
        if (path == null)
        {

            return;
        }

        if (currentWaypoint >= path.vectorPath.Count - 1)
        {
            _pathEnded = true;
            _enemyRB2D.linearVelocity = Vector3.zero;
            if (_currentState == States.Roaming)
            {
                UpdatePath(PathFindingTargets.Random);
            }
            else if (_currentState == States.Chasing)
            {
                UpdatePath(PathFindingTargets.Position);
                
            }
           

            return;
        }
        else
        {
            _pathEnded = false;
        }

        //path.vectorPath[currentWaypoint] //The vector3 location of the current waypoint on the path


        float distance2Waypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
        if (distance2Waypoint < _nextWaypointDistance)
        {
            currentWaypoint++;
        }
        _direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        
    }

    private void DetectPlayer()
    {
        float distance2Player = Vector3.Distance(_player.transform.position, transform.position);
        if (distance2Player < _detectRange + 1f)
        {
            Vector3 dir2Player = _player.transform.position - transform.position;
            dir2Player = dir2Player.normalized;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, dir2Player, _detectRange, _combinedLayers);


            if (hit)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    _currentState = States.Chasing;
                    _enemyRB2D.linearVelocity = Vector3.zero;
                    _attackTimer = _attackDuration;

                }
            }

        }
    }

    private void Move()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) > _maxDistance)
        {
            _enemyRB2D.linearVelocity = Vector3.zero;
            return;
        }
        if (_currentState == States.Roaming)
        {
            PathFind();
            _enemyRB2D.linearVelocity = _direction * _stats.MoveSpeed;
        }
        else if (_currentState == States.Chasing)
        {
            Vector3 playerPos = _player.transform.position;
            float xDir = currentAgroState == AgroStates.TopLeft || currentAgroState == AgroStates.BottomLeft ? -1 : 1;
            float yDir = currentAgroState == AgroStates.TopLeft || currentAgroState == AgroStates.TopRight ? -1 : 1;

            Vector3 newTargetLocation = new (playerPos.x + _targetOffset * xDir, playerPos.y + _targetOffset * yDir, 0f);
            if (Vector3.Distance(_targetLocation, newTargetLocation) > 1f)
            {
                _targetLocation = newTargetLocation;
                UpdatePath(PathFindingTargets.Position);
            }

            
            if (Vector3.Distance(_targetLocation, transform.position) > 1f)
            {
                _targetReached = false;
                PathFind();
                _enemyRB2D.linearVelocity = _direction * _stats.MoveSpeed;
            }
            else
            {
                _enemyRB2D.linearVelocity = Vector3.zero;
                _targetReached = true;
                if (_startMovespeed < _stats.MoveSpeed)
                {
                    ChangeAgroState();
                    _targetReached = false;
                    _attackTimer = _attackDuration;
                    _stats.MoveSpeed = _startMovespeed;
                    
                }
            }

        }
        else if (_currentState == States.Attacking)
        {
            _enemyRB2D.linearVelocity = _direction * _stats.MoveSpeed;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && _attacking)
        {
            if (collision.gameObject.TryGetComponent(out StatsScript PlayerStats))
            {
                PlayerStats.Health -= _stats.Damage(_baseDamage, "melee");
                _currentState = States.Chasing;
                _attacking = false;
                switch (currentAgroState)
                {
                    case AgroStates.TopLeft:
                        currentAgroState = AgroStates.BottomRight;
                        break;
                    case AgroStates.TopRight:
                        currentAgroState = AgroStates.BottomLeft;
                        break;
                    case AgroStates.BottomLeft:
                        currentAgroState = AgroStates.TopRight;
                        break;
                    case AgroStates.BottomRight:
                        currentAgroState = AgroStates.TopLeft;
                        break;
                }
                PlayerStats.Slow(1f, .35f);
                
            }
        }
    }

 
   private void ChangeAgroState()
    {
        switch (currentAgroState)
        {
            case AgroStates.TopLeft:
                currentAgroState = Random.value > 0.5f ? AgroStates.BottomLeft: AgroStates.TopRight;
                break;
            case AgroStates.TopRight:
                currentAgroState = Random.value > 0.5f ? AgroStates.BottomRight : AgroStates.TopLeft;
                break;
            case AgroStates.BottomLeft:
                currentAgroState = Random.value > 0.5f ? AgroStates.BottomRight : AgroStates.TopLeft;
                break;
            case AgroStates.BottomRight:
                currentAgroState = Random.value > 0.5f ? AgroStates.BottomLeft : AgroStates.TopRight;
                break;
        }
    }

    private void Die()
    {
        Destroy(gameObject); // (E)

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_targetLocation, 1f);
    }
}
