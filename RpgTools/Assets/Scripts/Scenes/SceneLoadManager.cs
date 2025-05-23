using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField]
    LoadSceneEventScriptableObject loadSceneEvent;

    [SerializeField]
    Vector2EventScriptableObject changePlayerPositionEvent;

    private string _currentScene;

    int startIndex = 0;

    private void Start()
    {
        loadSceneEvent.AddEvent(LoadScene);
        changePlayerPositionEvent.CallEvent(Vector2.zero);
        SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        _currentScene = SceneManager.GetSceneByBuildIndex(1).name;
    }

    private void LoadScene(SceneLoadData data) 
    {
        SceneManager.UnloadSceneAsync(_currentScene);
        SceneManager.LoadSceneAsync(data.name, LoadSceneMode.Additive);
        changePlayerPositionEvent.CallEvent(data.enterPosition);
        _currentScene = data.name;
    }
}
