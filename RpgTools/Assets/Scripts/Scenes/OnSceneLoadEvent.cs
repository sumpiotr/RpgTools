using UnityEngine;
using UnityEngine.Events;

public class OnSceneLoadEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onLoad;

    private void Start()
    {
        onLoad.Invoke();
    }
}
