using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping : Universal
{
   

    // get layer number 6, Ground
    private int _groundLayer = 1 << 6;


    private float _groundDetectionDistance = 0.2f;

    public Jumping(PlayerSM stateMachine) : base ("Jumping", stateMachine) 
    {
        _sm = (PlayerSM)stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _sm.meshRenderer.material.color = Color.green;

        if (_sm.grounded)
        {
            Vector3 vel = _sm.rb.velocity;
            vel.y += _sm.jumpForce;
            _sm.rb.velocity = vel;
        }
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

        Debug.Log("Update jump");
        if (Physics.Raycast(_sm.transform.position, _sm.transform.TransformDirection(Vector3.down), _groundDetectionDistance, _groundLayer))
        {
            Debug.Log("Verify grounded");
            _sm.grounded = _sm.rb.velocity.y < Mathf.Epsilon;
        }
        else
        {
            _sm.grounded = false;
        }


        

        Debug.DrawRay(_sm.transform.position, _sm.transform.TransformDirection(Vector3.down) * (_groundDetectionDistance), Color.red);




    }

}
