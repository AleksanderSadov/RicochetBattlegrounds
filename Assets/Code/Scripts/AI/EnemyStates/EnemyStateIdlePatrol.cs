namespace Unity.Ricochet.AI
{
    public class EnemyStateIdlePatrol : EnemyStateBase
    {
        public EnemyStateIdlePatrol(Enemy enemy) : base(enemy)
        {

        }

        public override void InitState()
        {
            if (enemy.patrolNode == null)
            {
                return;
            }

            enemy.navMeshAgent.speed = 6.0f;
            enemy.navMeshAgent.SetDestination(enemy.patrolNode.GetPosition());
        }

        public override void UpdateState()
        {
            if (enemy.patrolNode == null)
            {
                return;
            }

            if (HasReachedMyDestination())
            {
                enemy.patrolNode = enemy.patrolNode.nextNode;
                enemy.navMeshAgent.SetDestination(enemy.patrolNode.GetPosition());
            }
        }

        public override void EndState()
        {

        }
    }
}
