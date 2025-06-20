using UnityEngine;

public class CutscenePlayer : MonoBehaviour
{
    public void PlayCutscene(string name)
    {
        CutscenesManager.Instance.PlayCutscene(name);
    }
}
