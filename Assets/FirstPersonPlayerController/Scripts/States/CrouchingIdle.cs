using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchingIdle : Grounded
{

    private static float WEAPON_ACCURACY_MODIFIER = 0.8f;

    public CrouchingIdle(PlayerSM stateMachine) : base ("CrouchIdle", stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _sm.CrouchPlayer(true);
        _horizontalInput = 0f;
        _sm.meshRenderer.material.color = Color.black;
        ((PlayerSM)stateMachine).wInteraction.UpdateWeaponFromPlayerState(WEAPON_ACCURACY_MODIFIER);

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        // floats are not accurate use abs and epsilon to verify if float is 0 or is close enough to it.
        if (Mathf.Abs(_horizontalInput) > Mathf.Epsilon || Mathf.Abs(_verticalInput) > Mathf.Epsilon) 
        {
            stateMachine.ChangeState(((PlayerSM)stateMachine).crouchMovingState);
        }
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
