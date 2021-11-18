using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MobLogic : MonoBehaviour
{
    public static MobLogic instance;

    [Header("Info")]
    public bool showDebug;
    public bool showEmoteDebug;
    public string namePlate;
    public List<string> nameDatabase = new List<string>();
    public int serialID;
    

    [Header("Stats")]
    public float moveSpeed;
    private float curSpeed;
    public int endurance;
    private int curEndurance;
    private bool isPanicking;
    private bool isCowering;

    [Header("Relations")]
    public List<GameObject> friendsList;
    public int maxFriends;
    public string foe;
    public string friend;
    //private bool sameName;

    [Header("Threat and Morale")]
    public float moraleBase;
    private float curMorale;
    public float threatBase;
    private float curThreat;
    private float targetThreat;

    [Header("Movement")]
    private Vector3 moveDirection;
    private Vector3 wanderDirection;
    private Vector3 homePoint;
    private bool holdPlace;

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
    public bool shouldFlee;
    public bool shouldPanic;
    public bool shouldCower;
    public bool shouldMelee;
    public bool shouldGib;
    public bool shouldDropItems;
    public bool shouldEmote;

    //Ticks and Trips
    private bool idleTick, idleTrip;
    private bool wanderTick, wanderTrip;
    private bool fleeTick, fleeTrip;
    private bool panicTick, panicTrip;
    private bool cowerTick, cowerTrip;

/*-------------------------STATES--------------------------*/

    [Header("Ready")]
    public GameObject mobTarget;
    private GameObject sightTarget;
    private float targetRange;
    public float rangeToChase;
    private bool hasTarget;

    [Header("Idle")]
    public float idleLength;
    private float idleCounter;

    [Header("Wander")]
    public float wanderLength;
    private float wanderCounter;

    [Header("Flee")]
    public int rangeToFlee;
    public float fleeInterval;
    private float fleeCounter;
    public float cowerLength;
    private float cowerCounter;

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
    enum State {Ready, Idle, Wander, Flee, Cower, Patrol, Shoot, Melee, Charge};
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
        fleeCounter = fleeInterval;

        //Stats
        curMorale = Random.Range(moraleBase * .10f, moraleBase * 2.0f);
        holdPlace = false;

        //Bools
        if(shouldWander) { wanderTick = true; }
        if(shouldIdle) { idleTick = true; }
        if(shouldFlee) { fleeTick = true; }
        if(shouldCower) { cowerTick = true; }

        if (shouldEmote) { emoteTick = true; }

        //Starting appearance of mob
        curSpriteCount = 0;

        //Relationships
        //sameName = false;

        //Starting position
        homePoint = this.transform.position;
            
    }

/*--------------------------------------------------------*/
/*----------------------Functions-------------------------*/

    public void Identify()
    {
        int randName = Random.Range(1, nameDatabase.Count);
        namePlate = nameDatabase[randName];
        serialID = Random.Range(100, 999);

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


    public void Move()
    {
        if(!holdPlace)
        {
            anim.SetBool("IsMoving", true);
            theRB.velocity = moveDirection * curSpeed;
            moveDirection.Normalize();
        }
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
                    /*-------------*/
                    if(lastState == State.Idle)
                    {
                        if(idleCounter <= 0)
                        {
                            lastState = State.Wander;
                            idleCounter = Random.Range(idleLength * .75f, idleLength * 1.25f);
                        }else
                        {
                            curState = State.Idle;
                        }
                    }
                    /*-------------*/
                    if(lastState == State.Wander)
                    {
                        if(wanderCounter <= 0)
                        {
                            lastState = State.Ready;
                            wanderCounter = Random.Range(idleLength * .75f, idleLength * 1.25f);
                            wanderTrip = false;
                        }else
                        {
                            curState = State.Wander;
                        }
                    }
                    /*-------------*/
                    if(lastState == State.Flee)
                    {
                        if(fleeTrip && Vector3.Distance(this.transform.position, mobTarget.transform.position) > rangeToFlee)
                        {
                            anim.SetBool("IsMoving", false);
                            fleeTrip = false;
                            cowerCounter = cowerLength;
                            curState = State.Cower;
                        }else
                        {
                            if(fleeCounter <= 0f)
                            {
                                fleeCounter = fleeInterval;
                                fleeTrip = false;
                            }

                            curState = State.Flee;
                        }
                    }
                    /*-------------*/
                    /*if(lastState == State.Panic)
                    {
                        if(panicCounter <= 0f)
                        {
                            anim.SetBool("IsMoving", false);
                            panicTrip = false;
                            cowerCounter = cowerLength * 2f;
                            curState = State.Cower;
                        }

                        if(panicTrip && hasTarget && Vector3.Distance(this.transform.position, mobTarget.transform.position) > rangeToFlee)
                        {
                            anim.SetBool("IsMoving", false);
                            panicTrip = false;
                            cowerCounter = cowerLength * 2f;
                            curState = State.Cower;
                        }

                        if(hasTarget)
                        {
                            curState = State.Panic;
                        }else
                        {
                            anim.SetBool("IsMoving", false);
                            //panicTrip = false;
                            cowerCounter = cowerLength * 2f;
                            curState = State.Cower;
                        }

                    }*/
                    /*-------------*/
                    if(lastState == State.Cower)
                    {
                        //curSpeed = moveSpeed;
                        if(cowerCounter <= 0f)
                        {
                            isCowering = false;
                            anim.SetBool("isCowering", false);
                            isPanicking = false;
                            curState = State.Idle;
                            holdPlace = false;
                            cowerCounter = cowerLength;
                        }else
                        {
                            curState = State.Cower;
                            holdPlace = false;
                        }

                        if(hasTarget &&Vector3.Distance(this.transform.position, mobTarget.transform.position) < rangeToFlee)
                        {
                            if(curMorale < 2f && hasTarget) {curState = State.Flee;}
                            if(curMorale < 1f && hasTarget) {isPanicking = true;}
                        }

                    }
                    /*-------------*/
                    if(lastState == State.Melee)
                    {
                        
                    }
                }

            if(curMorale < 2f && hasTarget)
            {
                if(fleeTick)
                {
                    if(Vector3.Distance(this.transform.position, mobTarget.transform.position) < rangeToFlee)
                    {
                        curState = State.Flee;
                        fleeTrip = false;
                    }
                    
                    if(curMorale < 1f && hasTarget)
                    {
                        isPanicking = true;
                    }
                }
            }


            if(isCowering)
            {
                moveSpeed = 0f;
            }

            lastState = curState;

            if(curState == State.Ready)
            {
                curState = State.Idle;
            }

        break;
    /*--------------------------------------------------------*/
            case State.Idle:
            
                if(idleTick)
                {
                    anim.SetBool("IsMoving", false);
                    if(showEmoteDebug) { Debug.Log("I am bored"); }
                    
                    idleCounter -= Time.deltaTime;

                    lastState = curState;
                    curState = State.Ready;


                }
                break;
    /*----------------------------------------------------------------------------*/
            case State.Wander:
                
                if(wanderTick)
                {

                    if(showEmoteDebug) { Debug.Log("I'm the kind of sprite, who likes to roam around"); }

                    if(!wanderTrip)
                    {
                        moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                        wanderTrip = true;
                    }

                    wanderCounter -= Time.deltaTime;
                    
                    Move();

                    lastState = curState;
                    curState = State.Ready;
                }

                break;
    /*----------------------------------------------------------------------------*/
            case State.Flee:
                
                if(fleeTick)
                {
                    Move();

                    if(!fleeTrip)
                    {
                        moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                        fleeCounter = Random.Range(fleeInterval * .5f, fleeInterval * 2.0f);
                        fleeTrip = true;
                    }
                }

                lastState = curState;
                curState = State.Ready;

                break;
    /*----------------------------------------------------------------------------*/
            /*case State.Panic:

                if(panicTick)
                {
                    anim.SetBool("isPanicking", true);
                    panicCounter -= Time.deltaTime;

                    if(!panicTrip)
                    {
                        moveDirection = this.transform.position - mobTarget.transform.position;
                        panicCounter = Random.Range(panicInterval * .5f, panicInterval * 2.0f);
                        panicTrip = true;
                    }

                    Move();
                }
                
                lastState = curState;
                curState = State.Ready;

                break;*/

    /*----------------------------------------------------------------------------*/
            case State.Cower:

                if(cowerTick)
                {
                    holdPlace = true;
                    isCowering = true;
                    anim.SetBool("isCowering", true);
                    cowerCounter -= Time.deltaTime;
                }

                lastState = curState;
                curState = State.Ready;

                break;
    /*----------------------------------------------------------------------------*/
            case State.Patrol:
                if(showEmoteDebug) { Debug.Log("Hut, hut, hut, hut, hut, hut, hut"); }
                break;
    /*----------------------------------------------------------------------------*/
            case State.Shoot:
                if(showEmoteDebug) { Debug.Log("Pew Pew"); }
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
            moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        }

        //Enemy
        if(other.gameObject.tag == foe)
        {
            mobTarget = other.gameObject;

            if(showEmoteDebug) { Debug.Log(string.Format("Kill that SOB {0}", mobTarget)); }
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

        //Friendly
        if(other.gameObject.tag == friend)
        {
            moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
        }
                
    }

    private void OnTriggerEnter(Collider other) 
    {

        if(other.gameObject.tag == foe)
        {
            mobTarget = other.gameObject;
            hasTarget = true;

            if(showEmoteDebug) { Debug.Log(string.Format("Kill that SOB {0}", mobTarget)); }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == foe)
        {
            mobTarget = null;
            hasTarget = false;

            if(showEmoteDebug) { Debug.Log(string.Format("Lost that {0}", mobTarget)); }
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
