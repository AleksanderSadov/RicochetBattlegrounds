namespace Unity.Ricochet.AI
{
    public class EnemyStateIdleStatic : EnemyStateBase
    {
        public EnemyStateIdleStatic(Enemy enemy) : base(enemy)
        {

        }

        public override void InitState()
        {
            enemy.navMeshAgent.SetDestination(enemy.startingPosition);
            enemy.navMeshAgent.isStopped = false;
        }

        public override void UpdateState()
        {

        }

        public override void EndState()
        {

        }
    }
}
