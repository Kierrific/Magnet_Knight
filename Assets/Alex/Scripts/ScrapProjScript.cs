
using UnityEngine;




[RequireComponent(typeof(Rigidbody2D))] //Needs a Rigid Body 2D
public class ScrapProjScript : MonoBehaviour
{

    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _despawnTime;
    [SerializeField] private int _damage;
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.TryGetComponent(out StatsScript EnemyStats))
            {
                EnemyStats.Health -= _damage;
            }
            else
            {
                Debug.LogWarning($"{collision.gameObject.name} does not have a stats script, attach one or game will not function as desired!");
                //Debug.Break();
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
