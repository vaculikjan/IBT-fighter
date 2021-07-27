using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    public int player;
    private void Start() {
        animator = GetComponent<Animator> ();
    }
    void Update() {
        if (isInAction()) {
            return;
        }
        if ((Input.GetKeyDown(KeyCode.J) && player == 1) || (Input.GetKeyDown(KeyCode.Delete) && player == 2)) {
            Attack1();
        }
        if ((Input.GetKeyDown(KeyCode.K) && player == 1) || (Input.GetKeyDown(KeyCode.End) && player == 2)) {
            Attack2();
        }
        if ((Input.GetKeyDown(KeyCode.L) && player == 1) || (Input.GetKeyDown(KeyCode.PageDown) && player == 2)) {
            Parry();
        }
    }

    private void Attack1() {
        
        animator.SetTrigger("Attack");
    }
    private void Attack2() {
        animator.SetTrigger("Attack2");
    }
    private void Parry() {
        animator.SetTrigger("Parry");
    }
    private bool isInAction() {
        return animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Base Layer")).IsTag("Action");
    }     
    
}
