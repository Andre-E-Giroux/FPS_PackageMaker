using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingIdle : Grounded
{

    private float _horizontalInput;
    private float _verticalInput;

    public StandingIdle(PlayerSM stateMachine) : base ("StandingIdle", stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        _sm.CrouchPlayer(false);
        _horizontalInput = 0f;
        _sm.meshRenderer.material.color = Color.black;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        // floats are not accurate use abs and epsilon to verify if float is 0 or is close enough to it.
        if (Mathf.Abs(_horizontalInput) > Mathf.Epsilon || Mathf.Abs(_verticalInput) > Mathf.Epsilon) 
        {
            stateMachine.ChangeState(((PlayerSM)stateMachine).standingMovingState);
        }
    }

}
