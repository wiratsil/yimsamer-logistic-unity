using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [SerializeField]
    public List<IQuestEvent> EventList = new List<IQuestEvent>();

    // Todo: create delegate 
    public void OnQuestFinished()
    {
        //do thing when event queue end
    }
}
