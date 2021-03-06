using System.Collections;
using Unity.Ricochet.Game;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Unity.Ricochet.AI
{
    public class EnemyController : MonoBehaviour
    {
        public float delayBetweenAttacks = 0.5f;
        public float delayBetweenEvades = 1.0f;
        public float evadeDistance = 10f;
        public float evadeDuration = 0.5f;

        public bool isEvading = false;
        public float lastTimeAttacked = 0;
        public float lastTimeEvaded = 0;

        public Animator animator;
        public GameObject projectilePrefab;
        public Transform weaponPivot;

        public UnityAction onDangerDetected;
        public UnityAction onEvadeComplete;

        public GameObject detectedTarget => sensorSightFov.detectedTarget;
        public bool isSeeingTarget => sensorSightFov.isSeeingTarget;
        public bool isTargetInAttackRange => sensorSightFov.isTargetInAttackRange;
        public bool isTargetInAttackAngle => sensorSightFov.isTargetInAttackAngle;
        public bool isDangerDetected => sensorDanger.isDangerDetected;
        public GameObject detectedBullet => sensorDanger.detectedBullet;
        
        private NavMeshAgent navMeshAgent;
        private EnemySensorFindPlayer sensorFindPlayer;
        private EnemySensorSightFov sensorSightFov;
        private EnemySensorDanger sensorDanger;
        private Damageable damageable;
        private EnemyManager enemyManager;
        private int hashSpeed;
        private ClipAudio footstepsAudio;
        private ClipAudio gunAudio;

        private void Start()
        {
            footstepsAudio = GetComponent<ClipAudio>();
            gunAudio = weaponPivot.GetComponent<ClipAudio>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            sensorFindPlayer = GetComponentInChildren<EnemySensorFindPlayer>();
            sensorSightFov = GetComponentInChildren<EnemySensorSightFov>();
            sensorDanger = GetComponentInChildren<EnemySensorDanger>();
            sensorDanger.onDangerDetected += OnDangerDetected;
            enemyManager = FindObjectOfType<EnemyManager>();
            damageable = GetComponent<Damageable>();
            damageable.OnDie += OnDie;
            hashSpeed = Animator.StringToHash("Speed");
            animator.SetInteger("WeaponType", 1);
            enemyManager.EnemySpawned();
        }

        private void Update()
        {
            float speed = navMeshAgent.velocity.magnitude;

            animator.SetFloat(hashSpeed, navMeshAgent.velocity.magnitude);

            if (speed > 0)
            {
                footstepsAudio.SetVolume(0.25f);
            }
            else
            {
                footstepsAudio.SetVolume(0f);
            }
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
                transform.rotation = targetRotation;
            }
        }

        public void TryAttack()
        {
            if (Time.time - lastTimeAttacked < delayBetweenAttacks || !isTargetInAttackAngle)
            {
                return;
            }

            gunAudio.PlayRandomClipOnce();
            animator.SetBool("Attack", true);
            GameObject bullet = Instantiate(projectilePrefab, weaponPivot.position, weaponPivot.rotation);
            bullet.transform.Rotate(0, Random.Range(-7.5f, 7.5f), 0);
            lastTimeAttacked = Time.time;
        }

        public void TryEvade(Vector3 bulletDirection)
        {
            isEvading = true;

            Vector3 evadeVector = Vector3.Cross(bulletDirection, Vector3.up) * evadeDistance;
            SetNavDestination(transform.position + evadeVector);

            lastTimeEvaded = Time.time;
            StartCoroutine(WaitEvadeDuration());
        }

        public void TryNewAttackPosition()
        {
            if (!HasReachedDestination())
            {
                return;
            }

            float detectionRange = sensorSightFov.detectionRange;
            Vector3 randomDirection = new Vector3(Random.Range(detectionRange / 2, detectionRange), transform.position.y, Random.Range(detectionRange / 2, detectionRange));
            float randomSign = Random.Range(0f, 1f) >= 0.5f ? 1f : -1f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(detectedTarget.transform.position + randomDirection * randomSign, out hit, sensorSightFov.detectionRange, 1 << NavMesh.GetAreaFromName("Walkable")))
            {
                SetNavDestination(hit.position);
            }
        }

        private bool HasReachedDestination()
        {
            if (!navMeshAgent.pathPending
                && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance
                && (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f))
            {
                return true;
            }

            return false;
        }

        private void OnDangerDetected()
        {
            onDangerDetected?.Invoke();
        }

        private IEnumerator WaitEvadeDuration()
        {
            yield return new WaitForSeconds(evadeDuration);
            isEvading = false;
            onEvadeComplete?.Invoke();
        }

        private void OnDie()
        {
            navMeshAgent.velocity = Vector3.zero;
            animator.SetBool("Dead", true);
            animator.transform.parent = null;
            enemyManager.EnemyDied();
            Destroy(gameObject);
        }
    }
}
