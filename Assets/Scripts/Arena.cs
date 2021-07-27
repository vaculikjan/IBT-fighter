using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour //Class used to reset arena when episode ends
{
    //Set in editor
    public PlayerMovement Player1;
    public PlayerMovement Player2;
    public PlayerHealthManager Health1;
    public PlayerHealthManager Health2;
    public float timer = 1500f;

    public void resetFight() {
        Player1.EndEpisode();
        Player2.EndEpisode();
        Health1.resetHealth();
        Health2.resetHealth();
        timer = 1500f;
    }

    private void FixedUpdate() {

        Player1.AddReward(-0.001f);
        Player2.AddReward(-0.001f);

        if (timer > 0) {
            timer -= Time.fixedDeltaTime;
        }

        else {
            Player1.SetReward(0);
            Player2.SetReward(0);
            resetFight();
        }
        
    }
}
