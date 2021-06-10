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

    [Header("GFX")]
    private Vector3 moveDirection;

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

/*--------------------------------------------------------*/
/*-----------------Switches and Logic---------------------*/

    void Update()
    {
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
