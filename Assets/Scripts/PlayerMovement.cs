using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class PlayerMovement : Agent
{
    private Rigidbody2D rBody;
    private BoxCollider2D boxCollider2D;
    public PhysicsMaterial2D friction, noFriction;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private string axis;
    
    //Set in editor
    public Transform Enemy;
    public BoxCollider2D EnemyAttack1;
    public BoxCollider2D EnemyAttack2;
    public BoxCollider2D EnemyParry;
    public float forceMultiplier = 10;

    void Start () { //Fill variables used in the script

        rBody = GetComponent<Rigidbody2D>();
        rBody.sharedMaterial = noFriction;
        boxCollider2D =  GetComponent<BoxCollider2D> ();
        spriteRenderer = GetComponent<SpriteRenderer> ();
        animator = GetComponent<Animator> ();
        if (this.name == "Player 1") {
            axis = "Horizontal";
        }
        else {
            axis = "Horizontal2";
        }

        this.MaxStep = 5000; //Max episode length in steps

    }
    
    public override void OnEpisodeBegin() { //Whenever new episode begins this is called and sets the initial values for training

        if (this.name == "Player 1") {
            this.transform.localPosition = new Vector3(-2f,2f,0f);
        }
        else {
            this.transform.localPosition = new Vector3(2f,2f,0f);
        }
        
    }

    public override void CollectObservations(VectorSensor sensor) { //Observation vector of floats

        sensor.AddObservation(Enemy.localPosition);
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(EnemyAttack1.enabled);
        sensor.AddObservation(EnemyAttack2.enabled);
        sensor.AddObservation(EnemyParry.enabled);
    }

    public override void OnActionReceived(float[] vectorAction) { //Whenever Action Vector is issued this is called 
        
        int movement = Mathf.FloorToInt(vectorAction[0]);
        int action = Mathf.FloorToInt(vectorAction[0]);
        int direction = 0;

        if (movement == 1) { direction = -1; }
        if (movement == 2) { direction = 1; }

        if(!isInAction()) {
            if (action == 3) { Attack1(); }
            if (action == 4) { Attack2(); }
            if (action == 5) { Parry(); }
        }

        Move(direction);

    }

    public override void Heuristic(float[] actionsOut) {

        actionsOut[0] = 0;
        int input = Mathf.FloorToInt(Input.GetAxisRaw(axis));
        if (input == -1) { actionsOut[0] = 1; }
        if (input == 1) { actionsOut[0] = 2; }
        if ((Input.GetKey(KeyCode.J) && this.name == "Player 1") || (Input.GetKey(KeyCode.Delete) && this.name == "Player 2")) {
            actionsOut[0] = 3;
        }
        if ((Input.GetKey(KeyCode.K) && this.name == "Player 1") || (Input.GetKey(KeyCode.End) && this.name == "Player 2")) {
            actionsOut[0] = 4;
        }
        if ((Input.GetKey(KeyCode.L) && this.name == "Player 1") || (Input.GetKey(KeyCode.PageDown) && this.name == "Player 2")) {
            actionsOut[0] = 5;
        }
    }

    private void Move(int direction) { //Script allowing for movement of the agent

        if(isInAction() || direction == 0) {    //Restricting movement to only if no other action is happening
                                                //and adding fricetion to be non-movable by other agents
            rBody.sharedMaterial = friction;
            animator.SetFloat("Speed", Mathf.Abs(rBody.velocity.x));
            return;
        }

        rBody.sharedMaterial = noFriction;
        float movement_x = direction;

        //Adding velocity in direction and setting sprite animation
        rBody.velocity = new Vector2(movement_x * forceMultiplier, rBody.velocity.y);
        
        if ((rBody.velocity.x < 0 && this.name == "Player 1") || (rBody.velocity.x > 0 && this.name == "Player 2")) {
            forceMultiplier = 2.5f;
            animator.SetBool("Forward", false);

        }
        else if ((rBody.velocity.x > 0 && this.name == "Player 1") || (rBody.velocity.x < 0 && this.name == "Player 2")) {
            forceMultiplier = 4f;

            animator.SetBool("Forward", true);
        }

        animator.SetFloat("Speed", Mathf.Abs(rBody.velocity.x));
    }
 
    private bool isInAction() { //Check if agent is performing an action/animation
        return animator.GetCurrentAnimatorStateInfo(animator.GetLayerIndex("Base Layer")).IsTag("Action");
    }
    
    //Attacks trigger animations, whereas animations trigger hitboxes for attacks in the editor for set amount of time
    private void Attack1() {
        if(isInAction()) {
            return;
        }
        animator.SetTrigger("Attack");
    }
    private void Attack2() {
        if(isInAction()) {
            return;
        }
        animator.SetTrigger("Attack2");
    }
    private void Parry() {
        if(isInAction()) {
            return;
        }
        animator.SetTrigger("Parry");
    }
    public void ParryAttack() {
        AddReward(0.6f); //Because ParryAttack is conditional a reward is given when it's performed
        animator.SetTrigger("Parry_attack");
    }
    
}
