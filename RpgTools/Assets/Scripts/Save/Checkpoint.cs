using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public void MakeCheckPoint()
    {
        CheckPointManager.Instance.MakeCheckPoint();
    }

    public void LoadCheckPoint()
    {
        CheckPointManager.Instance.LoadCheckPoint();
    }
}
