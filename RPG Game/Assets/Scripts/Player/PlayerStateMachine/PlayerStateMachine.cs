using RPG.Controller;
using UnityEngine;

public class PlayerStateMachine : GenericStateMachine<PlayerController>
{
    private PlayerController player;

    public PlayerStateMachine(PlayerController owner) : base(owner) 
    {
        //SetState(new MovementState(stateMachine, GetComponent<CharacterController>()));
    }
}
