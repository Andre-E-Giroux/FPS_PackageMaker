using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingRunning : Grounded
{
    private static float WEAPON_ACCURACY_MODIFIER = 1.6f;


    public StandingRunning(PlayerSM stateMachine) : base ("StandingRunning", stateMachine) 
    {}


    public override void Enter()
    {
        base.Enter();
        _speedModifier = _sm.runningSpeed;
        _horizontalInput = 0f;
        _sm.meshRenderer.material.color = Color.red;
        ((PlayerSM)stateMachine).wInteraction.UpdateWeaponFromPlayerState(WEAPON_ACCURACY_MODIFIER);

    }

    public override void UpdateLogic()
    {
        Debug.Log("In running state!");

        base.UpdateLogic();
        //_sm.Move(_sm.runningSpeed);


        
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(_horizontalInput) < Mathf.Epsilon && Mathf.Abs(_verticalInput) < Mathf.Epsilon)
        {
            stateMachine.ChangeState(_sm.standingIdleState);
        }
        else if(!Input.GetKey(KeyCode.LeftShift))
        {
            stateMachine.ChangeState(_sm.standingMovingState);
        }
        
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        /*
        Vector2 xMov = new Vector2(_horizontalInput * _sm.transform.right.x, _horizontalInput * _sm.transform.right.z);
        Vector2 zMov = new Vector2(_verticalInput * _sm.transform.forward.x, _verticalInput * _sm.transform.forward.z);


        Vector2 velo = (xMov + zMov).normalized * _sm.runningSpeed;


        _sm.rb.velocity = new Vector3(velo.x, _sm.rb.velocity.y, velo.y);*/
    }


    public override void Exit()
    {
        base.Exit();
    }

    public override float GetWeaponAccuracyModifer()
    {
        return WEAPON_ACCURACY_MODIFIER;
    }
}
