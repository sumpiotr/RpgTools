using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    [SerializeField]
    UnityEvent<Vector2> interactionEvent;
    public void OnInteraction(GameObject caller)
    {
        interactionEvent.Invoke(caller.transform.position);
    }
}
