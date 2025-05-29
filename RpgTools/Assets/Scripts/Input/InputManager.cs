using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{


    private PlayerInput _playerInput;
    private InputMapEnum _inputMap = InputMapEnum.Player;
    public static InputManager Instance = null;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        if (Instance == null) Instance = this;
        else Destroy(this);
    }


    private void Start()
    {
       
    }

    public void ChangeMapping(InputMapEnum inputMap)
    {
        string name = "";
        switch (inputMap) 
        {
            case InputMapEnum.Player:
                name = "Player";
                break;
            case InputMapEnum.Dialog:
                name = "Dialog";
                break;
            case InputMapEnum.CharacterSelection:
                name = "CharacterSelection";
                break;
            case InputMapEnum.Battle:
                name = "Battle";
                break;
            default:
                name = "Player";
                break;
        }
        _playerInput.SwitchCurrentActionMap(name);
        _inputMap = inputMap;
    }

    public InputMapEnum GetInputMap()
    {
        return _inputMap;
    }
}

