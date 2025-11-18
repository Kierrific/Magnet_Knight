using UnityEngine;

public class StatsScript : MonoBehaviour
{
    private GameObject _player;
    //Health
    //---------------------------------------------------------------------------------------------------
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
                    Debug.Log("PLAYER DIED");
                    transform.position = new(0f, 0f, 0f);
                    _health = _maxHealth;
                }
            }
        }
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

    [Tooltip("How much damage the objects's attack does")][SerializeField] private int _damageScale = 1;
    [HideInInspector]
    public int DamageScale
    {
        get { return _damageScale; }
        set
        {
            _damageScale = value;
        }
    }

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
    [Tooltip("The amount of scrap an object has")][SerializeField] private int _scrap = 3;
    [HideInInspector]
    public int Scrap
    {
        get { return _scrap; }
        set
        {
            _scrap = value;
        }
    }
    //---------------------------------------------------------------------------------------------------



    void Awake()
    {
        _player = GameObject.FindWithTag("Player");
        if (gameObject.tag != "Player" || true)
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
