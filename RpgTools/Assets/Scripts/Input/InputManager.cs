using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private StringEventScriptableObject changeMapInputEvent;

    private PlayerInput _playerInput;

    private void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        changeMapInputEvent.AddEvent(ChangeMapping);
    }

    private void ChangeMapping(string name)
    {
        _playerInput.SwitchCurrentActionMap(name);
    }
}
