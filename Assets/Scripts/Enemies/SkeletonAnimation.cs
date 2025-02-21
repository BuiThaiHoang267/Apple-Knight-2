using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimation : MonoBehaviour
{
    private Animator anim;
    [SerializeField] 
    private Enemy4 mainCtrl;
    [SerializeField] 
    private Transform attackHitBox;
    private float facingDirection;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartAttack(float facing)
    {
        anim.SetBool("isAttack", true);
        facingDirection = facing;
    }
    public void Attacking()
    {
        mainCtrl.Attaking();
    }
    public void EndAttack()
    {
        anim.SetBool("isAttack", false);
        mainCtrl.FinishAttack1();
    }
    public void StartDamaged()
    {
        anim.SetBool("isDamaged", true);
    }
    public void EndDamaged()
    {
        anim.SetBool("isDamaged", false);
        mainCtrl.FinishDamaged();
    }
    public void StartWalk()
    {
        anim.SetBool("isWalk", true);
    }
    public void EndWalk()
    {
        anim.SetBool("isWalk", true);
    }
}
