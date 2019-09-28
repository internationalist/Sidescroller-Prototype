using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : EntityInputAbstract
{
    public override bool ActivateBlock()
    {
        return Input.GetButton("Block");
    }

    public override bool ActivateCrouch()
    {
        return Input.GetButton("Crouch");
    }

    public override bool ActivateHeavyAttack()
    {
        return Input.GetMouseButtonDown(1);
    }

    public override bool ActivateJump()
    {
        return Input.GetButtonDown("Jump");
    }

    public override bool ActivateLightAttack()
    {
        return Input.GetMouseButtonDown(0);
    }

    public override float MovementAmount()
    {
        return Input.GetAxis("Horizontal");
    }
}
