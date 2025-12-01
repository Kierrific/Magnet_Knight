using UnityEngine;

public class StatsScript : MonoBehaviour
{
    //Bonuses should be calculated last and not included in percentage scalers, and percentage scalers should be independent of each other so that increasing one doesn't increase another
    //For example the proper order of calculations for damage should look something like this:
    //Mathf.RoundToInt(Base Melee Damage + (Base Melee Damage * MeleeDamageScaler - Base Melee Damage) + (Base Melee Damage * All Damage Scalar - Base Melee Damage) + Melee Damage Bonus + All Damaage Bonus)
    //If you need this clarified later lmk

    private GameObject _player;
    //Health
    //---------------------------------------------------------------------------------------------------
    [Header("Generic Object Values")]
    [Tooltip("How much health the object has")][SerializeField] private int _maxHealth = 100; //The max health of the object (100 is current default)
    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    private int _health; //The current health of the player
    [HideInInspector]
    public int Health
    {
        get { return _health; }
        set
        {
            //Debug.Log($"Value: {value}\nHealth {_health}");

            if (value < _health) //Removing Health
            {
                //Debug.Log($"Health: {_health}\nDefense: {_defense}\nDamage: {_health - value}");
                _health = Mathf.Clamp(Mathf.RoundToInt((float)_health - (((float) _health - (float) value) * (1 - _defense))), 0, _maxHealth);
                //Debug.Log($"Updated Health: {_health}");
            }
            else //Adding Health
            {
                _health = Mathf.Clamp(value, 0, _maxHealth);
            }

            if (gameObject.tag == "Player")
            {
                //Update this later but likely just make the health variable in the save data equal _health
            }
        }
    }

    [Tooltip("The percentage of damage mitigation 0 being none 1 being 100% mitigation")] [Range(0f, 1f)] [SerializeField] private float _defense;
    [HideInInspector] public float Defense
    {
        get { return _defense; }
        set { _defense = Mathf.Clamp(value, 0f, 1f); }
    }

    [Tooltip("The percentage of a cooldown being reducted, 0 being the default cooldown value and 1 being no cooldown")][Range(0f, 1f)] [SerializeField] private float _cooldownReduction;
    [HideInInspector] public float CooldownReduction
    {
        get { return _cooldownReduction; }
        set { _cooldownReduction = Mathf.Clamp(value, 0f, 1f); }
    }

    //---------------------------------------------------------------------------------------------------

    //Movement Variables
    //---------------------------------------------------------------------------------------------------
    private float _maxMoveSpeed;
    [Tooltip("How fast the object moves")][SerializeField] private float _moveSpeed = 5f; //How fast the object moves (5 is current default)

    [HideInInspector]
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set
        {
            _moveSpeed = value;
            _maxMoveSpeed = value;
        }
    }
    //---------------------------------------------------------------------------------------------------

    //Combat Variables
    //---------------------------------------------------------------------------------------------------
    [Header("Generic Attack Values")]
    [Tooltip("How fast the object can attack")][SerializeField] [Range(0f, 1f)] private float _attackSpeedBonus = .5f; //Making this a percentage scaling and having attacks cooldown specified in their respective scripts to make cooldowns no longer unified 
    [HideInInspector]
    public float AttackSpeedBonus
    {
        get { return _attackSpeedBonus; }
        set
        {
            _attackSpeedBonus = Mathf.Clamp(value, 0f, 1f);
        }
    }

    [Tooltip("The difficulty scaling for damage, health, etc. Default value is 1")][SerializeField] private float _difficultyScaler = 1f;
    [HideInInspector] public float DifficultyScaler
    {
        get { return _difficultyScaler; }
        set
        {
            _difficultyScaler = value;
        }
    }

    [Header("Melee Damage")]
    [Tooltip("The amount of percentage scaling melee damage gets, default being 1")][SerializeField] private float _meleeDamageScaler = 1f;
    [HideInInspector] public float MeleeDamageScaler
    {
        get { return _meleeDamageScaler; }
        set
        {
            _meleeDamageScaler = value;
        }
    }

    [Tooltip("The amount of flat scaling melee damage gets, default being 0")][SerializeField] private int _meleeDamageBonus = 0; //Bonuses should be calculated last and not included in percentage scalers
    [HideInInspector]
    public int MeleeDamageBonus
    {
        get { return _meleeDamageBonus; }
        set
        {
            _meleeDamageBonus = value;
        }
    }




    [Header("Range Damage")]
    [Tooltip("The amount of percentage scaling ranged damage gets, default being 1")][SerializeField] private float _rangeDamageScaler = 1f;
    [HideInInspector]
    public float RangeDamageScaler
    {
        get { return _rangeDamageScaler; }
        set
        {
            _rangeDamageScaler = value;
        }
    }


    [Tooltip("The amount of flat scaling range damage gets, default being 0")][SerializeField] private int _rangeDamageBonus = 0;
    [HideInInspector]
    public int RangeDamageBonus
    {
        get { return _rangeDamageBonus; }
        set
        {
            _rangeDamageBonus = value;
        }
    }

    [Header("Ability Damage")]
    [Tooltip("The amount of percentage scaling ability damage gets, default being 1")][SerializeField] private float _abilityDamageScaler = 1f;
    [HideInInspector]
    public float AbilityDamageScaler
    {
        get { return _abilityDamageScaler; }
        set
        {
            _abilityDamageScaler = value;
        }
    }

    [Tooltip("The amount of flat scaling melee damage gets, default being 0")][SerializeField] private int _abilityDamageBonus = 0;
    [HideInInspector]
    public int AbilityDamageBonus
    {
        get { return _abilityDamageBonus; }
        set
        {
            _abilityDamageBonus = value;
        }
    }

    [Header("All Damage")]
    [Tooltip("The amount of percentage scaling all damage gets, default being 1")][SerializeField] private float _allDamageScaler = 1f;
    [HideInInspector]
    public float AllDamageScaler
    {
        get { return _allDamageScaler; }
        set
        {
            _allDamageScaler = value;
        }
    }

    [Tooltip("The amount of flat scaling melee damage gets, default being 0")][SerializeField] private int _allDamageBonus = 0;
    [HideInInspector]
    public int AllDamageBonus
    {
        get { return _allDamageBonus; }
        set
        {
            _allDamageBonus = value;
        }
    }



    //---------------------------------------------------------------------------------------------------

    //Scrap Variables
    //---------------------------------------------------------------------------------------------------
    [Header("Scrap Values")]
    [Tooltip("The amount of scrap an object has")][SerializeField] private int _scrap = 3;
    [HideInInspector]
    public int Scrap
    {
        get { return _scrap; }
        set
        {
            _scrap = Mathf.Clamp(value, 0, _maxScrap);
        }
    }

    [Tooltip("The amount of scrap an object has")][SerializeField] private int _maxScrap = 100;
    [HideInInspector]
    public int MaxScrap
    {
        get { return _maxScrap; }
        set
        {
            _maxScrap = value;
        }
    }
    //---------------------------------------------------------------------------------------------------

    //Slow Variables
    //---------------------------------------------------------------------------------------------------
        private float _slowDuration;
        private float _slowAmount;
        private bool _slowActive = true; 
    //---------------------------------------------------------------------------------------------------


    public int Damage(int damage, string type)
    {
        int calculatedDamage = 0;
        if (type == "melee")
        {
           calculatedDamage = Mathf.RoundToInt(damage + (damage * _meleeDamageScaler - damage) + (damage * _allDamageScaler - damage) + _meleeDamageBonus + _allDamageBonus);
        }
        else if (type == "range")
        {
            calculatedDamage = Mathf.RoundToInt(damage + (damage * _rangeDamageScaler - damage) + (damage * _allDamageScaler - damage) + _rangeDamageBonus + _allDamageBonus);
        }
        else if (type == "ability")
        {
            calculatedDamage = Mathf.RoundToInt(damage + (damage * _abilityDamageScaler - damage) + (damage * _allDamageScaler - damage) + _abilityDamageBonus + _allDamageBonus);
        }
        else
        {
            Debug.Log("Damage Calculation Type Input Is Wrong");
        }

        if (gameObject.tag == "Enemy") //If object doing the damage is an enemy, then scale with _difficultyScaler
        {
            calculatedDamage += Mathf.RoundToInt(damage * _difficultyScaler) - damage;
        }

        return calculatedDamage;
    }

    public void Slow(float duration, float slowAmount)
    {
        if (slowAmount > 1.1f)
        {
            Debug.LogWarning($"Tried to slow {gameObject.name} for more than 100%, attempted value was {slowAmount * 100f}.");
            slowAmount = 1f;
        }

        if (_slowActive)
        {
            //Debug.Log("SLOW IS ALREADY ACTIVE");
            if (slowAmount > _slowAmount || Mathf.Approximately(slowAmount, _slowAmount)) //Reapplies the slow at a higher or equal potency 
            {
                _slowAmount = slowAmount;
                _slowDuration = duration;
                _moveSpeed = _maxMoveSpeed * (1 - _slowAmount);
            }
            else
            {
                //Instead of completeling ignoring the lower potency slow, add time to the current slow based on the ratio of lower potency slow to higher potency slow 
                _slowDuration += slowAmount / _slowAmount * duration;
            }
        }
        else
        {
            _slowActive = true;
            _slowAmount = slowAmount;
            _slowDuration = duration;
            _moveSpeed = _maxMoveSpeed * (1 - _slowAmount);
        }
    }

    void Awake()
    {
        _maxMoveSpeed = _moveSpeed;
        //_player = GameObject.FindWithTag("Player");
        if (gameObject.tag != "Player")
        {
            if (gameObject.tag == "Enemy") //If object doing the damage is an enemy, then scale with _difficultyScaler
            {
                MaxHealth = Mathf.RoundToInt(_difficultyScaler * MaxHealth);
            }
            _health = _maxHealth;

            
            
            //Debug.Log($"Health: {Health}\nMovement Speed: {MoveSpeed}");
        }
        else
        {
            _player = gameObject;
            _health = _maxHealth;
            //Set Health to save data 
        }
    }

    private void Start()
    {

    }

    void Update()
    {
        if (_slowActive)
        {
            _slowDuration -= Time.deltaTime;
            if (_slowDuration <= 0f)
            {
                _slowActive = false;
                _moveSpeed = _maxMoveSpeed;

            }
        }
    }

   
}
