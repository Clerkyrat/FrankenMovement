using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MobLogic : MonoBehaviour
{
    public static MobLogic instance;

    [Header("Stats")]
    public int health;
    public int moraleBase;
    public float moveSpeed;
    public int atkSkill;
    public float atkSpeed;
    private Vector3 moveDirection;
    private int curMorale;

    //Roll every few moments based on ATKSPeed. Both gameobjects will roll independently if "shouldMelee"
    //is checked. 
    
    [Header("Emotes")]
    public bool shouldEmote;
    public GameObject[] emotes;
    public float timeBetweenEmotes,emoteCounter;
    private bool emoteTick;

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
    public bool shouldMelee;

    //States
    [Header("Idle")]
    //public bool blockWhenIdle;
    //private bool willBlock;
    public float idleLength;
    private float idleCounter;

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

    [Header("Melee")]
    public string foe;
    public string friend;
    private bool hasTarget;
    private GameObject mfToKill;
    

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

    //Enums
    enum State {Branch, Idle, Chase, Wander, Patrol, Shoot, Melee};
    State curState = State.Idle;
    State lastState;

    //Wake
    private void Awake()
    {
        instance = this;
    }


    void Start()
    {
        //Time between emote icons and logs apearing
        emoteCounter = timeBetweenEmotes;

        //Time mobs should wander before moving on to the next state
        wanderCounter = wanderLength;

        //Time mob should idle before moving on to the next state
        idleCounter = idleLength;

        //Starting appearance of mob
        curSpriteCount = 0;
            
    }

/*--------------------------------------------------------*/
/*----------------------Functions-------------------------*/

    public void Emote(int emotion)
    {
        Instantiate(emotes[emotion], emotePoint.position, emotePoint.rotation);
    }

    public void Move()
    {
        anim.SetBool("IsMoving", true);
        theRB.velocity = moveDirection * moveSpeed;
        moveDirection.Normalize();
    }

    void Update()
    {

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
                    
                    if(shouldEmote)
                    {
                        Debug.Log("I am bored");
                        //Emote(0); Unused Currently
                        shouldEmote = false;
                    }
                    
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
                Debug.Log("Come back here!");
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

                    if(shouldEmote)
                    {
                        Debug.Log("I'm the kind of sprite, who likes to roam around");
                        //Emote(0); Unused Currently
                        shouldEmote = false;
                    }

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
                Debug.Log("Hut, hut, hut, hut, hut, hut, hut");
                break;
    /*----------------------------------------------------------------------------*/
            case State.Shoot:
                Debug.Log("Pew Pew");
                break;
    /*----------------------------------------------------------------------------*/
            case State.Melee:

                if(shouldEmote)
                {
                    Debug.Log("Prepare to die!");
                    shouldEmote = false;
                }



                break;
    /*----------------------------------------------------------------------------*/
        }
        emoteCounter -= Time.deltaTime;
        
        if(emoteCounter <= 0 && !shouldEmote)
        {
            shouldEmote = true;
            emoteCounter = timeBetweenEmotes;
        }
        
        
    }
    
    private void OnCollisionEnter(Collision other)
    {
        //Possibly convert the following into a Switch statement as it expands
        
        if(other.gameObject.tag == friend)
        {
            Debug.Log("Excuse Me");
            idleCounter = idleCounter *.25f;
            curState = State.Idle;
            
        }

        if(other.gameObject.tag == foe && !hasTarget)
        {
           mfToKill = other.gameObject;
           curState = State.Melee;
        }


        if(other.gameObject.tag == "Building")
        {
            lastState = curState;
            Debug.Log("Ooof");
            //wanderCounter = wanderCounter + 1f;
            moveDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            
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
}
