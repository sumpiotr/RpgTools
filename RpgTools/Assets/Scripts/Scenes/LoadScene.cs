using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private SceneAsset scene;

    [SerializeField]
    private Vector2 enterPosition;

    private LoadSceneEventScriptableObject _changeSceneEvent;

    private SceneLoadData _loadData;

    private void Start()
    {
        _changeSceneEvent = Resources.Load<LoadSceneEventScriptableObject>("Events/LoadScene");
        _loadData = new SceneLoadData(enterPosition, scene.name, Vector2.zero);
    }

    public void LoadNewScene(Vector2 exitPosition)
    {
        _loadData.exitPosition = exitPosition;
        _changeSceneEvent.CallEvent(_loadData);
    }
}
