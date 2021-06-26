using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MobLogic : MonoBehaviour
{
    public static MobLogic instance;

    [Header("Stats")]
    public int health;
    public float moveSpeed;
    public bool shouldEmote;
    public GameObject[] emotes;

    [Header("Info")]
    public string namePlate;
    public int threat, threatMin, threatMax;

    [Header("Tools")]
    public Rigidbody theRB;
    public Animator anim;
    public SpriteRenderer theBody;
    public Transform emotePoint;

    /*[Header("GFX")]
    private Vector3 moveDirection;
    private Vector3 bodyPoint, backPoint;
    private float flipCount;
    public float flipMax =.25f;*/

    [Header("Bools")]
    public bool shouldIdle;
    public bool shouldChase;
    public bool shouldWander;
    public bool shouldShoot;
    public bool shouldGib;
    public bool shouldDropItems;

    //States
    [Header("Idle")]
    public bool blockWhenIdle;
    private bool willBlock;
    public int idleMax;
    private int idleTick;

    [Header("Chase")]
    public float rangeToChase;

    [Header("Wander")]
    //private bool wanderTick = false; Unused Currently
    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter, panicCounter;
    private Vector3 wanderDirection;

    [Header("Shoot")]
    public GameObject bullet;
    public Transform firePoint;

    [Header("Gibbing")]
    public bool shouldLeaveCorpse;
    public GameObject[] corpses;
    public GameObject hitEffect;
    public GameObject[] splatters;

    [Header("Drops")]
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    //Enums
    enum State {Idle, Chase, Wander, Patrol, Shoot};
    State curState = State.Idle;

    //Wake
    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        //GFX Flip Added 6/10. May need tweaking going from 2D to 3D
        /* CURRENTLY UNUSED
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
        flipCount = flipMax;
        backPoint = transform.position;
        */
    
    }

/*--------------------------------------------------------*/
/*----------------------Functions-------------------------*/

    public void Emote(int emotion)
    {
        Instantiate(emotes[emotion], emotePoint.position, emotePoint.rotation);
    }

    public void Move()
    {
        //In Progress. Commented to avoid errors.
        //theRB.velocity = moveDirection * moveSpeed;
        //moveDirection.Normalize();
    }

    void Update()
    {
    
/*--------------------------------------------------------*/
/*------------------------GFX Flip------------------------*/

    //Added 6/10. May need tweaking going from 2D to 3D
    //GFX Flip
    // V gets current position for reference.
    /*
    bodyPoint.x = transform.position.x;
    
    //Creates new "backpoint" every determined 
    if(flipCount > 0 )
    {
        flipCount -= Time.deltaTime;

        if(flipCount <= 0)
        {
            flipCount = .5f;
            backPoint = bodyPoint;
        }
    }
    
    if(bodyPoint.x < backPoint.x)
    {
        transform.localScale = new Vector2(scaleX, transform.localScale.y);

    }else if(bodyPoint.x > backPoint.x)
    {
         transform.localScale = new Vector2(-scaleX, transform.localScale.y);
    }
    */

/*--------------------------------------------------------*/
/*-----------------Switches and Logic---------------------*/
        switch(curState)
        {

            case State.Idle:
            
                if(shouldIdle)
                {
                    if(shouldEmote)
                    {
                        Debug.Log("I am bored");
                        //Emote(0); Unused Currently
                        shouldEmote = false;
                    }
                }
                break;
/*----------------------------------------------------------------------------*/
            case State.Chase:
                Debug.Log("Come back here!");
                break;
/*----------------------------------------------------------------------------*/
            case State.Wander:
                Debug.Log("I'm the kind of sprite, who likes to roam around");
                
                if(shouldWander)
                {
                    if(wanderCounter > 0)
                    {
                        wanderCounter -= Time.deltaTime;

                        //move the enemy
                        moveDirection = wanderDirection;
                        
                        Move()
                    }
                }
                break;
/*----------------------------------------------------------------------------*/
            case State.Patrol:
                Debug.Log("Hut, hut, hut, hut, hut, hut, hut");
                break;
/*----------------------------------------------------------------------------*/
            case State.Shoot:
                Debug.Log("Pew Pew");
                break;
        }
    }
}
