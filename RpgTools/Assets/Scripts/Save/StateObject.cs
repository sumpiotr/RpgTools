using UnityEngine;
using UnityEngine.SceneManagement;

public class StateObject : MonoBehaviour
{

    private string _sceneName;

    public void SetState(bool active)
    {
        ObjectsStateManager.Instance.SaveState(name, active);
        gameObject.SetActive(active);
    }
}
