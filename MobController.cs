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
    }
    
}



