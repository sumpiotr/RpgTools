using UnityEngine;
using UnityEngine.SceneManagement;

public class StateObject : MonoBehaviour
{

    private string _sceneName;

    private bool _active = true;

    public void SetState(bool active)
    {
        ObjectsStateManager.Instance.SaveState(name, active);
        gameObject.SetActive(active);
        _active = active;
        Debug.Log(_active, gameObject);

    }

    private void Update()
    {
        if(!_active)Debug.Log(_active);
    }

}
