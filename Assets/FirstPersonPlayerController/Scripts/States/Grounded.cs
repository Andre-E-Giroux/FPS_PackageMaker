using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounded : Universal
{


    public Grounded(string name, PlayerSM stateMachine) : base (name, stateMachine) 
    {
        _sm = (PlayerSM)stateMachine;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (Input.GetKeyDown(KeyCode.Space))
            stateMachine.ChangeState(_sm.jumpState);
       
    }

}
