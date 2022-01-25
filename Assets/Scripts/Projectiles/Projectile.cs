using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("max time until destroyed")]
    public float stayAlive = 5;
    public AudioSource hitSound;

    private float alivetime = 0;


    public virtual void Update()
    {
        alivetime += Time.deltaTime;
        if (alivetime > stayAlive)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (hitSound != null)
            {
                hitSound.Play();

                Destroy(gameObject, hitSound.clip.length);

            }
        }
    }

}
