using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("max time until destroyed")]
    public float stayAlive = 5;
    public AudioSource hitSound;

    private float alivetime = 0;


    void Update()
    {
        alivetime += Time.deltaTime;
        if (alivetime > stayAlive)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hitSound != null)
        {
            hitSound.transform.parent = transform.parent;
            hitSound.Play();
            Destroy(hitSound, hitSound.clip.length);
        }
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

}
