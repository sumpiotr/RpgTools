using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Vector2EventScriptableObject changePositionEvent;

    private Rigidbody2D _rigidbody2D;

    private SpriteRenderer _spriteRenderer;

    private Vector2 _direction;

    private Vector2 _velocity;


    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private Sprite front;
    [SerializeField] 
    private Sprite back;

    [SerializeField]
    private Sprite side;

    public static PlayerController Instance = null;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    void Start()
    {
        _rigidbody2D= GetComponent<Rigidbody2D>();
        _spriteRenderer= GetComponent<SpriteRenderer>();
        _direction = Vector2.up;

        if (changePositionEvent != null) changePositionEvent.AddEvent(UpdatePosition);
    }

    public void UpdatePosition(Vector2 position)
    {
        transform.position = position;
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    private void Update()
    {
        _rigidbody2D.linearVelocity = _velocity;
    }

    public void Movement(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();
        if (movement == Vector2.zero) {
            _velocity = Vector2.zero;
            //_rigidbody2D.linearVelocity = Vector2.zero;
            return;
        }

        if (Mathf.Abs(movement.y) > Mathf.Abs(movement.x)) 
        {
            if(movement.y > 0)
            {
                _direction = Vector2.up;
                _spriteRenderer.sprite = back;
            }
            else
            {
                _direction = Vector2.down;
                _spriteRenderer.sprite = front;
            }
        }
        else
        {
            _spriteRenderer.sprite = side;
            if (movement.x > 0)
            {
                _direction = Vector2.right;
                transform.localScale = new Vector3(1, 1, 1);
            }
            else 
            {
                _direction = Vector2.left;
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        _velocity = _direction*speed;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        int environmentLayer = LayerMask.NameToLayer("Environment");
        int ignoreEnvironmentMask = ~(1 << environmentLayer);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, 1.5f, ignoreEnvironmentMask);
        if (hit)
        {
           InteractableObject interactableObject = hit.collider.GetComponent<InteractableObject>();
            if (interactableObject == null) return;
            interactableObject.OnInteraction(gameObject);
        }
    }
}
