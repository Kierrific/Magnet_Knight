//using UnityEditor.Experimental.GraphView;
using Pathfinding;
using UnityEngine;

//Need to make it so enemiees don't get stuck on each other while pathfinding

public class MeleeEnemyScript : MonoBehaviour
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
        Far = 0, // Distance > 10
        Close = 1, // Distance < 5
        Middle = 2, //5 < Distance < 10
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
    [Tooltip("The size of the enemies sword swing.")][SerializeField] private float _attackRadius = 1f;
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

    private bool _canMove = true;
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
    private float _circleDir = 1f;

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
        //_seeker.StartPath(transform.position, _player.transform.position, PathComplete); //Make this update when needed rather than once

        _seeker.StartPath(transform.position, (Vector3)(Random.insideUnitCircle * _roamDistance) + transform.position, PathComplete);
        //Vector3 randomDirection = Random.insideUnitCircle * 10f + transform.position;
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
            if (_attackTimer < 0 && !_attacking)
            {
                UpdatePath(PathFindingTargets.Player);
                _currentState = States.Attacking;
                _attacking = true;
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

            }

        }
        else
        {

        }
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
                    //Debug.Log($"Name of object hit: {hit.collider.gameObject.name}");
                    _currentState = States.Chasing;
                }
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
            float distance2Player = Vector3.Distance(_player.transform.position, transform.position);
            if (_currentState == States.Roaming)
            {
                UpdatePath(PathFindingTargets.Random);
            }
            else if (_currentState == States.Chasing && distance2Player < 5f)
            {
                UpdatePath(PathFindingTargets.Retreat);
            }
            else if (_currentState == States.Chasing && distance2Player >= 10f)
            {
                _currentState = States.Roaming;
                UpdatePath(PathFindingTargets.Random);
            }
            else if (_currentState == States.Attacking)
            {
                UpdatePath(PathFindingTargets.Player);

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
            //WallCollision();
            agroSwapTimer -= Time.deltaTime;
            float distance2Player = Vector3.Distance(_player.transform.position, transform.position);
            if (agroSwapTimer < 0)
            {
                if (distance2Player > 10f && currentAgroState != AgroStates.Far)
                {
                    currentAgroState = AgroStates.Far;
                    agroSwapTimer = agroSwapDuration;
                    //Debug.Log("Agro State swapped to Far");
                }
                else if (distance2Player < 5f && currentAgroState != AgroStates.Close)
                {
                    currentAgroState = AgroStates.Close;
                    agroSwapTimer = agroSwapDuration;
                    //Debug.Log("Agro State swapped to Close");
                }
                else if (distance2Player > 5f && distance2Player < 10f && currentAgroState != AgroStates.Middle)
                {
                    currentAgroState = AgroStates.Middle;
                    agroSwapTimer = agroSwapDuration;
                    //Debug.Log("Agro State swapped to Middle");

                }
            }

            if (_canMove)
            {
                _enemyRB2D.linearVelocity = Vector3.zero;
            }

            if (currentAgroState == AgroStates.Far) //distance2Player > 10f
            {
                _renameMeLater = false;
                if (Mathf.Approximately(_timeSinceSeen, 0f))
                {
                    lastSeenPos = _player.transform.position;
                    //Vector3 dir2Player = lastSeenPos - transform.position;
                    UpdatePath(PathFindingTargets.Chase);
                    PathFind();
                    //_direction = dir2Player.normalized;
                }
                if (_canMove)
                {
                    PathFind();
                    _enemyRB2D.linearVelocity = _direction * _stats.MoveSpeed;
                }
                _enemyRenderer.color = new Color(0f, 1f, 0f);

                _timeSinceSeen += distance2Player > 12f ? Time.deltaTime : 0f;

                if (_timeSinceSeen >= _playerUndetected)
                {
                    _currentState = States.Roaming;
                    UpdatePath(PathFindingTargets.Random);
                }
            }
            else
            {
                _timeSinceSeen = 0f;

                Vector3 dir2Player = _player.transform.position - transform.position;
                _enemyRenderer.color = new Color(1f, 0f, 0f);
                _direction = dir2Player.normalized;
                _enemyRB2D.linearVelocity = Vector3.zero;

                if (currentAgroState == AgroStates.Close) //distance2Player > 5f
                {
                    if (!_renameMeLater)
                    {
                        _renameMeLater = true;
                        UpdatePath(PathFindingTargets.Retreat);

                    }
                    _enemyRenderer.color = new Color(0f, 0f, 1f);
                    PathFind();
                    //_direction *= -1;
                    if (_canMove)
                    {
                        _enemyRB2D.linearVelocity = _direction * (_stats.MoveSpeed / 1.5f);
                    }
                }
                else
                {
                    if (_canMove)
                    {
                        _renameMeLater = false;
                        _direction = Vector3.Cross(dir2Player, Vector3.forward) * _circleDir;
                        _direction = _direction.normalized;
                        WallCollision();
                        _enemyRB2D.linearVelocity = _direction.normalized * _stats.MoveSpeed;
                    }
                }

            }
        }
        else if (_currentState == States.Attacking)
        {
            //("ATTACKIONG/1?!??!?!?!?");
            if (Vector3.Distance(_player.transform.position, path.vectorPath[^1]) > 1f)//
            {
                UpdatePath(PathFindingTargets.Player);
            }

            float distance2Player = Vector3.Distance(_player.transform.position, transform.position);

            if (distance2Player < 1.75f) //Make attack (E)
            {
                HandleAttack();
                _enemyRenderer.color = new Color(1f, 1f, 0f);
            }
            else
            {
                PathFind();
                _enemyRB2D.linearVelocity = _direction.normalized * _stats.MoveSpeed;

            }



        }
    }

    private void WallCollision()
    {
        _wallCenterPosition = (Vector2)transform.position + ((Vector2)_direction * (_enemyRenderer.size.x / 2 + 0.5f)); //Change this if it doesnt work properly after adding the sprite(E)
        RaycastHit2D hit = Physics2D.BoxCast(_wallCenterPosition, _wallCastSize, 0, Vector2.zero, 0, _worldLayer);
        if (hit.collider != null)
        {
            float distance2Player = Vector3.Distance(_player.transform.position, transform.position);
            if (_currentState == States.Chasing && distance2Player < 10f && distance2Player > 5f)
            {
                _direction *= -1f;
                _circleDir *= -1f;
            }
            else if (_currentState == States.Attacking) // Means there is a wall between the player and the enemy
            {

            }
        }
    }


    private void HandleAttack()
    {
        _attacking = false;
        _attackTimer = _attackCooldown;
        _currentState = States.Chasing;
        Vector2 dir2Player = _player.transform.position - transform.position;
        dir2Player = dir2Player.normalized;
        Vector2 attackPosition = new (transform.position.x + dir2Player.x, transform.position.y + dir2Player.y);

        RaycastHit2D hit = Physics2D.CircleCast(attackPosition, _attackRadius, Vector2.zero, 0, _playerLayer);
        if (hit)
        {
            Debug.Log("PLAYER HIT!!!?!?!?!?!??");
        }
        //float distance2Player = Vector3.Distance(transform.position, _player.transform.position);
    }

    private void Die()
    {
        Destroy(gameObject); // (E)
    }


}
