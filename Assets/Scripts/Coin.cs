using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    public float rotSpeed = 1.0f;

    private void Update()
    {
        transform.rotation *= Quaternion.Euler(0, rotSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameData.Instance.Score++;
            Destroy(gameObject);
        }
    }
}
