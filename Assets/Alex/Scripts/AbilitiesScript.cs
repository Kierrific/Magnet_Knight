using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

//Make a function in statsscript similar to slow but for pushing enemy like a lerp would and use in pull and bind

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
    
    [Header("Random Variables")]
    [Tooltip("The instance of the StatsScript attached to the player (Should define its self in script but you should still set it in inspector)")] [SerializeField] private StatsScript _stats;
    [Tooltip("How far away from the mouse can the player select an enemy.")] [SerializeField] private float _selectionRange = 2.5f;
    [Tooltip("Set this to the prefab of the orbit game object")] [SerializeField] private GameObject _orbitPrefab;

    [Header("Ability Base Stats")]
    [Tooltip("The amount of base damage the Magnetic Trap ability does.")] [SerializeField] private int _trapDamage = 1;
    [Tooltip("How long the enemy stays trapped for in seconds.")] [SerializeField] private float _trapDuration = 3f;
    [Tooltip("The amount of base damage the Polar Pull ability does.")] [SerializeField] private int _pullDamage = 1;
    [Tooltip("The amount of base damage the Polar Bind ability does.")] [SerializeField] private int _bindDamage = 5;
    [Tooltip("The amount in which the player heals from the healing ability.")] [SerializeField] private int _healAmount = 3;

    [Header("Layers")]
    [Tooltip("The layer for enemies, should define its self in script but still should change this to be sure")] [SerializeField] private LayerMask _enemyLayer;

    private AbilityActions _currentAction = AbilityActions.None; 

    [Tooltip("The list of current player abilities.")] [SerializeField] private List<Abilities> _abilityList = new List<Abilities> {Abilities.None, Abilities.None, Abilities.None};
    private Vector3 _mousePosition;
    
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
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_mousePosition, _selectionRange);
        GetClosest();
    }

    public void Ability1(InputAction.CallbackContext ctx)
    {
        if (_currentAction != AbilityActions.None && _currentAction != AbilityActions.Ability1)
        {
            return;
        }

        if (ctx.ReadValue<float>() == 1f)
        {
            _currentAction = AbilityActions.Ability1;
            HandleAbility(1);
        }
        else if (ctx.ReadValue<float>() == 0f)
        {
            _currentAction = AbilityActions.None;
            HandleAbility(1);
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

    private void HandleAbility(int abilityNum)
    {
        abilityNum -= 1;
        if (_abilityList[abilityNum] == Abilities.MagnetTrap)
        {
            if (_currentAction == AbilityActions.None) //Ability keybind was released
            {
                Debug.Log("Trap Triggered");
                Trap();
            }
        }
        else if (_abilityList[abilityNum] == Abilities.PolarPull)
        {
            if (_currentAction == AbilityActions.None) //Ability keybind was released
            {
                Debug.Log("Pull Triggered");
                Pull();
            }
        }

        else if (_abilityList[abilityNum] == Abilities.PolarBind)
        {
            if (_currentAction == AbilityActions.None) //Ability keybind was released
            {
                Debug.Log("Bind Triggered");
                Bind();
            }
        }
    }

    private List<GameObject> GetClosest()
    {
        Vector3 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePosition.z = 0f;

        RaycastHit2D[] hits = Physics2D.CircleCastAll(_mousePosition, _selectionRange, Vector2.zero, 0, _enemyLayer);

        //Orders each enemy in selection range by their distance from the mouse, with the lower index being closer to the mouse
        List<GameObject> sortedHits = hits.OrderBy(hit => Vector3.Distance(_mousePosition, hit.collider.gameObject.transform.position)).Select(hit => hit.collider.gameObject).ToList();
            
        return sortedHits;
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

        GameObject closestEnemy = GetClosest()?[0]; 
        
        if (closestEnemy == null)
        {
            Debug.Log("No enemy found!");
            return;
        }

        if (closestEnemy.TryGetComponent(out StatsScript EnemyStats) && TryGetComponent(out StatsScript PlayerStats))
        {
            Debug.Log("Attempting to damage and slow");
            EnemyStats.Health -= _stats.Damage(_trapDamage, "ability");
            EnemyStats.Slow(_trapDuration, 1f);

        }
        else
        {
            Debug.Log($"Either the player or {closestEnemy.gameObject.name} doesnt have a stats script");  
        }


    }

    private void Pull() //Polar Pull in GDD
    {
        GameObject closestEnemy = GetClosest()?[0]; 

        if (closestEnemy == null)
        {
            return;    
        }

        if (closestEnemy.TryGetComponent(out Rigidbody2D _enemyRB2D))
        {
            Vector3 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mousePosition.z = 0f;
            Vector3 _mouseDirection = (_mousePosition - closestEnemy.transform.position).normalized;
            _enemyRB2D.AddForce(_mouseDirection * 1000f); //Change 1000f to a variable later
            if (closestEnemy.TryGetComponent(out StatsScript EnemyStats) && TryGetComponent(out StatsScript PlayerStats))
            {
                EnemyStats.Health -= _stats.Damage(_pullDamage, "ability");
            }
            else
            {
                Debug.Log($"Either the player or {closestEnemy.gameObject.name} doesnt have a stats script");  
            }
        }
        else
        {
            Debug.LogWarning($"{closestEnemy.name} does not have a Rigidbody2D so the players abilities cannot function properly", closestEnemy);
            return;
        }
    }

    private void Bind() //Polar Bind in GDD
    {
        List<GameObject> temp = GetClosest();
        GameObject closestEnemy = temp != null && temp.Count >= 2 ? temp[0]: null; 
        GameObject secondClosestEnemy = temp != null && temp.Count >= 2 ? temp[1]: null; 

       
        if (closestEnemy == null || secondClosestEnemy == null)
        {
            return;    
        }


        if (closestEnemy.TryGetComponent(out Rigidbody2D _enemyRB2D) && secondClosestEnemy.TryGetComponent(out Rigidbody2D _enemyTwoRB2D))
        {
            //Debug.Log("EEE");
            Vector3 BindDirection = (closestEnemy.transform.position - secondClosestEnemy.transform.position).normalized;
            _enemyRB2D.AddForce(BindDirection * -1000f); //Change 1000f to a variable later (E)
            _enemyTwoRB2D.AddForce(BindDirection * 1000f); //Change 1000f to a variable later (E)
            if (closestEnemy.TryGetComponent(out StatsScript EnemyStats) && secondClosestEnemy.TryGetComponent(out StatsScript EnemyTwoStats))
            {
                EnemyStats.Health -= _stats.Damage(_bindDamage, "ability");
                EnemyTwoStats.Health -= _stats.Damage(_bindDamage, "ability");
            }
            else
            {
                Debug.Log($"Either {secondClosestEnemy.gameObject.name} or {closestEnemy.gameObject.name} doesnt have a stats script");  
            }
        }
        else
        {
            Debug.LogWarning($"{closestEnemy.name} or {secondClosestEnemy.name} does not have a Rigidbody2D so the players abilities cannot function properly", closestEnemy);
            return;
        }   
    }

    private void Orbit()
    {
        GameObject scrapOrbit = Instantiate(_orbitPrefab, transform.position, Quaternion.identity, transform); //(I)
        //scrapOrbit.transform.SetParent(transform, false);
    }
}
