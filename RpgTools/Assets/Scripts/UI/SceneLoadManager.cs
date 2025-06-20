using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField]
    LoadSceneEventScriptableObject loadSceneEvent;

    [SerializeField]
    Vector2EventScriptableObject changePlayerPositionEvent;

    private string _currentScene = "";

    private SceneLoadData _data;

    int startIndex = 0;

    public static SceneLoadManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { 
            Destroy(this);
            return;
        }
        loadSceneEvent.AddEvent(LoadScene);
        changePlayerPositionEvent.CallEvent(Vector2.zero);
        //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        //_currentScene = SceneManager.GetSceneByBuildIndex(1).name;
    }


    private void LoadScene(SceneLoadData data) 
    {
        if(_currentScene != "")SceneManager.UnloadSceneAsync(_currentScene);
        PlayerController.Instance.UpdatePosition(data.enterPosition);
        _data = data;
        SceneManager.LoadSceneAsync(data.name, LoadSceneMode.Additive);
        //changePlayerPositionEvent.CallEvent(data.enterPosition);
        _currentScene = data.name;
    }

    public void ReloadScene() {
        LoadScene(_data);
    }

    public string GetCurrentSceneName()
    {
        return _currentScene;
    }


}
