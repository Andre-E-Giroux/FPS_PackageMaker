using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : Universal
{
    protected float _horizontalInput;
    protected float _verticalInput;

    protected float _speedModifier;

    public Grounded(string name, PlayerSM stateMachine) : base (name, stateMachine) 
    {
        _sm = (PlayerSM)stateMachine;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        _sm.Move(_speedModifier); 

        /*
                if (Input.GetKeyDown(KeyCode.Space))
                    stateMachine.ChangeState(_sm.jumpState);
        */

        if (Input.GetKey(KeyCode.LeftShift) && (Mathf.Abs(_horizontalInput) > Mathf.Epsilon || Mathf.Abs(_verticalInput) > Mathf.Epsilon))
        {
            _sm.CrouchPlayer(false);
            stateMachine.ChangeState(((PlayerSM)stateMachine).standingRunningState);
        }

    }

}
