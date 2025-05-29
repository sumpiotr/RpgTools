using UnityEngine;

public class DialogInvoker : MonoBehaviour
{
    [SerializeField]
    private TextAsset textAsset;


    public void Start()
    {
    }

    public void InvokeDialog()
    {
        DialogManager.Instance.StartDialog(textAsset.text, () => { });
    }
}
