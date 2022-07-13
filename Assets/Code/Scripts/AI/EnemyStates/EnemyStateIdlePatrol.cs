namespace Unity.Ricochet.AI
{
    public class EnemyStateIdlePatrol : EnemyStateBase
    {
        protected override void Start()
        {
            base.Start();

            if (enemy.patrolNode == null)
            {
                return;
            }

            enemy.navMeshAgent.speed = 6.0f;
            enemy.navMeshAgent.SetDestination(enemy.patrolNode.GetPosition());
        }

        protected override void Update()
        {
            base.Update();

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
    }
}
