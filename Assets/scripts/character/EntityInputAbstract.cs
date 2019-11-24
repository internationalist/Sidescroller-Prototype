using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityInputAbstract : MonoBehaviour {
    /// <summary>
    /// Input data to activate a jump action. This method will be implemented by Player input or an AI script.
    /// </summary>
    /// <returns></returns>
    public abstract bool ActivateJump();
    /// <summary>
    /// Input data to activate a crouch action. This method will be implemented by Player input or an AI script.
    /// </summary>
    /// <returns></returns>
    public abstract bool ActivateCrouch();
    /// <summary>
    /// Input data to activate a block action. This method will be implemented by Player input or an AI script.
    /// </summary>
    /// <returns></returns>
    public abstract bool ActivateBlock();
    /// <summary>
    /// Input data to activate a light attack action. This method will be implemented by Player input or an AI script.
    /// </summary>
    /// <returns></returns>
    public abstract bool ActivateLightAttack();
    /// <summary>
    /// Input data to activate a heavy attack action. This method will be implemented by Player input or an AI script.
    /// </summary>
    /// <returns></returns>
    public abstract bool ActivateHeavyAttack();
    /// <summary>
    /// Input data to obtain movement as a float. This is to map a button press or controller input. This method will be implemented by Player input or an AI script.
    /// </summary>
    /// <returns></returns>
    public abstract float MovementAmount();

    /// <summary>
    /// Input data to activate a dash movement. This method will be implemented by Player input or an AI script.
    /// </summary>
    /// <returns></returns>
    public abstract bool ActivateDash();

    public float GetDistanceToEnemy(int enemyLayer)
    {
        RaycastHit hit;
        int layerMask = 1 << enemyLayer;
        if (Physics.Raycast(transform.position + new Vector3(0, 1f), transform.right, out hit, Mathf.Infinity, layerMask))
        {
            return hit.distance;
        }
        else
        {
            return -1f;
        }
    }
}
