
using UnityEngine;




[RequireComponent(typeof(Rigidbody2D))] //Needs a Rigid Body 2D
public class ScrapProjScript : MonoBehaviour
{

    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _despawnTime;
    [SerializeField] private int _damage;
    private Rigidbody2D _projRB2D;
    private SpriteRenderer _projRenderer;
    private Vector3 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _projRB2D = GetComponent<Rigidbody2D>();
        _projRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {


        _projRB2D.linearVelocity = new Vector2();
    }
}
