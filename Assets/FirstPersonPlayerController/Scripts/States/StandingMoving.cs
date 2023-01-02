using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// State for player movement while standing
/// </summary>
public class StandingMoving : Grounded
{
    private static float WEAPON_ACCURACY_MODIFIER = 1.3f;


    public StandingMoving(PlayerSM stateMachine) : base ("StandingMoving", stateMachine) 
    {}


    public override void Enter()
    {
        base.Enter();
        _speedModifier = _sm.defaultSpeed;
        _horizontalInput = 0f;
        _sm.meshRenderer.material.color = Color.red;
        ((PlayerSM)stateMachine).wInteraction.UpdateWeaponFromPlayerState(WEAPON_ACCURACY_MODIFIER);

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(_horizontalInput) < Mathf.Epsilon && Mathf.Abs(_verticalInput) < Mathf.Epsilon)
        {
            stateMachine.ChangeState(_sm.standingIdleState);
        }


    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
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
