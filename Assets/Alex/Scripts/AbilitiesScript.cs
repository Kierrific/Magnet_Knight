using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class AbilitiesScript : MonoBehaviour
{
        public enum Abilities
    {
        None = 0,
        MagnetTrap = 1,
        PolarPull = 2,
        PolarBind = 3,
        OrbitalScrap = 4,
        ScrapSiphon = 5,
        RepulsionWave = 6,
        MagneticBlackhole = 7,
        SyntheticHeart = 8,
    }

    public enum AbilityActions
    {
        None = 0,
        Ability1 = 1,
        Ability2 = 2,
        Ability3 = 3,
    }
    
    
    [Tooltip("The instance of the StatsScript attached to the player (Should define its self in script but you should still set it in inspector)")] [SerializeField] private StatsScript _stats;
    [Tooltip("How far away from the mouse can the player select an enemy.")] [SerializeField] private float _selectionRange = 2.5f;
    [Tooltip("Set this to the prefab of the orbit game object")] [SerializeField] private GameObject _orbitPrefab;

    [Header("Ability Base Stats")]
    [Tooltip("The amount of base damage the Magnetic Trap ability does.")] [SerializeField] private int _trapDamage = 1;
    [Tooltip("The amount of base damage the Polar Pull ability does.")] [SerializeField] private int _pullDamage = 1;
    [Tooltip("The amount of base damage the Polar Bind ability does.")] [SerializeField] private int _bindDamage = 5;
    [Tooltip("The amount in which the player heals from the healing ability.")] [SerializeField] private int _healAmount = 3;

    [Header("Layers")]
    [Tooltip("The layer for enemies, should define its self in script but still should change this to be sure")] [SerializeField] private LayerMask _enemyLayer;

    private AbilityActions _currentAction = AbilityActions.None; 

    private List<Abilities> _abilityList = new List<Abilities> {Abilities.None, Abilities.None, Abilities.None};

    private GameObject _closestEnemy;
    private GameObject _secondClosestEnemy;
    
    private void Awake()
    {
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

        //Sets enemy later
        if (_enemyLayer == 0)

        {
            _enemyLayer = LayerMask.GetMask("Enemy");
            
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ability1(InputAction.CallbackContext ctx)
    {

        if (_currentAction != AbilityActions.None || _currentAction != AbilityActions.Ability1)
        {
            return;
        }

        if (ctx.ReadValue<float>() == 1f)
        {
            _currentAction = AbilityActions.Ability1;
        }
        else if (ctx.ReadValue<float>() == 0f)
        {
            _currentAction = AbilityActions.None;
        }
    }

    public void Ability2(InputAction.CallbackContext ctx)
    {
        if (_currentAction != AbilityActions.None || _currentAction != AbilityActions.Ability2)
        {
            return;
        }

        if (ctx.ReadValue<float>() == 1f)
        {
            _currentAction = AbilityActions.Ability1;
        }
        else if (ctx.ReadValue<float>() == 0f)
        {
            _currentAction = AbilityActions.None;
        }
    }

    public void Ability3(InputAction.CallbackContext ctx)
    {
        if (_currentAction != AbilityActions.None || _currentAction != AbilityActions.Ability3)
        {
            return;
        }

        if (ctx.ReadValue<float>() == 1f)
        {
            _currentAction = AbilityActions.Ability1;
        }
        else if (ctx.ReadValue<float>() == 0f)
        {
            _currentAction = AbilityActions.None;
        }
    }

    private GameObject GetClosest()
    {
        Vector3 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0f;

        GameObject ClosestObject = null;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(_mousePosition, _selectionRange, Vector2.zero, 0, _enemyLayer);
        foreach (RaycastHit2D hit in hits)
        {
            if (ClosestObject == null)
            {
                ClosestObject = hit.collider.gameObject;
            }
            else if (Vector3.Distance(_mousePosition, ClosestObject.transform.position) > Vector3.Distance(_mousePosition, hit.collider.gameObject.transform.position))
            {
                ClosestObject = hit.collider.gameObject;
            }
        }

        return ClosestObject;
    }

    private void Heal() //Maybe change this to match the name of the ability in the GDD
    {
        _stats.Health += _healAmount;
        _stats.Scrap -= 1; //Maybe mess around with changing this based on the heal amount variable  
    }

    private void Trap() //Magnet Trap in the GDD
    {
        
        //Vector3 _mouseDirection = _mousePosition - transform.position; //Look into using a cached transform position rather than calling it (E)
        //float _mouseAngle = Mathf.Atan2(_mouseDirection.y, _mouseDirection.x) * Mathf.Rad2Deg;

        if (_closestEnemy == null)
        {
            return;
        }

        if (_closestEnemy.TryGetComponent(out StatsScript EnemyStats) && TryGetComponent(out StatsScript PlayerStats))
        {
            EnemyStats.Health -= _stats.Damage(_trapDamage, "ability");
            //EnemyStats.Movespeed = 0f;//ADD SLOW (I)
        }
        else
        {
            Debug.Log($"Either the player or {_closestEnemy.gameObject.name} doesnt have a stats script");  
        }


    }

    private void Pull() //Polar Pull in GDD
    {
        if (_closestEnemy == null)
        {
            return;    
        }

        if (_closestEnemy.TryGetComponent(out Rigidbody2D _enemyRB2D))
        {
            Vector3 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mousePosition.z = 0f;
            Vector3 _mouseDirection = (_mousePosition - _closestEnemy.transform.position).normalized;
            _enemyRB2D.AddForce(_mouseDirection * 1000f); //Change 1000f to a variable later
            if (_closestEnemy.TryGetComponent(out StatsScript EnemyStats) && TryGetComponent(out StatsScript PlayerStats))
            {
                EnemyStats.Health -= _stats.Damage(_pullDamage, "ability");
            }
            else
            {
                Debug.Log($"Either the player or {_closestEnemy.gameObject.name} doesnt have a stats script");  
            }
        }
        else
        {
            Debug.LogWarning($"{_closestEnemy.name} does not have a Rigidbody2D so the players abilities cannot function properly", _closestEnemy);
            return;
        }
    }

    private void Bind() //Polar Bind in GDD
    {
        if (_closestEnemy == null || _secondClosestEnemy == null)
        {
            return;    
        }

        if (_closestEnemy.TryGetComponent(out Rigidbody2D _enemyRB2D) && _secondClosestEnemy.TryGetComponent(out Rigidbody2D _enemyTwoRB2D))
        {
            Vector3 BindDirection = (_closestEnemy.transform.position - _secondClosestEnemy.transform.position).normalized;
            _enemyRB2D.AddForce(BindDirection * 1000f); //Change 1000f to a variable later (E)
            _enemyRB2D.AddForce(BindDirection * -1000f); //Change 1000f to a variable later (E)
            if (_closestEnemy.TryGetComponent(out StatsScript EnemyStats) && _secondClosestEnemy.TryGetComponent(out StatsScript EnemyTwoStats))
            {
                EnemyStats.Health -= _stats.Damage(_bindDamage, "ability");
                EnemyTwoStats.Health -= _stats.Damage(_bindDamage, "ability");
            }
            else
            {
                Debug.Log($"Either {_secondClosestEnemy.gameObject.name} or {_closestEnemy.gameObject.name} doesnt have a stats script");  
            }
        }
        else
        {
            Debug.LogWarning($"{_closestEnemy.name} or {_secondClosestEnemy.name} does not have a Rigidbody2D so the players abilities cannot function properly", _closestEnemy);
            return;
        }   
    }

    private void Orbit()
    {
        GameObject scrapOrbit = Instantiate(_orbitPrefab, transform.position, Quaternion.identity, transform); //(I)
        //scrapOrbit.transform.SetParent(transform, false);
    }
}
