using UnityEngine;
using UnityEngine.InputSystem; //InputAction.CallbackContext ctx
using System.Collections.Generic;

//TO DO:
//Go through and look at anything marked

//Add IFrames to the stats script and make a check when taking damage 

//Make some sort of UI tell for whether or not you're in positive or negative

//Fix block not working properly 

[RequireComponent(typeof(Rigidbody2D))] //Needs a Rigid Body 2D
//[RequireComponent(typeof(StatsScript))] //Needs a stats script
public class PlayerScript : MonoBehaviour
{
    public enum Polarity 
    {
        Negative = -1,
        Positive = 1
    }

    public enum PlayerActions
    {
        None = 0,
        FirstAttack = 1,
        SecondAttack = 2,
        Swapping = 3,
        Dashing = 4, 
    }





    //Serialize Field Variables
    [Tooltip("Drag the players RigidBody2D into this variable (Should define its self in script but still should change this to be sure)")] [SerializeField] private Rigidbody2D _rb2d;
    [Tooltip("The list of the various projectiles that the enemy can fire (Primairy - Secondary")] [SerializeField] private List<GameObject> _scrapProjectilePrefabs;
    [Tooltip("How long in seconds till the player can dash again before scaling of cooldown reduction")][SerializeField] private float _dashCooldown;
    [Tooltip("How large the players melee attack hits")] [SerializeField] private float _meleeRadius;
    [Tooltip("How long the player will dash for, will define its self as 1/3 of base dash cooldown if undefined")][SerializeField] private float _dashingLength;
    [Tooltip("The instance of the StatsScript attached to the player (Should define its self in script but you should still set it in inspector)")] [SerializeField] private StatsScript _stats;
    [Tooltip("The force added to the player every frame of the dash")] [SerializeField] private float _dashForce;
    [Tooltip("How long in seconds till the player will be able to melee atack again")] [SerializeField] private float _meleeCooldown = 0.5f; //Update this later or speed up the animation in relation to the sword swing(E) 
    [Tooltip("How long in seconds till the player will be able to ranged attack again")] [SerializeField] private float _rangeCooldown = 0.3f; //Update this later same reason as _meleeCooldown (E)
    [Tooltip("The base damage (int) of the sword")] [SerializeField] private int _meleeDamage = 5;
    [Tooltip("The base amount of scrap lost (int) when blocking an attack.")] [SerializeField] private int _scrapLoss = 5;
    //[Tooltip("The base damage (int) of the projectile")] [SerializeField] private int _projectileDamage = 1;
  


    //Layers
    [Header("Layers")]
    [Tooltip("The layer for enemies, should define its self in script but still should change this to be sure")] [SerializeField] private LayerMask _enemyLayer;
    [Tooltip("The layer for projectiles, should define its self in script but still should change this to be sure")] [SerializeField] private LayerMask _projectileLayer;
    [Tooltip("The layer for the environment, should define its self in script but still should change this to be sure")] [SerializeField] private LayerMask _groundLayer;


    //Attack Variables
    private bool _mainAttackPressed;
    private bool _secondAttackPressed;
    private bool _abilityPressed;
    private float _attackChargeTime;
    private bool _blocking;
    
    private float _attackTimer; 
    private PlayerActions _playerBusy = PlayerActions.None; //Use this variable to check if another action is current being acted for example if using ability 1 set string to something like ability1
    private Vector3 _meleePosition; //Really only used for draw gizmos
    
    

    //Movement Variables
    private Vector2 _playerMovement;
    private Vector3 _playerDirection;
    private bool _canMove = true;
    private bool _dashing = false;
    private float _dashCooldownTimer;
    private float _dashingLengthTimer;



    //Other Variables
    private Polarity _playerState = Polarity.Positive; //Positive or Negative
    private Vector3 _mousePosition; //The mouse positions location relative to the world
    private Vector3 _mouseDirection; //The direction between the player and the mouses position
    private float _mouseAngle; //The angle in degrees between the player and the mouses position (-180 to 180)
    private SpriteRenderer _playerSpriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        //Sets some variables if they're not set in the inspector (You still should set these in the inspector if you're using them)
        if (_rb2d == null) 
        {
            _rb2d = GetComponent<Rigidbody2D>();
        }
        if (_enemyLayer == 0)

        {
            _enemyLayer = LayerMask.GetMask("Enemy");
            
        }

        if (_groundLayer == 0)
        {
            _groundLayer = LayerMask.GetMask("Ground");

        }

        _dashingLength = Mathf.Approximately(_dashingLength, 0f) ? _dashCooldown / 3f : _dashingLength;
        _dashCooldownTimer = _dashCooldown;

        _playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();


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
            Debug.LogWarning("Player Game Object requires a stats script and will not function properly without it.\nAsk me if you need help with that.", _stats);
        }

    }

    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime; //Caches the delta time for that frame, you should use this more or less any where instead of Time.deltaTime
        
        //Look into calculating this when needed rather then every frame (E)
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0f;

        _dashCooldownTimer -= _dashCooldownTimer > 0f ? delta : 0;

        _attackChargeTime += _mainAttackPressed || _abilityPressed || _secondAttackPressed ? delta: 0f;

        if (_blocking)
        {
            _mouseDirection = _mousePosition - transform.position;
            _meleePosition = transform.position + _mouseDirection.normalized;
            if (_stats.Scrap < _scrapLoss)
            {
                HandleSecondaryAttack(); // (E)
                
            }
            else
            {
                RaycastHit2D[] hits = Physics2D.CircleCastAll(_meleePosition, _meleeRadius, Vector2.zero, 0, _projectileLayer);
                foreach (RaycastHit2D hit in hits)
                {
                    if (_stats.Scrap > _scrapLoss)
                    {
                        _stats.Scrap -= _scrapLoss;
                        Destroy(hit.collider.gameObject);
                    }
                    else
                    {
                        HandleSecondaryAttack();
                        break;
                    }
                }
            }
        }

        if (_attackTimer > 0f) //Checks if attack is off cooldown
        {
            _attackTimer -= delta;
        }
        else if (_playerBusy != PlayerActions.None) //If attack is off cooldown, then reset current status
        {
            _playerBusy = PlayerActions.None;
        }




        if (_canMove)
        {
            _rb2d.linearVelocity = _playerMovement;
        }




    }

    void FixedUpdate()
    {
        if (_dashing)
        {
            HandleDash(); 
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        _playerMovement = ctx.ReadValue<Vector2>() * _stats.MoveSpeed;

        if (!_dashing)
        {
            _playerDirection = ctx.ReadValue<Vector2>() != Vector2.zero ? ctx.ReadValue<Vector2>() : _playerDirection; //Sets the player direction to the direction the player is moving unless the player isn't moving
        }
    }

    public void Swap(InputAction.CallbackContext ctx)
    {
        if (ctx.started && _playerBusy == PlayerActions.None)
        {
            _playerState = (Polarity)((int)_playerState * -1);
             _attackChargeTime = 0f; //Resets the charge time variable (This should be unneeded now)

            //Add another aditional logic here (E)
        }
    }

    public void MainAttack(InputAction.CallbackContext ctx)
    {
        if (_playerBusy != PlayerActions.FirstAttack && _playerBusy != PlayerActions.None) //Means the player is currently doing another action (every action should start with this)
        {
           return; 
        }

        if (ctx.ReadValue<float>() == 1) //Meaans that key is currently being pressed 
        {
            _playerBusy = PlayerActions.FirstAttack; //Make sure this name matches the previously defined name in the last if statement, (Look into using if _...Pressed instead so a specific string isn't required (E))
            _mainAttackPressed = true;
        }
        else //Means that key got released
        {
            HandleMainAttack();
        }
    }

    private void HandleMainAttack()
    {
        if (_playerBusy != PlayerActions.FirstAttack && _playerBusy != PlayerActions.None) //Means the player is currently doing another action (every action should start with this)
        {
            return;
        }


        if (_attackTimer > 0)
        {
            return; //Look into a way to adding a slight buffer, like if the player pressed while cooldown is at 0.05 seconds it waits that time then attack rather than completley canceling attack (E)
        }
        _mainAttackPressed = false;
        _mouseDirection = _mousePosition - transform.position; //Look into using a cached transform position rather than calling it (E)
        _mouseAngle = Mathf.Atan2(_mouseDirection.y, _mouseDirection.x) * Mathf.Rad2Deg;
        if (_playerState == Polarity.Positive) //If true the player is in melee form
        {
            _attackTimer = _meleeCooldown;
            _meleePosition = transform.position + _mouseDirection.normalized;
            RaycastHit2D[] hits = Physics2D.CircleCastAll(_meleePosition, _meleeRadius, Vector2.zero, 0, _enemyLayer);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject.TryGetComponent(out StatsScript EnemyStats) && TryGetComponent(out StatsScript PlayerStats))
                {
                    EnemyStats.Health -= _stats.Damage(_meleeDamage, "melee");
                    if (EnemyStats.Health <= 0) //The less than is likely not needed because health SHOULD be clamped to a minimum of 0
                    {
                        _stats.Scrap += EnemyStats.Scrap;
                    }
                }
            }
            _attackChargeTime = 0f;
        }
        else //Means the player is in ranged form
        {
            _attackTimer = _rangeCooldown;
            Vector3 projectileSpawnLocation = transform.position;
            //Adds an calculated offset to where the player spawns relative to the size of the projectile and to the size of the player //_mouseDirection = _mousePosition - transform.position; 
            float xLoc = projectileSpawnLocation.x + (_mouseDirection.normalized.x * (_playerSpriteRenderer.size.x / 2 + .1f)) + _mouseDirection.normalized.x * _scrapProjectilePrefabs[0].GetComponent<SpriteRenderer>().size.x / 2;
            float yLoc = projectileSpawnLocation.y + (_mouseDirection.normalized.y * (_playerSpriteRenderer.size.y / 2 + .1f)) + _mouseDirection.normalized.y * _scrapProjectilePrefabs[0].GetComponent<SpriteRenderer>().size.y / 2;
            projectileSpawnLocation = new Vector3(xLoc, yLoc, projectileSpawnLocation.z);

            Vector3 TempDirection = _mousePosition - projectileSpawnLocation;
            float TempAngle = Mathf.Atan2(TempDirection.y, TempDirection.x) * Mathf.Rad2Deg;
            GameObject scrapProjectile = Instantiate(_scrapProjectilePrefabs[0], projectileSpawnLocation, Quaternion.Euler(new Vector3(0, 0, TempAngle)));
            ScrapProjScript projScript = scrapProjectile.GetComponent<ScrapProjScript>(); //(I)
            projScript.Damage = _stats.Damage(projScript.Damage, "range");
            


            _attackChargeTime = 0f; //Resets the charge time variable
        }
    }

    public void SecondaryAttack(InputAction.CallbackContext ctx)
    {
        if (_playerBusy != PlayerActions.SecondAttack && _playerBusy != PlayerActions.None) //Means the player is currently doing another action (every action should start with this)
        {
            return;
        }

        if (_attackTimer > 0)
        {
            return; //Look into a way to adding a slight buffer, like if the player pressed while cooldown is at 0.05 seconds it waits that time then attack rather than completley canceling attack (E)
        }

        if (ctx.ReadValue<float>() == 1)
        {
            _playerBusy = PlayerActions.SecondAttack;
            _secondAttackPressed = true;
            if (_playerState == Polarity.Positive)//Melee
            {
                //Start blocking
                _blocking = true;
            }
            else //Range
            {
                
            }
        }
        else //Key released
        {
           HandleSecondaryAttack(); 
        }   
     }

     private void HandleSecondaryAttack()
    {
        if (_playerBusy != PlayerActions.SecondAttack && _playerBusy != PlayerActions.None) //Means the player is currently doing another action (every action should start with this)
        {
            return;
        }
        _secondAttackPressed = false;
        _mouseDirection = _mousePosition - transform.position; //Look into using a cached transform position rather than calling it (E)
        _mouseAngle = Mathf.Atan2(_mouseDirection.y, _mouseDirection.x) * Mathf.Rad2Deg;
        if (_playerState == Polarity.Positive)//Melee
        {
            _blocking = false;

            _attackTimer = _meleeCooldown;
        }
        else 
        {
            if (_attackTimer > 0)
            {
                //(E)
                return; 
            }

            Vector3 projectileSpawnLocation = transform.position;
            //Adds an calculated offset to where the player spawns relative to the size of the projectile and to the size of the player
            int projectileIndex = 0; //(0, 1, 2)
            Debug.Log($"Attack Charge Time: {_attackChargeTime}");
            if (_attackChargeTime > 0.5f && _stats.Scrap >= 2)
            {
                projectileIndex++;
                if (_attackChargeTime > 1f && _stats.Scrap >= 3)
                {
                    projectileIndex++; 
                }
            }

            float xLoc = projectileSpawnLocation.x + (_mouseDirection.normalized.x * (_playerSpriteRenderer.size.x / 2 + .1f)) + _mouseDirection.normalized.x * _scrapProjectilePrefabs[projectileIndex].GetComponent<SpriteRenderer>().size.x / 2;
            float yLoc = projectileSpawnLocation.y + (_mouseDirection.normalized.y * (_playerSpriteRenderer.size.y / 2 + .1f)) + _mouseDirection.normalized.y * _scrapProjectilePrefabs[projectileIndex].GetComponent<SpriteRenderer>().size.y / 2;
            projectileSpawnLocation = new Vector3(xLoc, yLoc, projectileSpawnLocation.z);

            Vector3 TempDirection = _mousePosition - projectileSpawnLocation;
            float TempAngle = Mathf.Atan2(TempDirection.y, TempDirection.x) * Mathf.Rad2Deg;
            GameObject scrapProjectile = Instantiate(_scrapProjectilePrefabs[projectileIndex], projectileSpawnLocation, Quaternion.Euler(new Vector3(0, 0, TempAngle)));
            ScrapProjScript projScript = scrapProjectile.GetComponent<ScrapProjScript>();
            projScript.Damage = _stats.Damage(projScript.Damage, "range");

            _attackTimer = _rangeCooldown;
            _stats.Scrap -= projectileIndex + 1;
        }
            
        _attackChargeTime = 0f;
    }

    public void Dash(InputAction.CallbackContext ctx)
    {
        if (_dashCooldownTimer <= 0f)
        {
            _dashing = true;
            _canMove = false;
            _dashCooldownTimer = _dashCooldown;
        }
    }

    private void HandleDash()
    {
        _dashingLengthTimer -= Time.deltaTime;
        _rb2d.AddForce(_dashForce * _playerDirection.normalized);

        if (_dashingLengthTimer < 0f)
        {

            Debug.Log(_dashCooldownTimer);
            _dashing = false;
            _canMove = true;
            _dashingLengthTimer = _dashingLength;
        }
    }



    public void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_meleePosition, _meleeRadius);
    }




}
