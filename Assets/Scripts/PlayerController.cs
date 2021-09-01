using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2.5f;
    
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    
    public float jumpForce = 5f;

    public float longIdleTime = 5f;
    
    private Rigidbody2D _body;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private InstantiatePrefab _instantiatePrefab;
    private Spawner _spawner;

    private Vector2 _movement;

    private bool _facingRight = true;
    private bool _grounded;

    private float _longIdleTimer;
    
    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _instantiatePrefab = GetComponent<InstantiatePrefab>();
        _spawner = GetComponent<Spawner>();
    }

    // Update is called once per frame
    private void Update()
    {
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        _movement = new Vector2(horizontalInput, 0);

        if (horizontalInput < 0 && _facingRight)
        {
            FlipImage();
        }
        else if (horizontalInput > 0 && !_facingRight)
        {
            FlipImage();
        }

        _grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetButtonDown("Jump") && _grounded)
        {
            _body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            _instantiatePrefab.Instantiate();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _animator.SetTrigger("Attack");
        }
    }

    private void FixedUpdate()
    {
        var horizontalNormalizedX = _movement.normalized.x * speed;
        _body.velocity = new Vector2(horizontalNormalizedX, _body.velocity.y);
    }

    private void LateUpdate()
    {
        _animator.SetBool("Idle", _movement == Vector2.zero);
        _animator.SetBool("Grounded", _grounded);
        _animator.SetFloat("VerticalVelocity", _body.velocity.y);
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            _longIdleTimer += Time.deltaTime;

            if (_longIdleTimer >= longIdleTime)
            {
                _animator.SetTrigger("LongIdle");
            }
        }
        else
        {
            _longIdleTimer = 0;
        }
    }

    private void FlipImage()
    {
        _facingRight = !_facingRight;

        var localScale = transform.localScale;
        transform.localScale = new Vector3(localScale.x * -1, localScale.y, localScale.z);
        // _spriteRenderer.flipX = GetFlipX();
        _spawner.direction = _facingRight ? Vector2.right : Vector2.left;
    }

    private bool GetFlipX()
    {
        return !_facingRight;
    }
}
