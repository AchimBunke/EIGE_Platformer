using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTrap : MonoBehaviour
{
    public float attackSpeed = 0.5f;
    public float projectileSpeed = 500;
    public Projectile projectile;
    public Transform muzzle;

    private bool _isTracking;
    private GameObject _target;
    private float shootTimer;




    // Start is called before the first frame update
    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        shootTimer += Time.deltaTime;
        if (_isTracking)
        {
            var tpos = _target.transform.position;
            // forward aiming
            tpos += _target.GetComponent<Rigidbody>().velocity * Time.deltaTime * (projectileSpeed * Time.deltaTime * (tpos - transform.position).magnitude);
            var newLook = ((tpos + new Vector3(0, (tpos - transform.position).magnitude/4, 0)) - transform.position).normalized;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(newLook, Vector3.up), Time.deltaTime * 100);

            if (shootTimer >= 1 / attackSpeed)
            {
                Shoot();
                shootTimer = 0;
            }
        }
    }

    protected void Shoot()
    {

        var p = Instantiate(projectile,muzzle.position,Quaternion.Euler(muzzle.forward));
        p.GetComponent<Rigidbody>().AddRelativeForce(muzzle.forward.normalized * projectileSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            _isTracking = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            _isTracking = false;
    }
}
