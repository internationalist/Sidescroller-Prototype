using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    public MagicProjectile magicProjectile;
    public GameObject parent;
    public void Throw()
    {
        magicProjectile.transform.position = parent.transform.position;
        magicProjectile.gameObject.SetActive(true);
        magicProjectile.LaunchProjectile(transform.forward);
    }
}
