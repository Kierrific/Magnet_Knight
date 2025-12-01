using UnityEngine;


[RequireComponent(typeof(StatsScript))]
[RequireComponent(typeof(Rigidbody2D))]
public class RangedEnemyScript : MonoBehaviour
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
    [Tooltip("How often the enemy will look for the player")] [SerializeField] private float _detectCooldown;
    [Tooltip("After how long of not seeing the player will the enemy return to a roaming state.")][SerializeField] private float _playerUndetected = 3f; //Change name later (E)
    [Tooltip("How long the enemy will strafe in 1 direction")][SerializeField] private float _strafeDuration;
    [Tooltip("Set this to the prefab of the projectile you want the enemy to attack with")][SerializeField] private GameObject _bullet;
    [Tooltip("How often the enemy can attack in seconds.")] [SerializeField] private float _attackCooldown;
    [Tooltip("The amount of rays the enemie's player detection function uses.")] private int _detectionRays = 5;
    [Tooltip("The angle in which rays will spawn in while trying to find the player")] private float _detectionAngle = 130f;


    private bool _canMove = true;
    private float _detectTimer;
    private float _targetLocation; 
    private Vector3 _direction = Vector2.down;
    private float _timeSinceSeen = 0f;
    private float _strafeDirection = 1f;
    private float _attackTimer;
    private float _strafeTimer;
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
        _strafeTimer = _strafeDuration;

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

    // Update is called once per frame
    void Update()
    {
        if (_stats.Health <= 0)
        {
            Die();
        }
        Move();

        if (_currentState == States.Chasing)
        {
            if (_attackTimer >= 0f)
            {
                _attackTimer -= Time.deltaTime;
            }
            else
            {
                _attackTimer = _attackCooldown;
                Attack();
            }
        }
        else if (_currentState == States.Roaming)
        {
            if (_detectTimer > 0f)
            {
                _detectTimer -= Time.deltaTime; 
            }
            else
            {
                float distance2Player = Vector3.Distance(_player.transform.position, transform.position);
                if (distance2Player < _detectRange + 1f) //Slight performance check, ray cast wont even run unless there is the possibility it hits the player
                {
                    DetectPlayer(_detectionRays);
                    _detectTimer = _detectCooldown;
                }
                
            }
        }
        else if (_currentState == States.Attacking)
        {
            HandleAttack();
        }
    }

    private void DetectPlayer(int rays)
    {
        if (rays > 0)
        {
            //_detectionAngle
            Vector2 startingDirection = (Vector2) (_player.transform.position - transform.position).normalized;
            
            startingDirection = _direction.normalized;
            float angle = Mathf.Atan2(startingDirection.y, startingDirection.x) * Mathf.Rad2Deg;
            //Debug.Log($"Current Angle to player {angle}");
            angle += (_detectionAngle / 2);
            //Debug.Log($"Current Angle to player {angle}");
            startingDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            Vector2 currentDirection = startingDirection;
            float angleStep = _detectionAngle / ((float)rays - 1);
            for (int i = 0; i < rays; i++)
            {
                
                RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDirection, _detectRange, _playerLayer);
                
                if (hit == true)
                {
                    _currentState = States.Chasing;
                    return;
                }

                angle -= angleStep; 
                currentDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            }
        }
        else //Make a raycast from enemy to player and if it hit then change state (I)
        {
            _currentState = States.Chasing;
            return;
        }
    }

    private void Move()
    {
        if (_currentState == States.Roaming)
        {
            WallCollision();
            if (!_canMove || _targetLocation < 0){
                _canMove = true;
                _targetLocation = Random.Range(5f, 15f);

                //Get a random direction
                float xDir = Random.Range(-1f, 1f);
                float yDir = Random.Range(-1f, 1f) > 0f ? Mathf.Sqrt(1-(xDir*xDir)): Mathf.Sqrt(1-(xDir*xDir)) * -1;
                _direction = (Vector3)new Vector2(xDir, yDir);

                _enemyRB2D.linearVelocity = Vector3.zero;
            }
            _targetLocation -= _stats.MoveSpeed * Time.deltaTime;
            _enemyRB2D.linearVelocity = _direction * _stats.MoveSpeed;
        }
        else if (_currentState == States.Chasing)
        {
            WallCollision();

            if (!_canMove)
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
                    _enemyRB2D.linearVelocity = _direction * (_stats.MoveSpeed / 1.75f);
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
                _timeSinceSeen = 0f; //Resets the time since the enemy last saw the player

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
                        _enemyRB2D.linearVelocity = _direction * (_stats.MoveSpeed / 2);
                    }
                }
                else
                {
                    if (_canMove)
                    {
                        if (_strafeTimer > 0)
                        {
                            _strafeTimer -= Time.deltaTime;
                        }
                        else
                        {
                            _strafeTimer = _strafeDuration;
                            _strafeDirection *= -1;
                        }
                        _direction = Vector3.Cross(dir2Player.normalized, Vector3.forward) * _strafeDirection;
                        _enemyRB2D.linearVelocity = _direction * (_stats.MoveSpeed / 1.75f);
                    }
                }
            }
            _canMove = true;
        }
        else if (_currentState == States.Attacking)
        {
            _enemyRB2D.linearVelocity = Vector3.zero; 
        }
    }

    private void WallCollision()
    {
        float distance2Player = Vector3.Distance(_player.transform.position, transform.position);
        _wallCenterPosition = (Vector2) transform.position + ((Vector2) _direction * (_enemyRenderer.size.x / 2 +  0.5f)); //Change this if it doesnt work properly after adding the sprite(E)
        RaycastHit2D hit = Physics2D.BoxCast(_wallCenterPosition, _wallCastSize, 0, Vector2.zero, 0, _worldLayer);
        if (hit.collider != null)
        {
            if (_currentState == States.Roaming)
            {
                _direction *= -1;
            }
            else if (_currentState == States.Chasing)
            {
                if (distance2Player > 5f && distance2Player < 10f)
                {
                    _strafeTimer = _strafeDuration;
                    _strafeDirection *= -1;
                }
                else
                {
                    _canMove = false;
                }
            }
        }
    }

    private void Attack()
    {
        float distance2Player = Vector3.Distance(_player.transform.position, transform.position);
        if (distance2Player <= 10f)
        {
            _attackDir = _player.transform.position - transform.position;
        }
        

        _attacked = false;
        _attackDurationTimer = _attackDuration;
        _currentState = States.Attacking;
            
        
            //GameObject instancedBullet = Instantiate(_bullet, transform.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
            //EnemyProjectileScript bulletScript = instancedBullet.GetComponent<EnemyProjectileScript>();
            //bulletScript.Damage = _stats.Damage(bulletScript.Damage, "range"); //Make sure the fired projectile scales with enemy scaling
        
    }

    private void HandleAttack() //(E)
    {
        float distance2Player = Vector3.Distance(_player.transform.position, transform.position);


        _attackDurationTimer -= Time.deltaTime;

        if (_attackDurationTimer > (_attackDuration / 2f)) //Make it so that it 1 First prepares the shot like in animation, 2 fires the shot, 3 goes back to other animation as soon as I get the enemy animation (I)
        {
            if (distance2Player <= 10f)
            {
                _attackDir = _player.transform.position - transform.position;
            }
        }
        else if (_attackDurationTimer > 0f) //Stop Updating Direction
        {
            if (!_attacked) //Play sound sound indicator or something that enemy is about to attack(E)
            {
                _attacked = true;
            }

        }
        else
        {
            float angle = Mathf.Atan2(_attackDir.y, _attackDir.x) * Mathf.Rad2Deg;
            GameObject instancedBullet = Instantiate(_bullet, transform.position, Quaternion.Euler(new Vector3(0f, 0f, angle)));
            ScrapProjScript bulletScript = instancedBullet.GetComponent<ScrapProjScript>();
            bulletScript.Damage = _stats.Damage(bulletScript.Damage, "range"); //Make sure the fired projectile scales with enemy scaling
            _currentState = States.Chasing;
        }
    }

    private void Die()
    {
        Destroy(gameObject); // (E)
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_wallCenterPosition, (Vector3)_wallCastSize);
    }
}
