using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MeleeEnemyScript : MonoBehaviour
{
    public enum States
    {
        Roaming = 0,
        Chasing = 1,
        Attacking = 2
    }


    [Tooltip("The instance of the StatsScript attached to the player (Should define its self in script but you should still set it in inspector)")][SerializeField] private StatsScript _stats;
    [Tooltip("The distance in which the enemy can detect the player")][SerializeField] private float _detectRange;
    [Tooltip("Set this to the layer that the player is on")][SerializeField] private LayerMask _playerLayer;
    [Tooltip("Set this to the layer that the environment is on")][SerializeField] private LayerMask _worldLayer;
    [Tooltip("The size of the BoxCast that checks if the enemy will run into a wall")][SerializeField] private Vector2 _wallCastSize;
    [Tooltip("How often the enemy will look for the player")][SerializeField] private float _detectCooldown;
    [Tooltip("After how long of not seeing the player will the enemy return to a roaming state.")][SerializeField] private float _playerUndetected = 3f; //Change name later (E)
    [Tooltip("How often the enemy can attack in seconds.")][SerializeField] private float _attackCooldown;
    [Tooltip("The size of the enemies sword swing.")] private float _attackRadius = 1f;

    private bool _canMove = true;
    private float _detectTimer;
    private float _targetLocation;
    private Vector3 _direction = Vector2.down;
    private float _timeSinceSeen = 0f;
    private float _attackTimer;
    private Rigidbody2D _enemyRB2D;
    private SpriteRenderer _enemyRenderer;
    private States _currentState = States.Roaming;
    private Vector2 _wallCenterPosition; //The center position of the boxcast that checks if the enemy is going to run into a wall
    private GameObject _player;
    private Vector3 _attackDir;
    private float _attackDurationTimer;
    private float _attackDuration = 1f; //How long each attack last
    private bool _attacked = false; //Whether or not the enemy has attacked this attack pattern

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
            Debug.LogWarning("Ranged Enemy Script cannot find the player object and will not work");
            Debug.Break();
        }


        //Verifies the stats script is set up properly
        if (TryGetComponent<StatsScript>(out StatsScript statsScript))
        {
            if (_stats != statsScript)
            {
                _stats = statsScript;
                Debug.LogWarning("The stats script component on the player should be assigned to the Stats variable in the inspector", _stats);
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
    }

    private void DetectPlayer()
    {
        float distance2Player = Vector3.Distance(_player.transform.position, transform.position);
        if (distance2Player < _detectRange + 1f)
        {
            Vector3 dir2Player = _player.transform.position - transform.position;
            dir2Player = dir2Player.normalized;
            RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, dir2Player, _detectRange);

            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log($"Name of object hit: {hit.collider.gameObject.name}");
            }
            else
            {
                Debug.Log("ibug8gi");
            }
        }
    }

    private void Move()
    {
        if (_currentState == States.Roaming)
        {
            WallCollision();
            if (!_canMove || _targetLocation < 0)
            {
                _canMove = true;
                _targetLocation = Random.Range(5f, 15f);

                //Get a random direction
                float xDir = Random.Range(-1f, 1f);
                float yDir = Random.Range(-1f, 1f) > 0f ? Mathf.Sqrt(1 - (xDir * xDir)) : Mathf.Sqrt(1 - (xDir * xDir)) * -1;
                _direction = (Vector3)new Vector2(xDir, yDir);

                _enemyRB2D.linearVelocity = Vector3.zero;
            }

            _targetLocation -= _stats.MoveSpeed * Time.deltaTime;
            _enemyRB2D.linearVelocity = _direction * _stats.MoveSpeed;

        }
        else if (_currentState == States.Chasing)
        {
            WallCollision();

            if (_canMove)
            {
                _enemyRB2D.linearVelocity = Vector3.zero;
            }
            float distance2Player = Vector3.Distance(_player.transform.position, transform.position);

            if (distance2Player > 10f)
            {
                if (Mathf.Approximately(_timeSinceSeen, 0f))
                {
                    Vector3 dir2Player = _player.transform.position - transform.position;
                    _direction = dir2Player.normalized;
                    _attackDir = _direction;
                }
                if (_canMove)
                {
                    _enemyRB2D.linearVelocity = _direction * _stats.MoveSpeed;
                }
                _enemyRenderer.color = new Color(0f, 1f, 0f);

                _timeSinceSeen += distance2Player > 12f ? Time.deltaTime : 0f;

                if (_timeSinceSeen >= _playerUndetected)
                {
                    _currentState = States.Roaming;
                }
            }
            else
            {
                _timeSinceSeen = 0f;

                Vector3 dir2Player = _player.transform.position - transform.position;
                _enemyRenderer.color = new Color(1f, 0f, 0f);
                _direction = dir2Player.normalized;
                _enemyRB2D.linearVelocity = Vector3.zero;

                if (distance2Player < 5f)
                {
                    _enemyRenderer.color = new Color(0f, 0f, 1f);
                    _direction *= -1;
                    if (_canMove)
                    {
                        _enemyRB2D.linearVelocity = _direction * (_stats.MoveSpeed / 1.5f);
                    }
                }
                else
                {
                    if (_canMove)
                    {
                        _direction = Vector3.Cross(dir2Player, Vector3.forward);
                        _enemyRB2D.linearVelocity = _direction * _stats.MoveSpeed;
                    }
                }

            }
        }
        else if (_currentState == States.Attacking) 
        {
            WallCollision();

            if (_canMove)
            {
                float distance2Player = Vector3.Distance(_player.transform.position, transform.position);
                _enemyRB2D.linearVelocity = Vector3.zero;
                if (distance2Player < 0.75f)
                {
                    Vector3 dir2Player = _player.transform.position - transform.position;
                    _direction = dir2Player.normalized;
                    _enemyRenderer.color = new Color(1f, 1f, 0f);
                    _enemyRB2D.linearVelocity = _direction * _stats.MoveSpeed * 2f;
                }
            }

        }
    }

    private void WallCollision()
    {
        _wallCenterPosition = (Vector2)transform.position + ((Vector2)_direction * (_enemyRenderer.size.x / 2 + 0.5f)); //Change this if it doesnt work properly after adding the sprite(E)
        RaycastHit2D hit = Physics2D.BoxCast(_wallCenterPosition, _wallCastSize, 0, Vector2.zero, 0, _worldLayer);
        if (hit.collider != null)
        {
            if (_currentState == States.Roaming)
            {
                _direction *= -1f; //Change this later to make it not just turn around(E)
            }
        }
    }


    private void HandleAttack()
    {
        float distance2Player = Vector3.Distance(transform.position, _player.transform.position);
    }

    private void Die()
    {
        Destroy(gameObject); // (E)
    }

    public void OnDrawGizmos()
    {
        //RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, dir2Player, _detectRange, _playerLayer);
        Gizmos.DrawWireSphere(transform.position, _detectRange);
        Gizmos.DrawWireCube(_wallCenterPosition, (Vector3)_wallCastSize);
    }
}
