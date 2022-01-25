using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public AudioSource hitSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (hitSound != null)
                hitSound.Play();

        }
    }

}
