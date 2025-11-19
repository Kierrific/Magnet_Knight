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

    //Attack Variables
    private bool _mainAttackPressed;
    private bool _secondAttackPressed;
    private bool _abilityPressed;
    private float _dashTimer;
    

    //Movement Variables
    private Vector2 _playerMovement;
    private Vector2 _playerDirection;

    //Mouse Variables
    private Vector3 _mousePosition;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>(); 
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
