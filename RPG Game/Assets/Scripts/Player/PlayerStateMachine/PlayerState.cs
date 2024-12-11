using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : IState
{
    protected PlayerStateMachine stateMachine;
    protected CharacterController characterController;

    public PlayerState(PlayerStateMachine stateMachine, CharacterController characterController)
    {
        this.stateMachine = stateMachine;
        this.characterController = characterController;
    }

    public virtual void Enter() { }
    public virtual void HandleInput() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}
