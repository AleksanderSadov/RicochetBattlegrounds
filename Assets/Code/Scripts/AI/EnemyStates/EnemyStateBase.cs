using UnityEngine;

namespace Unity.Ricochet.AI
{
    public abstract class EnemyStateBase
    {
        protected Enemy enemy;

        public EnemyStateBase(Enemy enemy)
        {
            this.enemy = enemy;
        }

        public abstract void InitState();
        public abstract void UpdateState();
        public abstract void EndState();

        protected bool HasReachedMyDestination()
        {
            float distance = Vector3.Distance(enemy.transform.position, enemy.navMeshAgent.destination);
            if (distance <= 1.5f)
            {
                return true;
            }

            return false;
        }

        protected void RandomRotate()
        {
            float randomAngle = Random.Range(45, 180);
            float randomSign = Random.Range(0, 2);
            if (randomSign == 0)
            {
                randomAngle *= -1;
            }

            enemy.transform.Rotate(0, randomAngle, 0);
        }
    }
}
