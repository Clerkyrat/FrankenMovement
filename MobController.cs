using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
            
public class MobController : MonoBehavior
{
    public static MobController instance;
    
    [Header("Stats")]
    public int health;
    public float moveSpeed;
    private Vector3 moveDirection;
    
    enum State {Idle, Chase, Wamder, Patrol, Shoot};
    
    //Bools to trigger States
    [Header("Bools")]
    public bool shouldIdle,shouldChase,shouldWander,shouldPatrol,shouldShoot,shouldGib, shouldDropItems;
    
    //States
    [Header("Idle")]
    public bool blockWhenIdle;
    private bool willBlock;
    
    [Header("Chase")]
    public float rangeToChase;
    
    [Header("Wander")]
    private bool wanderTick = false;
    public float wanderLength, pauseLength;
    private float wanderCounter, pauseCounter, panicCounter;
    private Vector3 wanderDirection;
    
    [Header("Patrol")]
    public Transform[] patrolPoints;
    private int currentPatrolPoints;
    
    [Header("Shoot")]
    public GameObject bullet;
    public Transform firePoint;
    public float fireRate;
    private float fireCounter;
    public float shotRange;
    
    [Header("Gibbing")]
    public bool shouldLeaveCorpse;
    public GameObject[] deathSplatters;
    public GameObject hitEffect;
    
    [Header("Drops")]
    public GameObject[] itemsToDrop;
    public float itemDropPercent;
    
    //Misc
    [Header("Tools")]
    public Rigidbody2D theRB;
    public Animator anim;
    public SpriteRenderer theBody;
    
    [Header("GFX")]
    private Vector2 facing = Vector2.zero;
    private Vector3 bodyPoint, backPoint; //check between both points to detect direction
    private float flipCount;
    public float flipMax =.25f
    private float scaleY,scaleX;
            
    private void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        
        //GFX Flipping start positions
        scaleX = transform.localScale.x;
        scaleY = transform.localScale.y;
        flipCount = flipMax;
        backPoint = transform.position;
        anim.SetFloat("Idle",startIdle);
        
        State myState;
        
        mystate = State.Idle
    }
    
    void State()
    {
    case Idle:
        print("I am bored")
        break;
        
    case Chase:
        print("Come back here!")
        break;
        
    case Wander:
        print("I'm the kind of sprite, who likes to roam around")
        break;
        
    case Patrol:
        print("Hut, hut, hut, hut, hut, hut, hut")
        break;
        
    case Shoot:
        print("Pew Pew")
        break;
    
    
    
    void Update()
    
        //GFX Flip and facing tracking//
        bodyPoint.x = transform.position.x;

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
        
        
    
}



