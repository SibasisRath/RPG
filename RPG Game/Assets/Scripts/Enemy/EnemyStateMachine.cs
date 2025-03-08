using RPG.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.FSM
{
    public class EnemyStateMachine : GenericStateMachine<AIController>
    {
        public EnemyStateMachine(AIController owner) : base(owner) { }

        protected override void InitializeStates()
        {
            States[StateEnum.PATROLLING] = new PatrolState(Owner, this);
            States[StateEnum.CHASING] = new ChaseState(Owner, this);
            States[StateEnum.ATTACKING] = new AttackState(Owner, this);
            States[StateEnum.IDLE] = new IdleState(Owner, this);
            States[StateEnum.SUSPICION] = new SuspicionState(Owner, this);
            States[StateEnum.FLEEING] = new FleeState(Owner, this);

            ChangeState(StateEnum.PATROLLING);
        }

        public void LoadState(StateEnum savedState)
        {
            if (States.ContainsKey(savedState))
            {
                ChangeState(savedState);
            }
            else
            {
                ChangeState(StateEnum.IDLE);
            }
        }
    }
}