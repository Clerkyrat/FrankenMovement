using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMob : MonoBehaviour
{
    public int damage;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Mob")
        {
            EnemyController.instance.damageEnemy(damage);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.tag == "Mob")
        {
            EnemyController.instance.damageEnemy(damage);
        }
    }

    private void OnTCollisionEnter2D(Collision other)
    {
        if(other.gameObject.tag == "Mob")
        {
            EnemyController.instance.damageEnemy(damage);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.tag == "Mob")
        {
            EnemyController.instance.damageEnemy(damage);
        }
    }
}

