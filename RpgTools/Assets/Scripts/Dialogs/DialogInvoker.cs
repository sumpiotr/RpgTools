using UnityEngine;

public class DialogInvoker : MonoBehaviour
{
    [SerializeField]
    private TextAsset textAsset;

    private StringEventScriptableObject _dialogEvent;

    public void Start()
    {
        _dialogEvent = Resources.Load<StringEventScriptableObject>("Events/StartDialog");
    }

    public void InvokeDialog()
    {
        if(_dialogEvent != null)_dialogEvent.CallEvent(textAsset.text);
    }
}
