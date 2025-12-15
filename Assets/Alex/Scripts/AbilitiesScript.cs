using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
//using UnityEditor.ShaderGraph.Internal;

//Make a function in statsscript similar to slow but for pushing enemy like a lerp would and use in pull and bind
//For Bind make it actually move the enemies
//Make it so that when ever you are about to add something to the list of selected enemies you check a variable for the causes


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
    [Tooltip("Set this to the prefab of the wave attack game object")] [SerializeField] private GameObject _wavePrefab;
    [Tooltip("Set this to the prefab of the black hole attack game objec")][SerializeField] private GameObject _holePrefab; 
    [Tooltip("The amount of time you can press an ability with the time remaining and have it still activative. (Like Jump Buffering but for abilities)")] [SerializeField] private float _bufferAmount = .25f;
    [Tooltip("Set this to the Sprite Renderer of the player.")] [SerializeField] private SpriteRenderer _playerSpriteRenderer;


    [Header("Ability Base Stats")]
    [Tooltip("The amount of base damage the Magnetic Trap ability does.")] [SerializeField] private int _trapDamage = 1;
    [Tooltip("How long the enemy stays trapped for in seconds.")] [SerializeField] private float _trapDuration = 3f;
    [Tooltip("How long of a cooldown Magnetic Trap has.")] [SerializeField] private float _trapCooldown = 3f;
    [Tooltip("The amount of base damage the Polar Pull ability does.")] [SerializeField] private int _pullDamage = 1;
    [Tooltip("How long the player can pull the enemy for with Polar Pull.")][SerializeField] private float _pullDuration = 3f;
    [Tooltip("How long of a cooldown Polar Pull has.")] [SerializeField] private float _pullCooldown = 10f;
    [Tooltip("The amount of base damage the Polar Bind ability does.")] [SerializeField] private int _bindDamage = 5;
    [Tooltip("How long enemies get stuck together with Polar Bind.")][SerializeField] private float _bindDuration = 2f;
    [Tooltip("How long of a cooldown Polar Bind has.")] [SerializeField] private float _bindCooldown = 10f;
    [Tooltip("How much scrap it cost to use the Polar Bind ability.")] [SerializeField] private int _bindScrap = 25;


    [Tooltip("The base amount the player heals from the Synthetic Heart ability")] [SerializeField] private int _healAmount = 3;
    [Tooltip("How often should the player heal with the Synthetic Heart ability")] [SerializeField] private float _healTimer = 0.5f;
    [Tooltip("How long of a cooldown Synthetic Heal has.")] [SerializeField] private float _healCooldown = 1f;
    [Tooltip("How much scrap it cost to use the Synthetic Heart Ability per _healTimer")][SerializeField] private int _healScrap = 3;

    [Tooltip("How long of a cooldown Wave Attack has.")] [SerializeField] private float _waveCooldown = 3f;
    [Tooltip("How much scrap it cost to use the wave ability.")] [SerializeField] private int _waveScrap = 25;

    [Tooltip("The base amount of damage the BlackHole ability does per second")][SerializeField] private int _holeDamage = 5;
    [Tooltip("How long the blackhole ability will last")][SerializeField] private float _holeDuration = 5f;
    [Tooltip("How long of a cooldown the Black Hole ability has")][SerializeField] private float _holeCooldown = 20f;
    [Tooltip("How much scrap it cost to use the blackhole ability")][SerializeField] private int _holeScrap = 75;
    [Tooltip("How large of an aoe the Black Hole ability has")][SerializeField] private float _holeRadius = 10f;







    [Header("Layers")]
    [Tooltip("The layer for enemies, should define its self in script but still should change this to be sure")] [SerializeField] private LayerMask _enemyLayer;

    private AbilityActions _currentAction = AbilityActions.None; 

    [Tooltip("The list of current player abilities.")] [SerializeField] private List<Abilities> _abilityList = new List<Abilities> {Abilities.None, Abilities.None, Abilities.None};
    private Vector3 _mousePosition;
    private bool _pulling = false;
    private float _pullTimer = 0f;
    private int _startHealth;
    private bool _healing = false; //Could likely combine a lot of these to be the same variable rather than each having their own, like _pulling
    private List<GameObject> _selectedEnemies = new List<GameObject>();
    private List<float> _abilityTimers = new List<float> {0f, 0f, 0f};


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

        if (_playerSpriteRenderer == null)
        {
            _playerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        //Sets enemy later
        if (_enemyLayer == 0)

        {
            _enemyLayer = LayerMask.GetMask("Enemy");
            
        }
        _pullTimer = _pullDuration;
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
        _abilityTimers[0] -= _abilityTimers[0] > 0f && _currentAction != AbilityActions.Ability1 ? Time.deltaTime : 0;
        _abilityTimers[1] -= _abilityTimers[1] > 0f && _currentAction != AbilityActions.Ability2 ? Time.deltaTime : 0;
        _abilityTimers[2] -= _abilityTimers[2] > 0f && _currentAction != AbilityActions.Ability3 ? Time.deltaTime : 0;

    }

    private void FixedUpdate()
    {
        if (_pulling)
        {
            if (_pullTimer <= 0f)
            {
                _pullTimer = _pullDuration;
                _pulling = false;
                return;
            }
            Pull();
            _pullTimer -= (1f / 50f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_mousePosition, _selectionRange);
        GetClosest();
    }



    public void Ability1(InputAction.CallbackContext ctx) //private List<InputAction.CallbackContext ctx> _bufferCTX; //Use this to save ctx actions //Private List<AbilityActions> _bufferActions; //
    {

        if (_currentAction != AbilityActions.None && _currentAction != AbilityActions.Ability1)
        {
            return;
        }

        if (_abilityTimers[0] > 0)
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

        if (_currentAction != AbilityActions.None && _currentAction != AbilityActions.Ability2)
        {
            return;
        }

        if (_abilityTimers[1] > 0)
        {
            return;
        }

        if (ctx.ReadValue<float>() == 1f)
        {
            _currentAction = AbilityActions.Ability2;
            HandleAbility(2);
        }
        else if (ctx.ReadValue<float>() == 0f)
        {
            _currentAction = AbilityActions.None;
            HandleAbility(2);
        }
    }

    public void Ability3(InputAction.CallbackContext ctx)
    {
        if (_currentAction != AbilityActions.None && _currentAction != AbilityActions.Ability3)
        {
            return;
        }

        if (_abilityTimers[1] > 0)
        {
            return;
        }

        if (ctx.ReadValue<float>() == 1f)
        {
            Debug.Log("TEST3");
            _currentAction = AbilityActions.Ability3;
            HandleAbility(3);
        }
        else if (ctx.ReadValue<float>() == 0f)
        {
            _currentAction = AbilityActions.None;
            HandleAbility(3);
        }
    }

    private void HandleAbility(int abilityNum) //Could likely make this take the enum instead of the index of the enum
    {
        abilityNum -= 1;
        if (_abilityList[abilityNum] == Abilities.MagnetTrap)
        {
            if (_currentAction == AbilityActions.None) //Ability keybind was released
            {
                Debug.Log("Trap Triggered");
                Trap();
                _abilityTimers[abilityNum] = _trapCooldown + _trapDuration;
            }
        }
        else if (_abilityList[abilityNum] == Abilities.PolarPull)
        {
            if (_currentAction != AbilityActions.None) //Ability keybind is pressed
            {
                _pulling = true;
                // Debug.Log("Pull Triggered");
                // Pull();
            }
            else
            {
                _pullTimer = _pullDuration;
                _pulling = false;
                _abilityTimers[abilityNum] = _pullCooldown;
            }
        }

        else if (_abilityList[abilityNum] == Abilities.PolarBind)
        {
            if (_currentAction == AbilityActions.None) //Ability keybind was released
            {
                //Debug.Log("Bind Triggered");
                StartCoroutine(Bind(abilityNum));

            }
        }
        else if (_abilityList[abilityNum] == Abilities.SyntheticHeart)
        {
            if (_currentAction != AbilityActions.None)
            {
                _healing = true;
                StartCoroutine(Heal());
            }
            else 
            {
                _healing = false;
                _abilityTimers[abilityNum] = _healCooldown;
            }
        }
        else if (_abilityList[abilityNum] == Abilities.RepulsionWave)
        {
            if (_currentAction != AbilityActions.None)
            {
                Wave(abilityNum);
            }
        }
        else if (_abilityList[abilityNum] == Abilities.MagneticBlackhole)
        {
            if (_currentAction != AbilityActions.None)
            {
                StartCoroutine(Blackhole(abilityNum));
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

    IEnumerator Heal() 
    {
        if (_stats.Health == _stats.MaxHealth)
        {
            yield break;
        }
        for (float i = 0; _stats.Scrap > 0; i += _healTimer)
        {
            if (!Mathf.Approximately(i, 0))
            {
                _stats.Health += _healAmount * (_stats.MaxHealth / 100);
                _stats.Scrap -= _healScrap;               
            }

            if (!_healing || _stats.Health == _stats.MaxHealth)
            {
                yield break;
            }
            yield return new WaitForSeconds(_healTimer);
                
        }

        yield break;
    }

    private void Trap() //Magnet Trap in the GDD //(E) Make cost 3 scrap
    {
        
        //Vector3 _mouseDirection = _mousePosition - transform.position; //Look into using a cached transform position rather than calling it (E)
        //float _mouseAngle = Mathf.Atan2(_mouseDirection.y, _mouseDirection.x) * Mathf.Rad2Deg;
        var temp = GetClosest();
        GameObject closestEnemy = temp != null && temp.Count > 0? temp[0] : null;
        //GameObject closestEnemy = GetClosest()?[0]; 
        
        if (closestEnemy == null)
        {
            return;
        }

        if (closestEnemy.TryGetComponent(out StatsScript EnemyStats) && TryGetComponent(out StatsScript PlayerStats))
        {
            EnemyStats.Slow(_trapDuration, 1f);
            EnemyStats.Health -= _stats.Damage(_trapDamage, "ability");
            

        }
        else
        {
            Debug.LogWarning($"Either the player or {closestEnemy.gameObject.name} doesnt have a stats script");  
        }


    }

    private void Pull() //Polar Pull in GDD //(E) (Cost 2 scrap per second) //This one is super jank but i'll worry about it if I get time
    {
        var temp = GetClosest();
        GameObject closestEnemy = temp != null && temp.Count > 0? temp[0] : null;

        if (closestEnemy == null)
        {
            return;    
        }

        if (closestEnemy.TryGetComponent(out Rigidbody2D _enemyRB2D))
        {   
            Vector3 _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mousePosition.z = 0f;
            Vector3 _mouseDirection = (_mousePosition - closestEnemy.transform.position).normalized;
            if (Vector3.Distance(_enemyRB2D.position, _mousePosition) > 0.25f)
            {
                _enemyRB2D.AddForce(_mouseDirection * 500f); //Change 1000f to a variable later
            }
            if (closestEnemy.TryGetComponent(out StatsScript EnemyStats) && TryGetComponent(out StatsScript PlayerStats))
            {
                EnemyStats.Slow(0.1f, 1f);
                //Debug.Log(_pullTimer % 1f);
                if (_pullTimer % 1f <= 0.02f)
                {
                    EnemyStats.Health -= _stats.Damage(_pullDamage, "ability");
                    _stats.Scrap -= 2;
                }
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

    IEnumerator Bind(int abilityNum) //Polar Bind in GDD
    {
        List<GameObject> temp = GetClosest();
        GameObject closestEnemy = temp != null && temp.Count >= 1 ? temp[0] : null;
        GameObject secondClosestEnemy = temp != null && temp.Count >= 2 ? temp[1] : null;
        bool enemyAdded = false;
        if (_stats.Scrap < _bindScrap)
        {
            yield break;
        } 
        if (secondClosestEnemy == null)
        {
            if (closestEnemy != null)
            {
                if (_selectedEnemies.Count < 2)
                {
                    if (!_selectedEnemies.Contains(closestEnemy))
                    {
                        _selectedEnemies.Add(closestEnemy);
                        Debug.Log($"Added {closestEnemy.name} to the selected enemy list");
                        if (_selectedEnemies.Count != 2)
                        {
                            yield break;
                        }
                        else
                        {
                            closestEnemy = _selectedEnemies[0];
                            secondClosestEnemy = _selectedEnemies[1];
                            enemyAdded = true;
                            _selectedEnemies = new List<GameObject>();
                        }
                    }
                    else
                    {
                        _selectedEnemies.Remove(closestEnemy);
                        
                        yield break;
                    }
                }

            }
            else
            {
                yield break;
            }
        }

        if (_selectedEnemies.Count > 0 && !enemyAdded)
        {
            secondClosestEnemy = _selectedEnemies[0];
            _selectedEnemies = new List<GameObject>();

        }

        _abilityTimers[abilityNum] = _bindCooldown + _bindDuration;
        _stats.Scrap -= _bindScrap; 
        if (closestEnemy.TryGetComponent(out Rigidbody2D _enemyRB2D) && secondClosestEnemy.TryGetComponent(out Rigidbody2D _enemyTwoRB2D))
        {
            if (closestEnemy.TryGetComponent(out StatsScript EnemyStats) && secondClosestEnemy.TryGetComponent(out StatsScript EnemyTwoStats))
            {
                if (EnemyTwoStats.Health - _stats.Damage(_bindDamage, "ability") <= 0 || EnemyStats.Health - _stats.Damage(_bindDamage, "ability") <= 0)
                {
                    EnemyStats.Health -= _stats.Damage(_bindDamage, "ability");
                    EnemyTwoStats.Health -= _stats.Damage(_bindDamage, "ability");
                    _selectedEnemies = new List<GameObject>();
                    yield break;
                }

                EnemyStats.Health -= _stats.Damage(_bindDamage, "ability");
                EnemyTwoStats.Health -= _stats.Damage(_bindDamage, "ability");
                EnemyStats.Slow(_bindDuration, 1f);
                EnemyTwoStats.Slow(_bindDuration, 1f);
                

            }
            else
            {
                Debug.LogWarning($"Either {secondClosestEnemy.gameObject.name} or {closestEnemy.gameObject.name} doesnt have a stats script");  
            }
            for (float i = 0; i < _bindDuration; i += Time.deltaTime) // ADD A CHECK TO SEE IF THE GAME OBJECT IS DESTROYED(I)
            {
                if (Vector3.Distance(closestEnemy.transform.position, secondClosestEnemy.transform.position) > 0.5f)
                {
                    Vector3 BindDirection = (closestEnemy.transform.position - secondClosestEnemy.transform.position).normalized;
                    _enemyRB2D.AddForce(BindDirection * -200f); 
                    _enemyTwoRB2D.AddForce(BindDirection * 200f); 
                }
                
                yield return null;
            }
            
        }
        else
        {
            Debug.LogWarning($"{closestEnemy.name} or {secondClosestEnemy.name} does not have a Rigidbody2D so the players abilities cannot function properly", closestEnemy);
            yield break;
;
        }   
    }

    private void Wave(int abilityNum) //Repulsion Wave in GDD 
    {
        if (_stats.Scrap < _waveScrap)
        {
            return;
        }
        _stats.Scrap -= _waveScrap;
        Vector3 projectileSpawnLocation = transform.position;
        Vector3 _mouseDirection = _mousePosition - transform.position; 
        float xLoc = projectileSpawnLocation.x + (_mouseDirection.normalized.x * (_playerSpriteRenderer.size.x / 2 + .1f)) + _mouseDirection.normalized.x * _wavePrefab.GetComponent<SpriteRenderer>().size.x / 2;
        float yLoc = projectileSpawnLocation.y + (_mouseDirection.normalized.y * (_playerSpriteRenderer.size.y / 2 + .1f)) + _mouseDirection.normalized.y * _wavePrefab.GetComponent<SpriteRenderer>().size.y / 2;
        projectileSpawnLocation = new Vector3(xLoc, yLoc, projectileSpawnLocation.z);
        Vector3 TempDirection = _mousePosition - projectileSpawnLocation;
        float TempAngle = Mathf.Atan2(TempDirection.y, TempDirection.x) * Mathf.Rad2Deg;
        GameObject waveProjectile = Instantiate(_wavePrefab, projectileSpawnLocation, Quaternion.Euler(new Vector3(0, 0, TempAngle)));
        ScrapProjScript waveScript = waveProjectile.GetComponent<ScrapProjScript>(); //(I)
        waveScript.Damage = _stats.Damage(waveScript.Damage, "ability");
        _abilityTimers[abilityNum] = _waveCooldown;
        _currentAction = AbilityActions.None;
        //HandleAbility(1);

    }

    private void Orbit()
    {
        GameObject scrapOrbit = Instantiate(_orbitPrefab, transform.position, Quaternion.identity, transform); //(I)
        //scrapOrbit.transform.SetParent(transform, false);
    }

    IEnumerator Blackhole(int abilityNum) //Blackhole in the GDD
    {
        if (_stats.Scrap < _holeScrap)
        {
            yield break;
        }
        _currentAction = AbilityActions.None;
        _stats.Scrap -= _holeScrap;
        _abilityTimers[abilityNum] = _holeCooldown;
        GameObject holePreFab = Instantiate(_holePrefab, _mousePosition, Quaternion.identity);
        int nextWholeNumber = 0;
        for (float i = 0; i < _holeDuration; i += Time.deltaTime)
        {
            RaycastHit2D[] hits = Physics2D.CircleCastAll(holePreFab.transform.position, _holeRadius, Vector2.zero, 0, _enemyLayer);
            foreach (RaycastHit2D hit in hits)
            {


                if (hit.collider.gameObject.TryGetComponent(out Rigidbody2D _enemyRB2D))
                {
                    if (Vector3.Distance(transform.position, hit.collider.gameObject.transform.position) > 1.5f)
                    {
                        Vector3 _dir = holePreFab.transform.position - hit.collider.gameObject.transform.position;
                        _enemyRB2D.AddForce(_dir * 300f);
                    }
                }
                else
                {
                    Debug.LogWarning("AAAAAAHHHHHHHHHHHHHHHHHHHHHHHHH Ojidgdjnijjiosdgnsignosngosmgonsognsgosngosngojn");
                    Debug.Break();
                }
                if (hit.collider.gameObject.TryGetComponent(out StatsScript EnemyStats) && Mathf.FloorToInt(i) > nextWholeNumber)
                {
                    EnemyStats.Health -= _stats.Damage(_holeDamage, "ability");
                    nextWholeNumber++;

                }
                
            }

                yield return null;
        }

        Destroy(holePreFab);
        yield break;
    }
}
