using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : Universal
{
   

    // get layer number 6, Ground
    private int _groundLayer = 1 << 6;


    private float _groundDetectionDistance = 0.2f;
    private static float WEAPON_ACCURACY_MODIFIER = 2f;



    public Jumping(PlayerSM stateMachine) : base ("Jumping", stateMachine) 
    {
        _sm = (PlayerSM)stateMachine;
    }

    public override void Enter()
    {
        Debug.Log("Entering jumping state");
        base.Enter();
        _sm.meshRenderer.material.color = Color.green;

        if (_sm.grounded)
        {
          //  Vector3 vel = _sm.rb.velocity;
         //   vel.y += _sm.jumpForce;
          //  _sm.rb.velocity = vel;
        }

        ((PlayerSM)stateMachine).wInteraction.UpdateWeaponFromPlayerState(WEAPON_ACCURACY_MODIFIER);

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();


        if (_sm.grounded)
        {
            if(!_sm.isCrouching)
                stateMachine.ChangeState(_sm.standingIdleState);
            else
                stateMachine.ChangeState(_sm.crouchIdleState);

        }

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();



    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("Exiting jumping state. is grounded: " + _sm.grounded);

    }

    public override float GetWeaponAccuracyModifer()
    {
        return WEAPON_ACCURACY_MODIFIER;
    }
}
