using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacterManager : CharacterManager
{
    public override void TriggerDash()
    {
        anim.SetTrigger("forwarddash");
    }
}
