using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.FSM
{
    public class FleeState : IState
    {
        private AIController ai;
        private EnemyStateMachine stateMachine;
        private Health health;

        public FleeState(AIController ai, EnemyStateMachine stateMachine)
        {
            this.ai = ai;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter()
        {
            health = ai.GetHealth();
        }

        public void Update()
        {
            if (!health.IsDead())
            {
                ai.GetMover().StartMoveAction(ai.transform.position + (ai.transform.position - ai.GetTarget().transform.position).normalized * 10f, 1f);
            }
            else
            {
                stateMachine.ChangeState(StateEnum.IDLE);
                return;
            }
            
        }
        public void OnStateExit() { }
    }
}