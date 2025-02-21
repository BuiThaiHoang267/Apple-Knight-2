using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float speed;
    private Rigidbody2D myRb;
    private float damage;
    private float explodeRadius;

    public LayerMask whatIsPlayer;
    public LayerMask whatIsGround;
    void Start()
    {
        damage = 30;
        myRb=GetComponent<Rigidbody2D>();
        explodeRadius = 2f;
    }

    
    void Update()
    {
        
    }

    public void SetUp(float H, float L, float facing)
    {
        myRb=GetComponent<Rigidbody2D>();
        speed = Mathf.Sqrt(Mathf.Abs((-9.81f * L * L) / (H - L)));
        myRb.velocity = new Vector2(1*facing,1).normalized * speed;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlaySound("Explosion");
            //Animation Effect Explode
            GameObject effectExplode = EffectManager.Instance.Take(0);
            effectExplode.transform.position = this.transform.position;
            Dust dust = effectExplode.GetComponent<Dust>();
            dust.StartAnimExplode();
            CheckHitBoxExplode();
            BulletManager.Instance.ReturnBomb(this.gameObject);
        }
        if (whatIsGround == (whatIsGround | (1 << collision.gameObject.layer)))
        {
            AudioManager.Instance.PlaySound("Explosion");
            //Animation Effect Explode
            GameObject effectExplode = EffectManager.Instance.Take(0);
            effectExplode.transform.position = this.transform.position;
            Dust dust = effectExplode.GetComponent<Dust>();
            dust.StartAnimExplode();
            CheckHitBoxExplode();
            BulletManager.Instance.ReturnBomb(this.gameObject);
        }
    }

    private void CheckHitBoxExplode()
    {
        Collider2D collider = Physics2D.OverlapCircle(this.transform.position, explodeRadius, whatIsPlayer);
        if(collider != null)
        {
            GameObject player = collider.gameObject;
            player.GetComponent<PlayerCombatController>().TakeDamage(damage, gameObject, 15);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, explodeRadius);
    }
}
