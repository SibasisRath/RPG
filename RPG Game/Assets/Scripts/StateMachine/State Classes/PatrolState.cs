using RPG.Control;
using RPG.Movement;
using UnityEngine;

namespace RPG.FSM
{
    public class PatrolState : IState
    {
        private AIController ai;
        private EnemyStateMachine stateMachine;
        private Vector3 currentWaypoint;
        private Vector3 nextWaypoint;
        private int currentWaypointIndex = 0;
 

        private Mover mover;
        private PatrolPath patrolPath;
        private AIConfig aiConfig;

        public PatrolState(AIController ai, EnemyStateMachine stateMachine)
        {
            this.ai = ai;
            this.stateMachine = stateMachine;
        }

        public void OnStateEnter()
        {
            mover = ai.GetMover();
            patrolPath = ai.GetPatrolPath();
            aiConfig = ai.GetAIConfig();

            if (patrolPath != null)
            {
                currentWaypoint = patrolPath.GetWaypoint(currentWaypointIndex);
                nextWaypoint = patrolPath.GetWaypoint(patrolPath.GetNextIndex(currentWaypointIndex));
            }
        }

        public void Update()
        {
            if (ai.GetHealth().IsDead())
            {
                stateMachine.ChangeState(StateEnum.IDLE);
                return;
            }

            if (ai.IsAggrevated() && ai.GetFighter().CanAttack(ai.GetTarget().gameObject))
            {
                stateMachine.ChangeState(StateEnum.ATTACKING);
                return;
            }
            if (ai.IsAggrevated() && !ai.GetTarget().IsDead())
            {
                stateMachine.ChangeState(StateEnum.CHASING);
                return;
            }

           

            PatrolBehaviour();
        }

        public void OnStateExit() { }

        private void PatrolBehaviour()
        {
            if (patrolPath == null) return;

            if (AtWaypoint())
            {
                ai.TimeSinceArrivedAtWaypoint = 0;
                CycleWaypoint();
            }

            if (ai.TimeSinceArrivedAtWaypoint > aiConfig.waypointDwellTime)
            {
                mover.StartMoveAction(currentWaypoint, aiConfig.patrolSpeedFraction);
            }
        }

        private bool AtWaypoint() => Vector3.Distance(ai.transform.position, currentWaypoint) < aiConfig.waypointTolerance;

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
            currentWaypoint = nextWaypoint; // Use cached next waypoint
            nextWaypoint = patrolPath.GetWaypoint(patrolPath.GetNextIndex(currentWaypointIndex)); // Pre-cache next waypoint
        }
    }
}