using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public ParticleSystem traceEffect;
    public ParticleSystem explosionEffect;
    public bool throwProjectile=true;
    public float projectileSpeed = 10;
    public AudioClip effectSound;
    private Vector3 dir = Vector3.right;


    public void LaunchProjectile(Vector3 dir)
    {
        this.dir = dir.normalized;
        traceEffect.gameObject.SetActive(true);
        traceEffect.Play();
        throwProjectile = true;
        GameManager.TransitionToEventSnapShot();
        GameManager.PlayEffectSound(effectSound);
    }


    // Update is called once per frame
    void Update()
    {
        if(throwProjectile)
        {
            Vector3 position = transform.position;
            position = position + dir*projectileSpeed * Time.deltaTime;
            transform.position = position;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(throwProjectile)
        {
            traceEffect.Stop();
            traceEffect.gameObject.SetActive(false);
            throwProjectile = false;
            explosionEffect.Play();
            StartCoroutine(killThis());
        }
    }

    private IEnumerator killThis() {

        yield return new WaitForSeconds(.5f);
        GameManager.TransitionToMainSnapShot();
        this.gameObject.SetActive(false);
    }

}
