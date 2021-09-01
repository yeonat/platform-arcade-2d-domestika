using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 2f;
    public Vector2 direction = Vector2.right;

    public float livingTime = 3f;
    
    public Color initColor = Color.white;
    public Color finalColor = Color.white;

    private float _startTime;
    private SpriteRenderer _spriteRenderer;

    public Transform spriteRotate;
    
    private float _angle;

    private Rigidbody2D _body;

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _body = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        Destroy(this.gameObject, livingTime);
        _startTime = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        _angle += Time.deltaTime * speed * 100;
        
        if (_angle > 360)
        {
            _angle = 0;
        }
        
        spriteRotate.rotation = Quaternion.Euler(0, 0, _angle);
        
        // var movement = (direction.normalized * speed) * Time.deltaTime;
        // transform.Translate(movement);

        var timeSinceStarted = Time.time - _startTime;
        var percentTageComplete = timeSinceStarted / livingTime;

        _spriteRenderer.color = Color.Lerp(initColor, finalColor, percentTageComplete);
    }

    private void FixedUpdate()
    {
        var movement = direction.normalized * speed;
        _body.velocity = movement;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponentInParent<EnemyPatrol>().AddDamage(1);
            Debug.Log("Auch!!");
            Destroy(gameObject);
        }
    }
}
