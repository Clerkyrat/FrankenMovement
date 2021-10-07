using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MobLogic : MonoBehaviour
{
    public static MobLogic instance;

    [Header("Stats")]
    private float moveSpeed;
    public float moveSpeedMax;
    public int atkSkill;
    public int atkValue;
    public int endurance, enduranceMax;
    private int curThreat;
    public int threat, threatMin, threatMax;
    private int curMorale;
    public int moraleBase;

    //Roll every few moments based on ATKSPeed. Both gameobjects will roll independently if "shouldMelee"
    //is checked. 
    
    [Header("Emotes")]
    public bool shouldEmote;
    public GameObject[] emotes;
    public float timeBetweenEmotes,emoteCounter;
    private bool emoteTick;

    [Header("Info")]
    public string namePlate;
    public int serialID;

    [Header("Relations")]
    public List<GameObject> friendsList;
    public int maxFriends;
    public string foe;
    public string friend;


    [Header("Tools")]
    public Rigidbody theRB;
    public Animator anim;
    public SpriteRenderer theBody;
    public Transform emotePoint;
    public bool showDebug;
    private Vector3 moveDirection;
    private Vector3 shiftDirection;
    private Vector3 chargeDirection;
 
    [Header("Bools")]
    public bool shouldIdle;
    public bool shouldChase;
    public bool shouldWander;
    public bool shouldShoot;
    public bool shouldGib;
    public bool shouldDropItems;
    public bool shouldMelee;

    //States
    [Header("Idle")]
    public float idleLength;
    private float idleCounter;
    //public bool blockWhenIdle;
    //private bool willBlock;
    

    [Header("Chase")]
    public float rangeToChase;

    [Header("Wander")]
    //private bool wanderTick = false; Unused Currently
    public float wanderLength;
    private float wanderCounter;
    private Vector3 wanderDirection;
    private bool wanderTick;

    [Header("Shoot")]
    public GameObject bullet;
    public Transform firePoint;

    [Header("Melee, Ready, Shift, Charge")]
    public bool hasTarget;
    private bool isGuarding;
    private bool isCharging;
    private bool shiftTick = false;
    public float shiftTime = 3f;
    private GameObject mfToKill;
    private float mfRange;
    public int attacksPerShift = 1;
    public int woundThreshold = 2;
    public float timeBetweenCharging;
    public float shiftTimeMax;
    public float meleeTime;
    public float meleeTimeLength;
    
    [Header("Gibbing")]
    public bool shouldLeaveCorpse;
    public GameObject[] corpses;
    public GameObject hitEffect;
    public GameObject[] splatters;

    [Header("Drops")]
    public GameObject[] itemsToDrop;
    public float itemDropPercent;

    [Header("Sprites")]
    public Sprite[] bodies;
    public int curSpriteCount;
    private Sprite curSprite;

    [Header("Fluff")]
    public List<string> nameDatabase = new List<string>();
    //{"Bob" , "Larry" , "Phil" , "Claire" , "ALex" , "Hayley" , "Luke" , "Mitchell" , "Cameron" , "Lilly" ,"Mark" , "John" , "Betty" , "Sam" , "Frodo" , "Pippin"};

    //Enums
    enum State {Branch, Idle, Chase, Wander, Patrol, Shoot, Melee, Ready, Shift, Charge};
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
            
        //Time between emote icons and logs apearing
        emoteCounter = timeBetweenEmotes;
        if (shouldEmote)
        {
            emoteTick = true;
        }

        //Time mobs should wander before moving on to the next state
        wanderCounter = wanderLength;

        //Time mob should idle before moving on to the next state
        idleCounter = idleLength;

        //Starting appearance of mob
        curSpriteCount = 0;

        shiftTimeMax = shiftTime;
        moveSpeed = moveSpeedMax;
        meleeTime = meleeTimeLength;
            
    }

/*--------------------------------------------------------*/
/*----------------------Functions-------------------------*/

    public void Identify()
    {
        int randName = Random.Range(1, nameDatabase.Count);
        serialID = Random.Range(100, 999);
        namePlate = nameDatabase[randName];

        if(showDebug) 
        {   
            Debug.Log(string.Format("My name is {0}, my Serial # is {1} and I am at {2}", namePlate, serialID, transform.position));
        }

    }

    public void Emote(int emotion)
    {   
        Instantiate(emotes[emotion], emotePoint.transform.position, emotePoint.transform.rotation);

        if(emoteTick)
        {
            Instantiate(emotes[emotion], emotePoint.position, emotePoint.rotation);
        }
    }

    public void Move()
    {
        anim.SetBool("IsMoving", true);
        theRB.velocity = moveDirection * moveSpeed;
        moveDirection.Normalize();
    }

    void FixedUpdate()
    {

    /*----------------------------------------------------------------------------*/
    /*-------------------------------Decisions------------------------------------*/

    /*--------------------------------------------------------*/
    /*--------------------Sprites and GFX---------------------*/
        curSprite = bodies[curSpriteCount];
        theBody.sprite = curSprite;

    /*--------------------------------------------------------*/
    /*-----------------Switches and Logic---------------------*/
        switch(curState)
        {
    /*--------------------------------------------------------*/
            case State.Branch:

            //Decision tree to select next State after varaible check.
    /*--------------------------------------------------------*/
            case State.Idle:
            
                if(shouldIdle)
                {
                    //FUTURE: Pick from list of idle animations and tweak variables
                    //during said animation. Movespeed 0 when sitting, interacting, ect...
                    anim.SetBool("IsMoving", false);
                    
                    /*if(shouldEmote)
                    {
                        if(showDebug) { Debug.Log("I am bored"); }
                        Emote(0);
                        shouldEmote = false;
                    }*/
                    
                    idleCounter -= Time.deltaTime;

                    if(idleCounter <= 0)
                    {
                        curState = State.Wander;
                        idleCounter = Random.Range(idleLength * .75f, idleLength * 1.25f);
                    }
                    
                }
                break;
    /*----------------------------------------------------------------------------*/
            case State.Chase:
                if(showDebug) { Debug.Log("Come back here!"); }
                break;
    /*----------------------------------------------------------------------------*/
            case State.Wander:
                
                if(shouldWander)
                {
                    if(!wanderTick)
                    {
                        moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                        wanderTick = true;
                    }

                    /*if(shouldEmote)
                    {
                        if(showDebug) { Debug.Log("I'm the kind of sprite, who likes to roam around"); }
                        Emote(1);
                        shouldEmote = false;
                    }*/

                    wanderCounter -= Time.deltaTime;
                    
                    Move();

                    if(wanderCounter <= 0)
                    {
                        curState = State.Idle;
                        wanderCounter = Random.Range(wanderLength * .75f, wanderLength * 1.25f);
                        wanderTick = false;
                    
                    }
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

                if(shouldEmote)
                {
                    Emote(4);
                    if(showDebug) { Debug.Log("Prepare to die!"); }
                    shouldEmote = false;
                }

                atkValue = Random.Range(1, 20) + atkSkill;
                if(showDebug) { Debug.Log(string.Format("{0}'s attack roll was {1}", namePlate, atkValue)); }

                meleeTime = meleeTime - Time.deltaTime;

                if(meleeTime <= 0)
                {
                    meleeTime = meleeTimeLength;
                    curState = State.Ready;
                }

                //Initial atk or volley 
                //compare ATk roll to target. Holding value of atk for a second i case of multiple attackers (possible opponent counter down the line to change animations)
                //timer between attacks in SHift state
                //Charge towards mftoKills
                //revert back to melee state



                break;
    /*----------------------------------------------------------------------------*/
            case State.Ready:

                //curState = State.Idle;
                            
                if(hasTarget)
                {
                    mfRange = Vector3.Distance(transform.position, mfToKill.transform.position);

                    if(mfRange > rangeToChase)
                    {
                        if(showDebug) { Debug.Log(string.Format("Lost em {0}", mfRange)); }
                        hasTarget = false;

                        curState = State.Idle;
                        //Set to ready and if lacking mfToKill switch to Idle. Ready will be the powerhous of decision making.
                    }

                    if(mfRange < rangeToChase)
                    {
                        curState = State.Shift;
                    }
                }

            
            break;

    /*----------------------------------------------------------------------------*/
            case State.Shift:

                if(!shiftTick)
                {
                    shiftTick = true;
                    shiftDirection = transform.position - mfToKill.transform.position;
                    moveDirection = shiftDirection;
                    moveSpeed = moveSpeed * .75f;
                }

                Move();

                shiftTime = shiftTime - Time.deltaTime;

                if(shiftTime <= 0)
                {
                    shiftTick = false;
                    moveSpeed = moveSpeedMax;
                    shiftTime = shiftTimeMax;
                    curState = State.Charge;
                }

            break;

    /*----------------------------------------------------------------------------*/
    //Bugged, launches off map
    
            case State.Charge:

            if(mfRange < rangeToChase)
            {

                if(!isCharging)
                {
                    Emote(2);
                    if(showDebug) { Debug.Log(string.Format("CHAAAARGE!")); }
                    isCharging = true;
                    chargeDirection = mfToKill.transform.position;
                    moveDirection =chargeDirection;
                    
                }

                Move();
            }else
            {
                isCharging = false;
                curState = State.Ready;
            }

            break;

    /*----------------------------------------------------------------------------*/
    /*--------------------------------Timers--------------------------------------*/
        }

        if(emoteCounter >= 0)
        emoteCounter -= Time.deltaTime;
        
        if(emoteCounter <= 0 && !shouldEmote)
        {
            shouldEmote = true;
            emoteCounter = timeBetweenEmotes;
        }

    }

    /*----------------------------------------------------------------------------*/
    /*-----------------------------Collisions-------------------------------------*/

    private void OnCollisionEnter(Collision other)
    {
        //Possibly convert the following into a Switch statement as it expands
        
        //switch:

        //Friendly
        if(other.gameObject.tag == friend)
        {
            if(showDebug) { Debug.Log("Excuse Me"); }
            
            if(shouldEmote)
            {
                Emote(1);
                shouldEmote = false;
            }

            bool sameName = false;

            for(int i = 0; i < friendsList.Count; i++)
            {
                if(friendsList[i] = other.gameObject)
                {
                    sameName = true;
                }
                
            }
            if(!sameName)
            {
                friendsList.Add(other.gameObject);
            }


            idleCounter = idleCounter *.25f;
            curState = State.Idle;
            
        }

        //Enemy
        if(other.gameObject.tag == foe)
        {

            if(!hasTarget)
            {
                mfToKill = other.gameObject;
                hasTarget =true;
                curState = State.Melee;

                if(showDebug) { Debug.Log(string.Format("Kill that SOB {0}",mfToKill)); }
            }

            if(mfToKill = other.gameObject)
            {
                curState = State.Melee;
            }
        }

        //Bump
        if(other.gameObject.tag == "Building")
        {
            if(curState != State.Shift)
            {
                lastState = curState;
                if(showDebug) { Debug.Log("Ooof"); }
                //wanderCounter = wanderCounter + 1f;
                moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            }
        }
    }
    
    private void OnCollisionStay(Collision other) 
    {
        if(other.gameObject.tag == "Building")
            {
                
                //wanderCounter = wanderCounter + 1f;
                moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
                
            }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "Player")
        {
            
        }
    }

    /* CODE SNIPPETS

    if(showDebug) { Debug.Log(string.Format("")); }

    */
}
