using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MobLogic : MonoBehaviour
{
    public static MobLogic instance;

    [Header("Info")]
    public bool showDebug;
    public string namePlate;
    public List<string> nameDatabase = new List<string>();
    public int serialID;
    

    [Header("Stats")]
    public float moveSpeed;
    private float curSpeed;
    public int endurance;
    private int curEndurance;

    [Header("Relations")]
    public List<GameObject> friendsList;
    public int maxFriends;
    public string foe;
    public string friend;
    //private bool sameName;

    [Header("Threat and Morale")]
    public int moraleBase;
    private int curMorale;
    public int thretBase;
    private int curThreat;

    [Header("Movement")]
    private Vector3 moveDirection;
    private Vector3 wanderDirection;
    private Vector3 homePoint;

    [Header("Tools")]
    public Rigidbody theRB;
    public Animator anim;
    public SpriteRenderer theBody;
    public Transform emotePoint;
    public GameObject beacon;

    [Header("Emotes")]
    public GameObject[] emotions;
    public float timeBetweenEmotes;
    private float emoteCounter;
    private bool emoteTick;


    [Header("Bools")]
    public bool shouldIdle;
    public bool shouldCharge;
    public bool shouldWander;
    public bool shouldShoot;
    public bool shouldMelee;
    public bool shouldGib;
    public bool shouldDropItems;
    public bool shouldEmote;

    //Ticks
    private bool idleTick;
    private bool wanderTick;

/*-------------------------STATES--------------------------*/

    [Header("Ready")]
    public GameObject mfToKill;
    private float mfRange;
    public float rangeToChase;
    private bool hasTarget;

    [Header("Idle")]
    public float idleLength;
    private float idleCounter;

    [Header("Wander")]
    public float wanderLength;
    private float wanderCounter;

    [Header("Shoot")]
    public GameObject bullet;
    public Transform firePoint;

    [Header("Melee")]
    public float meleeTimeLength;
    private float meleeCounter;

    [Header("Charge")]
    public float chargeLength;
    private float chargeCounter;
    
    [Header("Gibbing")]
    public bool shouldLeaveCorpse;
    public GameObject hitEffect;
    public GameObject[] splatters;

    [Header("Drops")]
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    [Header("Sprites")]
    public Sprite[] bodies;
    public int curSpriteCount;
    private Sprite curSprite;

    //Enums
    enum State {Ready, Idle, Wander, Patrol, Shoot, Melee, Charge};
    enum Bump {Friend, Foe, Building}
    State curState = State.Idle;
    State lastState;

    //Wake
    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        //Initialize ID
        Identify();

        //Timers
        wanderCounter = wanderLength;
        emoteCounter = timeBetweenEmotes;
        curSpeed = moveSpeed;
        meleeCounter = meleeTimeLength;
        idleCounter = idleLength;

        //Bools
        if(shouldWander) { wanderTick = true; }
        if(shouldIdle) { idleTick = true; }
            
        //Time between emote icons and logs apearing
        if (shouldEmote) { emoteTick = true; }

        //Starting appearance of mob
        curSpriteCount = 0;

        //Relationships
        //sameName = false;

        //Starting position
        homePoint = transform.position;
            
    }

/*--------------------------------------------------------*/
/*----------------------Functions-------------------------*/

    public void Identify()
    {
        int randName = Random.Range(1, nameDatabase.Count);
        serialID = Random.Range(100, 999);
        
        if(namePlate == null)
        {
            namePlate = nameDatabase[randName];
        }

        if(showDebug) 
        {   
            Debug.Log(string.Format("My name is {0}, my Serial # is {1} and I am at {2}", namePlate, serialID, transform.position));
        }

    }

    public void Emote(int emotion)
    {   
        if(shouldEmote && emoteTick)
        {
            Instantiate(emotions[emotion], emotePoint.transform.position, emotePoint.transform.rotation);
            shouldEmote = false;
        }else{ if(showDebug) { Debug.Log(string.Format("{0} Quiet", namePlate)); } }
    }

    public void FriendBump()
    {
        if(showDebug) { Debug.Log("Excuse Me"); }
        
        Emote(1);

    }

    public void Move()
    {
        //anim.SetBool("IsMoving", true);
        theRB.velocity = moveDirection * curSpeed;
        moveDirection.Normalize();
    }

/*----------------------------------------------------------------------------*/

    void FixedUpdate()
    {
        //if(showDebug) { Debug.Log(string.Format("{0}{1}",namePlate, curState)); }

/*--------------------------------------------------------*/
/*-----------------Switches and Logic---------------------*/
        switch(curState)
        {


    /*----------------------------------------------------------------------------*/
            case State.Ready:

                {
                    if(lastState == State.Idle)
                    {
                        if(idleCounter <= 0)
                        {
                            lastState = State.Wander;
                            idleCounter = Random.Range(idleLength * .75f, idleLength * 1.25f);
                        }
                    }

                    if(lastState == State.Wander)
                    {

                    }

                    if(lastState == State.Melee)
                    {
                        
                    }
                }

            lastState = curState;

            if(curState == State.Ready)
            {
                curState = State.Idle;
            }

        break;
    /*--------------------------------------------------------*/
            case State.Idle:
            
                if(shouldIdle)
                {
                    anim.SetBool("IsMoving", false);
                    if(showDebug) { Debug.Log("I am bored"); }
                    
                    idleCounter -= Time.deltaTime;

                    lastState = curState;
                    curState = State.Ready;


                }
                break;
    /*----------------------------------------------------------------------------*/
            case State.Wander:
                
                if(wanderTick)
                {
                    anim.SetBool("IsMoving", true);

                    if(showDebug) { Debug.Log("I'm the kind of sprite, who likes to roam around"); }

                    /*if(!wanderTick)
                    {
                        moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                        wanderTick = true;
                    }*/

                    wanderCounter -= Time.deltaTime;
                    
                    Move();

                    curState = State.Ready;
                }
                break;
    /*----------------------------------------------------------------------------*/
            case State.Patrol:
                if(showDebug) { Debug.Log("Hut, hut, hut, hut, hut, hut, hut"); }
                break;
    /*----------------------------------------------------------------------------*/
            case State.Shoot:
                if(showDebug) { Debug.Log("Pew Pew"); }
                break;
    /*----------------------------------------------------------------------------*/
            case State.Melee:

                if(shouldMelee)
                {
                    lastState = curState;
                }
                break;
    /*----------------------------------------------------------------------------*/
            case State.Charge:

                if(shouldCharge)
                {
                    lastState = curState;
                }
            
            break;

/*----------------------------------------------------------------------------*/
/*--------------------------------Timers--------------------------------------*/
        }

        //Emote
        if(emoteCounter >= 0)
        emoteCounter -= Time.deltaTime;
        
        if(emoteCounter <= 0)
        {
            shouldEmote = true;
            emoteCounter = timeBetweenEmotes;
        }

    }


/*-----------------------------Collisions-------------------------------------*/

    private void OnCollisionEnter(Collision other)
    {
        if(showDebug) { Debug.Log(string.Format("Collide" )); }

        //Friendly
        if(other.gameObject.tag == friend)
        {
        }

        //Enemy
        if(other.gameObject.tag == foe)
        {
            mfToKill = other.gameObject;

            if(showDebug) { Debug.Log(string.Format("Kill that SOB {0}", mfToKill)); }
        }

        //Bump
        if(other.gameObject.tag == "Building")
        {
                moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        }
        
    }
    
    private void OnCollisionStay(Collision other) 
    {
        if(other.gameObject.tag == "Building")
            {
                moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            }
                
    }
/*----------------------------------------------------------------------------*/
/*--------------------------Sprites and GFX-----------------------------------*/

        //curSprite = bodies[curSpriteCount];
        //theBody.sprite = curSprite;
}


    /* CODE SNIPPETS

    if(showDebug) { Debug.Log(string.Format("")); }

    //Combat
    /*Initial atk or volley 
    compare ATk roll to target. Holding value of atk for a second i case of multiple attackers
    (possible opponent counter down the line to change animations)
    timer between attacks in SHift state
    Charge towards mftoKills
    revert back to melee state*/
