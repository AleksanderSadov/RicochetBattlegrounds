using Unity.Ricochet.Game;
using UnityEngine;
using UnityEngine.AI;

namespace Unity.Ricochet.AI
{
    public class EnemyController : MonoBehaviour
    {
        public float delayBetweenAttacks = 0.5f;
        public float orientationSpeed = 10f;

        public Animator animator;
        public GameObject projectilePrefab;
        public Transform weaponPivot;

        public bool isSeeingTarget => sensorSightFov.isSeeingTarget;
        public bool isTargetInAttackRange => sensorSightFov.isTargetInAttackRange;
        public GameObject detectedTarget => sensorSightFov.detectedTarget;

        private NavMeshAgent navMeshAgent;
        private EnemySensorFindPlayer sensorFindPlayer;
        private EnemySensorSightFov sensorSightFov;
        private Damageable damageable;
        private EnemyManager enemyManager;
        private ScoreManager scoreManager;
        private int hashSpeed;
        private float lastTimeAttacked = 0;

        private void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            sensorFindPlayer = GetComponentInChildren<EnemySensorFindPlayer>();
            sensorSightFov = GetComponentInChildren<EnemySensorSightFov>();
            enemyManager = FindObjectOfType<EnemyManager>();
            scoreManager = FindObjectOfType<ScoreManager>();
            damageable = GetComponent<Damageable>();
            damageable.OnDie += OnDie;
            hashSpeed = Animator.StringToHash("Speed");
            animator.SetInteger("WeaponType", 1);
            enemyManager.EnemySpawned();
        }

        private void Update()
        {
            animator.SetFloat(hashSpeed, navMeshAgent.velocity.magnitude);
        }

        public void SetNavDestination(Vector3 destination)
        {
            if (navMeshAgent)
            {
                navMeshAgent.SetDestination(destination);
            }
        }

        public void FollowPlayer()
        {
            SetNavDestination(sensorFindPlayer.playerPosition);
        }

        public void OrientTowards(Vector3 lookPosition)
        {
            Vector3 lookDirection = Vector3.ProjectOnPlane(lookPosition - transform.position, Vector3.up).normalized;
            if (lookDirection.sqrMagnitude != 0f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * orientationSpeed);
            }
        }

        public void Attack()
        {
            if (Time.time - lastTimeAttacked < delayBetweenAttacks)
            {
                return;
            }

            GameObject bullet = Instantiate(projectilePrefab, weaponPivot.position, weaponPivot.rotation);
            bullet.transform.Rotate(0, Random.Range(-7.5f, 7.5f), 0);
            lastTimeAttacked = Time.time;
        }

        private void OnDie()
        {
            navMeshAgent.velocity = Vector3.zero;
            animator.SetBool("Dead", true);
            animator.transform.parent = null;
            scoreManager.AddScore(100);
            enemyManager.EnemyDied();
            Destroy(gameObject);
        }
    }
}
