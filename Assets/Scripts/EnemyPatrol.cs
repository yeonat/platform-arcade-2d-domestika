using System;
using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 2f;
    public float wallAware = .5f;
    public LayerMask groundLayer;
    public float playerAware = 3f;
    public float aimingTime = .5f;
    public float shootingTime = 1.5f;

    public AudioClip deadAudio;
    public AudioClip attackAudio;

    private Rigidbody2D _body; 
    private Animator _animator;
    private AudioSource _audioSource;
    
    //public float xMin;
    //public float xMax;
    //public float waitingTime = 2f;

    public GameObject dust;
    private GameObject _target;

    private Vector2 _movement;
    private bool _facingRight;
    private bool _attacking;

    private PlayerHealth _playerHealth;

    // Start is called before the first frame update

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (transform.localScale.x < 0f)
        {
            _facingRight = false;
        }
        else if (transform.localScale.x > 0f)
        {
            _facingRight = true;
        }
        
        // UpdateTransform();
        // StartCoroutine(PatrolToTarget());
    }

    // Update is called once per frame
    private void Update()
    {
        var direction = Vector2.right;

        if (!_facingRight)
        {
            direction = Vector2.left;
        }

        if (!_attacking)
        {
            if (Physics2D.Raycast(transform.position, direction, wallAware, groundLayer))
            {
                Flip();
            }
        }
    }

    private void FixedUpdate()
    {
        var horizontalVelocity = speed;

        if (!_facingRight)
        {
            horizontalVelocity = horizontalVelocity *= -1;
        }

        _body.velocity = new Vector2(horizontalVelocity, _body.velocity.y);
    }

    private void LateUpdate()
    {
        _animator.SetBool("Idle", _body.velocity == Vector2.zero);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!_attacking && other.CompareTag("Player"))
        {
            _playerHealth = other.GetComponent<PlayerHealth>();
            // SendMessage("AddDamage", 1);
            StartCoroutine(AimAndShoot());
            Debug.Log("RAAAAW!");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _playerHealth = null;
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        var localScaleX = transform.localScale.x;
        transform.localScale = new Vector3(localScaleX * -1, transform.localScale.y, transform.localScale.z);
    }

    private IEnumerator AimAndShoot()
    {
        var speedBackup = speed;
        speed = 0;

        _attacking = true;

        // yield return new WaitForSeconds(aimingTime);
        
        _animator.SetTrigger("Attack");
        _audioSource.clip = attackAudio;
        _audioSource.Play();
        
        yield return new WaitForSeconds(shootingTime);

        _attacking = false;
        speed = speedBackup;
    }

    public void AddDamage(int amount)
    {
        _audioSource.clip = deadAudio;
        _audioSource.Play();
        GetComponent<InstantiatePrefab>().Instantiate();
        gameObject.SetActive(false);
    }

    /*private void UpdateTransform()
    {
        if (_target == null)
        {
            //_target = new GameObject("Target");
            //_target.transform.position = new Vector2(xMin, transform.position.y);
            
            _target = new GameObject("Target")
            {
                transform =
                {
                    position = new Vector2(xMin, transform.position.y),
                }
            };
            
            transform.localScale = new Vector3(-1f, 1f, 1f);
            
            return;
        }

        if (Mathf.Approximately(_target.transform.position.x,xMin))
        {
            _target.transform.position = new Vector2(xMax, transform.position.y);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (Mathf.Approximately(_target.transform.position.x,xMax))
        {
            _target.transform.position = new Vector2(xMin, transform.position.y);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private IEnumerator PatrolToTarget()
    {
        while (Vector2.Distance(transform.position, _target.transform.position) > 0.05f)
        {
            _animator.SetBool("Idle", false);
            
            Vector2 direction = _target.transform.position - transform.position;
            var directionX = direction.x;
            
            transform.Translate(direction.normalized * speed * Time.deltaTime);

            yield return null;
        }

        transform.position = new Vector2(_target.transform.position.x, transform.position.y);
        
        _animator.SetBool("Idle", true);
        _animator.SetTrigger("Attack");
        
        yield return new WaitForSeconds(waitingTime);
        
        UpdateTransform();

        StartCoroutine(PatrolToTarget());
    }*/

    public void DoAttack()
    {
        if (_playerHealth != null)
        {
            _playerHealth.AddDamage(1);
        }
        
        Debug.Log("Attack");
        dust.SetActive(true);
    }
}
