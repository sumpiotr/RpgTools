using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private StringEventScriptableObject changeMapInputEvent;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        changeMapInputEvent.AddEvent(ChangeMapping);
    }


    private void Start()
    {
       
    }

    private void ChangeMapping(string name)
    {
        _playerInput.SwitchCurrentActionMap(name);
    }
}
