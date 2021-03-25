using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobshot : MonoBehaviour
{
    public float speed;
    private Vector3 direction;
    public float dustCount, dustMax = 1f;
    public bool shouldTrail;
    public GameObject trailDust;
    public GameObject impactEffect;

    // Start is called before the first frame update
    void Start()
    {
        direction = PlayerController.instance.transform.position - transform.position;
        direction.Normalize();
        dustCount = dustMax;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += direction * speed * Time.fixedDeltaTime;
        if(shouldTrail && dustCount <= 0)
            {
                Instantiate(trailDust, transform.position, transform.rotation);

                dustCount = dustMax;
            }else
            {
                dustCount -= Time.fixedDeltaTime;
            }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Instantiate(impactEffect, transform.position, transform.rotation);

        if(other.tag == "Player")
        {
                HealthManager.instance.DamagePlayer(); 
        }

        Destroy(gameObject);

    }

    private void OnBecameInvisible() 
    {
        Destroy(gameObject);
    }

}
