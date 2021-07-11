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
    private Vector3 moveDirection;
    
    [Header("Emotes")]
    public bool shouldEmote;
    private bool emoteTick;
    public GameObject[] emotes;
    public float timeBetweenEmotes;

    [Header("Info")]
    public string namePlate;
    public int threat, threatMin, threatMax;

    [Header("Tools")]
    public Rigidbody theRB;
    public Animator anim;
    public SpriteRenderer theBody;
    public Transform emotePoint;
 
    [Header("Bools")]
    public bool shouldIdle;
    public bool shouldChase;
    public bool shouldWander;
    public bool shouldShoot;
    public bool shouldGib;
    public bool shouldDropItems;

    //States
    [Header("Idle")]
    //public bool blockWhenIdle;
    //private bool willBlock;
    public int idleLength;
    private bool idleCounter,idleTick;

    [Header("Chase")]
    public float rangeToChase;

    [Header("Wander")]
    //private bool wanderTick = false; Unused Currently
    public float wanderLength;
    private float wanderCounter,wanderTick;
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
        wanderCounter -= Time.deltaTime;
    }

/*--------------------------------------------------------*/
/*----------------------Functions-------------------------*/

    public void Emote(int emotion)
    {
        Instantiate(emotes[emotion], emotePoint.position, emotePoint.rotation);
    }

    public void Move()
    {
        theRB.velocity = moveDirection * moveSpeed;
        moveDirection.Normalize();
    }
    
    /*public void Emote();
    {
        Need to make.
    }*/

    void Update()
    {
    
    /*--------------------------------------------------------*/
    /*-----------------Switches and Logic---------------------*/
        switch(curState)
        {

            case State.Idle:
            
                if(shouldIdle)
                {
                    //FUTURE: Pick from list of idle animations and tweak variables
                    //during said animation. Movespeed 0 when sitting, interacting, ect...
                    
                    idle
                    
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

                    wanderCounter -= Time.deltaTime;
                    moveDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

                    if(wanderCounter =< 0)
                    {
                        curState = State.Idle;
                        
                        //Need to link Wander and Idle instead of pauseCounter.
                        //Set chance to wander or ("Say something") \
                        //Also work on visual EMOTES when a new state is entered.
                        //July 10th
                    
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
        
        timeBetweenEmotes -= Time.DeltaTime;
        
        if(timeBetweenEmotes =< 0 && !shouldEmote)
        {
            shouldEmote = true;
        }
        
        
    }
}
