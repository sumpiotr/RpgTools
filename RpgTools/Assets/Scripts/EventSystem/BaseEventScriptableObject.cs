using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(fileName = "BaseEvent", menuName = "Scriptable Objects/Base Event")]
public class BaseEventScriptableObject<T> : ScriptableObject
{
    private UnityEvent<T> objectEvent = new UnityEvent<T>();

    public void AddEvent(UnityAction<T> action)
    {
        objectEvent.AddListener(action);
    }

    public void RemoveEvent(UnityAction<T> action)
    {
        objectEvent.RemoveListener(action);
    }

    public void CallEvent(T argument)
    {
        objectEvent.Invoke(argument);
    }
}
