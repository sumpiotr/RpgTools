using UnityEngine;

public class LoadScene : MonoBehaviour
{
    [SerializeField]
    private string scene;

    [SerializeField]
    private Vector2 enterPosition;

    [SerializeField]
    private Vector2 direction = Vector2.zero;

    private LoadSceneEventScriptableObject _changeSceneEvent;

    private SceneLoadData _loadData;


    private void Start()
    {
        _changeSceneEvent = Resources.Load<LoadSceneEventScriptableObject>("Events/LoadScene");
        _loadData = new SceneLoadData(enterPosition, scene, Vector2.zero);
    }

    public void LoadNewScene(Vector2 exitPosition)
    {
        _loadData.exitPosition = direction;
        _changeSceneEvent.CallEvent(_loadData);
    }
}
