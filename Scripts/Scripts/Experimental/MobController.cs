using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobController : MonoBehaviour
{
    public static MobController instance;

    //Tools
    public Rigidbody2D theRB;
    public Animator anim;
    public SpriteRenderer theBody;

    //Stats
    public int health = 15;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
