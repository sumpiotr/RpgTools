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

    int startIndex = 0;

    private void Awake()
    {
        loadSceneEvent.AddEvent(LoadScene);
        changePlayerPositionEvent.CallEvent(Vector2.zero);
        //SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        //_currentScene = SceneManager.GetSceneByBuildIndex(1).name;
    }


    private void LoadScene(SceneLoadData data) 
    {
        if(_currentScene != "")SceneManager.UnloadSceneAsync(_currentScene);
        SceneManager.LoadSceneAsync(data.name, LoadSceneMode.Additive);
        //changePlayerPositionEvent.CallEvent(data.enterPosition);
        PlayerController.Instance.UpdatePosition(data.enterPosition);
        _currentScene = data.name;
    }
}
