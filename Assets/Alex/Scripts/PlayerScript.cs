using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
[RequireComponent(typeof(Rigidbody2D))] //Needs a Rigid Body 2D
[RequireComponent(typeof(StatsScript))] //Needs a stats script
public class PlayerScript : MonoBehaviour
{
    //Serialize Field Variables
    [Tooltip("Drag the players RigidBody2D into this variable (Should function without setting it here maybe)")] [SerializeField] private Rigidbody2D _rb2d;
    [Tooltip("The list of the various projectiles that the enemy can fire (Small - Medium - Large")] [SerializeField] private List<GameObject> _scrapProjectilePrefabs;
    [Tooltip("How long in seconds till the player can dash again before scaling of cooldown reduction")][SerializeField] private float _dashCooldown;
    [Tooltip("")] [SerializeField] private  
    [Tooltip("How large the players melee attack hits")] [SerializeField] private float _meleeRadius;

    //Layers
    [Tooltip("The layer for enemies, should define its self in script but still should change this to be sure")] [SerializeField] private LayerMask _enemyLayer;
    [Tooltip("The layer for the environment, should define its self in script but still should change this to be sure")] [SerializeField] private LayerMask _groundLayer;


    //Attack Variables
    private bool _mainAttackPressed;
    private bool _secondAttackPressed;
    private bool _abilityPressed;
    private float _dashTimer;
    private float _attackTimer;  
    private string _playerBusy = "none"; //Use this variable to check if another action is current being acted
    
    

    //Movement Variables
    private Vector2 _playerMovement;
    private Vector2 _playerDirection;
    private bool _canMove = true;
    private bool _dashing = false;

    //Mouse Variables
    private Vector3 _mousePosition;





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
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

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
