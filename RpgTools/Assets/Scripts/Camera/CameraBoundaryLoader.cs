using UnityEngine;

public class CameraBoundaryLoader : MonoBehaviour
{

    private Vector2 _previousSize;
    void Update()
    {
       if(Screen.width != _previousSize.x || Screen.height != _previousSize.y)
        {
            _previousSize = new Vector2(Screen.width, Screen.height);
            UpdateBoundaries();
        }
    }

    private void UpdateBoundaries()
    {
        ClampPositionRange clampPositionRange = new ClampPositionRange();
        clampPositionRange.xMin = transform.position.x - (transform.localScale.x / 2) + (Camera.main.orthographicSize * Camera.main.aspect);
        clampPositionRange.xMax = transform.position.x + (transform.localScale.x / 2) - (Camera.main.orthographicSize * Camera.main.aspect);
        clampPositionRange.yMin = transform.position.y - (transform.localScale.y / 2) + Camera.main.orthographicSize;
        clampPositionRange.yMax = transform.position.y + (transform.localScale.y / 2) - Camera.main.orthographicSize;
        CameraController.rangeBound = clampPositionRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
