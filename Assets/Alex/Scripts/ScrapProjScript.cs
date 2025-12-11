using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] //Needs a Rigid Body 2D
public class ScrapProjScript : MonoBehaviour
{

    [Tooltip("The speed at which the projectile travels")] [SerializeField] private float _projectileSpeed = 10f;
    [Tooltip("How long in seconds for the projectile to despawn")] [SerializeField] private float _despawnTime = 5f;
    [Tooltip("How much base damage the projectile does before scaling")] [SerializeField] private int _damage = 1;
    [Tooltip("The string name of the tag the projectile is looking for. (For example if its an enemy projectile type in Player)")] [SerializeField] private string _targetType;
    [Tooltip("Whether or not the projectile will destroyed when it hits the target")] [SerializeField] private bool _destroyOnHit = true;
    private Rigidbody2D _projRB2D;
    private SpriteRenderer _projRenderer;
    private Vector2 direction;
    [HideInInspector] public int Damage
    {
        get {return _damage;}
        set {_damage = value;}
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, _despawnTime);
        _projRB2D = GetComponent<Rigidbody2D>();
        _projRenderer = GetComponent<SpriteRenderer>();
        float angle = transform.eulerAngles.z * Mathf.Deg2Rad;
        direction = new(Mathf.Cos(angle), Mathf.Sin(angle)); 
    }

    // Update is called once per frame
    void Update()
    {

        _projRB2D.linearVelocity = direction.normalized * _projectileSpeed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(_targetType))
        {
            if (collision.gameObject.TryGetComponent(out StatsScript TargetStats))
            {
                TargetStats.Health -= _damage;
                if (_destroyOnHit)
                {
                    Destroy(gameObject);
                }

            }
            else
            {
                Debug.LogWarning($"{collision.gameObject.name} does not have a stats script, attach one or game will not function as desired!");
                Debug.Break(); //DELETE THIS DURING PRESENTATION 
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        {
            
        }
    }

    void OnDestroy()
    {
        
    }
}
