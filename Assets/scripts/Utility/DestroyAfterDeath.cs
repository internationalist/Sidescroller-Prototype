using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDeath : MonoBehaviour
{
    public IEnumerator Destroy(GameObject obj)
    {
        yield return new WaitForSeconds(5f);
        Destroy(obj);
    }
}
