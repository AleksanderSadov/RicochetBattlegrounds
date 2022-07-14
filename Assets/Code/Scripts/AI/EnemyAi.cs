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
            Evade,
        }

        public AIState aiState;

        private EnemyController enemyController;

        private void Start()
        {
            enemyController = GetComponent<EnemyController>();
            enemyController.onDangerDetected += OnDangerDetected;
            enemyController.onEvadeComplete += OnEvadeComplete;
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
                    }
                    break;
                case AIState.Attack:
                    if (!enemyController.isTargetInAttackRange || !enemyController.isSeeingTarget)
                    {
                        aiState = AIState.Follow;
                        enemyController.animator.SetBool("Attack", false);
                    }
                    break;
                case AIState.Evade:
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
                    enemyController.TryAttack();
                    break;
                case AIState.Evade:
                    if (!enemyController.isEvading && enemyController.detectedBullet)
                    {
                        enemyController.TryEvade(enemyController.detectedBullet.transform.forward);
                    }
                    break;
            }
        }

        private void OnDangerDetected()
        {
            if (Time.time - enemyController.lastTimeEvaded < enemyController.delayBetweenEvades)
            {
                return;
            }

            aiState = AIState.Evade;
        }

        private void OnEvadeComplete()
        {
            aiState = AIState.Follow;
        }
    }
}

