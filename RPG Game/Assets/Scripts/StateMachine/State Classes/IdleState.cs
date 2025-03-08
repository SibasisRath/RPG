using RPG.Attributes;
using RPG.Control;
using UnityEngine;

namespace RPG.FSM
{
    public class IdleState : IState
    {
        private AIController ai;
        private EnemyStateMachine stateMachine;
        private Health health;
        public IdleState(AIController ai, EnemyStateMachine stateMachine)
        {
            this.ai = ai;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter()
        {
            health = ai.GetHealth();
            ai.GetMover().Cancel();
        }
        public void Update()
        {
            if (ai.GetHealth().IsDead())
            {
                stateMachine.ChangeState(StateEnum.IDLE);
                return;
            }
            if (ai.IsAggrevated() && !health.IsDead())
            {
                stateMachine.ChangeState(StateEnum.CHASING);
                return;
            }
        }
        public void OnStateExit() { }
    }
}