using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Vector2EventScriptableObject changePositionEvent;

    private Rigidbody2D _rigidbody2D;

    private Vector2 _direction;

    private Vector2 _velocity;


    [SerializeField]
    private float speed = 2f;
    
    void Start()
    {
        _rigidbody2D= GetComponent<Rigidbody2D>();
        _direction = Vector2.up;

        if (changePositionEvent != null) changePositionEvent.AddEvent(UpdatePosition);
    }

    private void UpdatePosition(Vector2 position)
    {
        transform.position = position;
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
            _direction = movement.y > 0 ? Vector2.up : Vector2.down;
        }
        else
        {
            _direction = movement.x > 0 ? Vector2.right : Vector2.left;
        }
        _velocity = _direction*speed;
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, _direction, 1.5f);
        if (hit)
        {
           InteractableObject interactableObject = hit.collider.GetComponent<InteractableObject>();
            if (interactableObject == null) return;
            interactableObject.OnInteraction(gameObject);
        }
    }
}
