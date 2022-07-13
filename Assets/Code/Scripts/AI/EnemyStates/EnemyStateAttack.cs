using Unity.Ricochet.Game;
using UnityEngine;
using static Unity.Ricochet.AI.Enemy;

namespace Unity.Ricochet.AI
{
    public class EnemyStateAttack : EnemyStateBase
    {
        private Timer attackActionTimer;
        private bool actionDone;

        public EnemyStateAttack(Enemy enemy) : base(enemy)
        {

        }

        public override void InitState()
        {
            if (attackActionTimer == null)
            {
                attackActionTimer = enemy.gameObject.AddComponent<Timer>();
            }

            enemy.navMeshAgent.isStopped = true;
            enemy.navMeshAgent.velocity = Vector3.zero;
            enemy.animator.SetBool("Attack", true);
            enemy.CancelInvoke("AttackAction");
            enemy.Invoke("AttackAction", enemy.weaponActionTime);
            attackActionTimer.StartTimer(enemy.weaponTime);

            actionDone = false;
        }

        public override void UpdateState()
        {
            if (!actionDone && attackActionTimer.isFinished)
            {
                EndAttack();

                actionDone = true;
            }
        }

        public override void EndState()
        {
            enemy.animator.SetBool("Attack", false);
        }

        private void EndAttack()
        {
            enemy.SetState(EnemyStateType.INSPECT);
        }
    }
}
