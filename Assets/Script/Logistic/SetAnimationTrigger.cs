using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SetAnimationTrigger : IQuestEvent
{
    public Animator Animator;
    public string Trigger;

    public void Execute()
    {
        //Debug.LogError("Trigger is : " + Trigger);
        //Animator.SetTrigger(Trigger);
    }
}
