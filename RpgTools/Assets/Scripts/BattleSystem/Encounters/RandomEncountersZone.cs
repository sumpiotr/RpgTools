using System.Collections.Generic;
using UnityEngine;

public class RandomEncountersZone : MonoBehaviour
{

    [SerializeField]
    private float minEncounterWaitTime;

    [SerializeField]
    private float maxEncounterWaitTime;

    [SerializeField]
    private List<EncounterScriptableObject> encounters;

    private float waitTime;
    private float counter;

    private float GetWaitTime()
    {
        return Random.Range(minEncounterWaitTime, maxEncounterWaitTime);
    }

    private EncounterScriptableObject GetRandomEncounter()
    {
        return encounters[Random.Range(0, encounters.Count)];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        waitTime = GetWaitTime();
        counter = 0;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!PlayerController.Instance.IsMoving()) return;
        counter += Time.deltaTime;
        Debug.Log(counter);
        if(counter >= waitTime)
        {
            counter = 0;
            BattleManager.Instance.LoadBattle(GetRandomEncounter());
        }
    }
}
