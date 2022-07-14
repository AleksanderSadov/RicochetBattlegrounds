using UnityEngine;

namespace Unity.Ricochet.AI
{
    [RequireComponent(typeof(EnemyController))]

    public class EnemyAi : MonoBehaviour
    {
        public enum AIState
        {
            Follow,
            Attack,
        }

        public AIState aiState;

        private EnemyController enemyController;

        private void Start()
        {
            enemyController = GetComponent<EnemyController>();
            aiState = AIState.Follow;
        }

        private void Update()
        {
            UpdateAiStateTransitions();
            UpdateCurrentAiState();
        }

        private void UpdateAiStateTransitions()
        {
            switch (aiState)
            {
                case AIState.Follow:
                    if (enemyController.isSeeingTarget && enemyController.isTargetInAttackRange)
                    {
                        aiState = AIState.Attack;
                        enemyController.SetNavDestination(transform.position);
                        enemyController.animator.SetBool("Attack", true);
                    }
                    break;
                case AIState.Attack:
                    if (!enemyController.isTargetInAttackRange || !enemyController.isSeeingTarget)
                    {
                        aiState = AIState.Follow;
                        enemyController.animator.SetBool("Attack", false);
                    }
                    break;
            }
        }

        private void UpdateCurrentAiState()
        {
            switch (aiState)
            {
                case AIState.Follow:
                    enemyController.FollowPlayer();
                    break;
                case AIState.Attack:
                    enemyController.OrientTowards(enemyController.detectedTarget.transform.position);
                    enemyController.Attack();
                    break;
            }
        }
    }
}

