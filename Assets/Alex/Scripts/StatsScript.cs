using UnityEngine;

public class StatsScript : MonoBehaviour
{
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
            _health = Mathf.Clamp(value, 0, _maxHealth);
            if (gameObject.tag == "Player")
            {
                //Handle Custom Logic Here (Edit save data)
            }


            if (_health == 0)
            {
                if (gameObject.tag == "Enemy")
                {
                    if (_player.TryGetComponent(out StatsScript PlayerStats))
                    {
                        PlayerStats.Scrap += _scrap;
                    }
                    else
                    {
                        Debug.Log("Player Doesn't Have Stats Script");
                    }
                    Destroy(gameObject);
                }
                else if (gameObject.tag == "Player")
                {
                    //UPDATE THIS LATER
                    Debug.Log("PLAYER DIED");
                    transform.position = new(0f, 0f, 0f);
                    _health = _maxHealth;
                }
            }
        }
    }

    [Tooltip("The percentage of damage mitigation 0 being none 1 being 100% mitigation")] [Range(0f, 1f)] [SerializeField] private float _defense;
    [HideInInspector] public float Defense
    {
        get { return _defense; }
        set { _defense = Mathf.Clamp(value, 0f, 1f); }
    }

    //---------------------------------------------------------------------------------------------------

    //Movement Variables
    //---------------------------------------------------------------------------------------------------
    [Tooltip("How fast the object moves")][SerializeField] private float _moveSpeed = 5f; //How fast the object moves (5 is current default)

    [HideInInspector]
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set
        {
            _moveSpeed = value;
        }
    }
    //---------------------------------------------------------------------------------------------------

    //Combat Variables
    //---------------------------------------------------------------------------------------------------
    [Header("Generic Attack Values")]
    [Tooltip("How fast the object can attack")][SerializeField] private float _attackSpeed = .5f;
    [HideInInspector]
    public float AttackSpeed
    {
        get { return _attackSpeed; }
        set
        {
            _attackSpeed = value;
        }
    }

    [Tooltip("The difficulty scaling for damage, health, etc.")][SerializeField] private float _difficultyScaler = 1f;
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

    [Tooltip("The amount of flat scaling melee damage gets, default being 0")][SerializeField] private int _meleeDamageBonus = 0;
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

    [Header("Block Values")]
    [Tooltip("How many attacks can the object block")][SerializeField] private int _maxBlockCharges = 5;
    [HideInInspector]
    public int MaxBlockCharges
    {
        get { return _maxBlockCharges; }
        set
        {
            _maxBlockCharges = value;
        }
    }

    [Tooltip("The rate in which the object get block charges back")][SerializeField] private float _blockChargeCooldown = 1f;
    [HideInInspector]
    public float BlockChargeCooldown
    {
        get { return _blockChargeCooldown; }
        set
        {
            _blockChargeCooldown = value;
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



    void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        if (gameObject.tag != "Player")
        {
            Health = MaxHealth;
            Debug.Log($"Health: {Health}\nMovement Speed: {MoveSpeed}");
        }
        else
        {
            //Set Health to save data 
        }




    }
}
