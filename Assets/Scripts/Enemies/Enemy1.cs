using UnityEngine;

public class Enemy1: BaseEnemy
{
    [SerializeField]
    private EnemyData enemyData;

    protected Animator anim;
    protected Rigidbody2D myRb;
    protected bool isDetectPlayerLeft;
    protected bool isDetectPleyerRight;
    protected bool isFacingRight;

    private float attackCoolDown;
    private float attackTimeLeft;
    private float attackRadius;
    private float HP;
    private bool isAttack;

    public Transform detectPlayer;
    public Transform attackHitBoxPos;
    public LayerMask whatIsPlayer;

    void Start()
    {
        InitializedEnemy();
    }
    public override void InitializedEnemy()
    {
        enemyData = new EnemyData();

        HP = enemyData.maxHP;
        anim = GetComponent<Animator>();
        myRb = GetComponent<Rigidbody2D>();
        isDetectPlayerLeft = isDetectPleyerRight = false;
        canMove = true;
        attackCoolDown = 1f;
        attackTimeLeft = 1f;
        attackRadius = 1.5f;
    }
    void Update()
    {
        if (InGameManager.Instance.PauseGame())
        {
            return;
        }
        DetectPlayer();
        CheckMoveDirection();
        DealDamage();
    }
    private void FixedUpdate()
    {
        if (InGameManager.Instance.PauseGame())
        {
            return;
        }
        Move();
    }

    public override void DealDamage()
    {
        isDetectInHitBox = Physics2D.Raycast(this.transform.position, transform.right, 1.5f, whatIsPlayer);
        if(isDetectInHitBox)
        {
            isAttack = true;
        }
        if (isAttack)
        {
            if(attackTimeLeft > 0)
            {
                attackTimeLeft -= Time.deltaTime;
                canMove = false;
            }
            else
            {
                anim.SetBool("isAttack", true);
                CheckAttackHitBox();
                attackTimeLeft = attackCoolDown;
                isAttack = false;
            }
        }
        else
        {
            attackTimeLeft = attackCoolDown;
            canMove = true; 
        }
    }
    public void CheckAttackHitBox()
    {
        Collider2D coll = Physics2D.OverlapCircle(attackHitBoxPos.position, attackRadius, whatIsPlayer);
        if (coll != null)
        {
            GameObject player = coll.gameObject;
            player.GetComponent<PlayerCombatController>().TakeDamage(enemyData.damage, this.gameObject, 0);
        }
    }

    public void FinishAttack1()
    {
        anim.SetBool("isAttack", false);
        canMove = true;
        attackTimeLeft = attackCoolDown;
    }
    public void CheckMoveDirection()
    {
        if(isDetectPlayerLeft || isDetectPleyerRight) 
        {
            Flip();
        }
    }
    public void Flip()
    {
        if (isDetectPlayerLeft)
        {
            transform.Rotate(0, 180, 0);
            if (isFacingRight)
            {
                facingDirection = -1;
            }
            else
            {
                facingDirection = 1;
            }
        }
    }
    public override void Move()
    {
        if (isDetectPleyerRight && canMove)
        {
            myRb.velocity = new Vector2(enemyData.speed*facingDirection,myRb.velocity.y);
        }
        else
        {
            myRb.velocity = new Vector2(enemyData.speed * 0, myRb.velocity.y);
        }
    }
    public override void DetectPlayer()
    {
        isDetectPlayerLeft = Physics2D.Raycast(detectPlayer.position + transform.right.normalized * -1, transform.right * -1, enemyData.detectionRange - 1, whatIsPlayer);
        isDetectPleyerRight = Physics2D.Raycast(detectPlayer.position, transform.right, enemyData.detectionRange, whatIsPlayer);
        isFacingRight = Physics2D.Raycast(new Vector2(detectPlayer.position.x-enemyData.detectionRange,detectPlayer.position.y), transform.right, enemyData.detectionRange, whatIsPlayer);
    }
    public override void IsDamaged(float damage)
    {
        HP -= damage;
        anim.SetBool("isDamaging", true);
        myRb.AddForce(new Vector2(50, 100));
    }
    public override void FinishDamaged()
    {
        anim.SetBool("isDamaging", false);
        if (HP <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
