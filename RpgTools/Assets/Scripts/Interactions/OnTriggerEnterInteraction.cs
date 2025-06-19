using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEnterInteraction : MonoBehaviour
{
    [SerializeField]
    UnityEvent<Vector2> interactionEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        interactionEvent.Invoke((Vector2)transform.position);
    }
}
