using UnityEngine;
using UnityEngine.Events;

public class DialogInvoker : MonoBehaviour
{
    [SerializeField]
    private TextAsset textAsset;

    [SerializeField]
    private UnityEvent onDialogEnded;


    public void Start()
    {
    }

    public void InvokeDialog()
    {
        DialogManager.Instance.StartDialog(textAsset.text, () => { onDialogEnded.Invoke(); });
    }
}
