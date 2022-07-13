using Unity.Ricochet.Game;
using UnityEngine;
using static Unity.Ricochet.AI.Enemy;

namespace Unity.Ricochet.AI
{
    public class EnemyStateInspect : EnemyStateBase
    {
        Timer inspectTimer;
        Timer inspectTurnTimer;
        bool inspectWait;

        public EnemyStateInspect(Enemy enemy) : base(enemy)
        {

        }

        public override void InitState()
        {
            if (inspectTimer == null)
            {
                inspectTimer = enemy.gameObject.AddComponent<Timer>();
            }
            if (inspectTurnTimer == null)
            {
                inspectTurnTimer = enemy.gameObject.AddComponent<Timer>();
            }

            enemy.navMeshAgent.speed = 16.0f;
            enemy.navMeshAgent.isStopped = false;
            inspectTimer.StopTimer();
            inspectWait = false;
        }

        public override void UpdateState()
        {
            if (HasReachedMyDestination() && !inspectWait)
            {
                inspectWait = true;
                inspectTimer.StartTimer(2.0f);
                inspectTurnTimer.StartTimer(1.0f);
            }

            enemy.navMeshAgent.SetDestination(enemy.targetPosition);

            RaycastHit hit = new RaycastHit();
            Physics.Raycast(enemy.transform.position, enemy.transform.forward, out hit, enemy.weaponRange, enemy.hitTestLayer);
            if (hit.collider != null && hit.collider.tag == "Player")
            {
                enemy.SetState(EnemyStateType.ATTACK);
            }
            if (inspectWait)
            {
                if (inspectTurnTimer.isFinished)
                {
                    RandomRotate();
                    inspectTurnTimer.StartTimer(Random.Range(0.5f, 1.25f));
                }
                if (inspectTimer.isFinished)
                {
                    enemy.SetState(enemy.idleState);
                }
            }
        }

        public override void EndState()
        {

        }
    }
}
