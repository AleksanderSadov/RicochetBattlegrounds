namespace Unity.Ricochet.AI
{
    public class EnemyStateIdleStatic : EnemyStateBase
    {
        protected override void Start()
        {
            base.Start();

            enemy.navMeshAgent.SetDestination(enemy.startingPosition);
            enemy.navMeshAgent.isStopped = false;
        }
    }
}
