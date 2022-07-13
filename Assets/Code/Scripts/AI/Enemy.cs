using Unity.Ricochet.Game;
using UnityEngine;
using UnityEngine.AI;

namespace Unity.Ricochet.AI
{
    public class Enemy : MonoBehaviour
    {
        public enum EnemyStateType { IDLE_STATIC, IDLE_ROAM, IDLE_PATROL, INSPECT, ATTACK, FIND_WEAPON, KNOCKED_OUT, DEAD, NONE }
        public enum EnemyWeaponType { KNIFE, RIFLE, SHOTGUN }

        public float inspectTimeout;
        public NavMeshAgent navMeshAgent;
        public Animator animator;
        public GameObject projectilePrefab;
        public EnemyWeaponType weaponType = EnemyWeaponType.KNIFE;
        public EnemyStateType idleState = EnemyStateType.IDLE_ROAM;
        public LayerMask hitTestLayer;
        public Transform weaponPivot;
        public Vector3 targetPosition;
        public Vector3 startingPosition { get; private set; }
        public PatrolNode patrolNode;
        public float weaponRange, weaponActionTime, weaponTime;

        private EnemyStateType currentStateType = EnemyStateType.NONE;
        private EnemyStateBase currentState;
        private int hashSpeed;
        private Damageable damageable;
        private EnemyManager enemyManager;
        private ScoreManager scoreManager;

        private void Start()
        {
            enemyManager = FindObjectOfType<EnemyManager>();
            scoreManager = FindObjectOfType<ScoreManager>();
            damageable = GetComponent<Damageable>();
            damageable.OnDie += OnDie;
            startingPosition = transform.position;
            hashSpeed = Animator.StringToHash("Speed");
            enemyManager.EnemySpawned();
            SetWeapon(weaponType);
            SetState(idleState);
        }

        private void Update()
        {
            animator.SetFloat(hashSpeed, navMeshAgent.velocity.magnitude);
        }

        public void SetState(EnemyStateType newStateType)
        {
            if (currentStateType == newStateType)
            {
                return;
            }

            if (currentState != null)
            {
                Destroy(GetComponent(currentState.GetType()));
            }

            EnemyStateBase newState;
            switch (newStateType)
            {
                case EnemyStateType.IDLE_STATIC:
                    newState = gameObject.AddComponent<EnemyStateIdleStatic>();
                    break;
                case EnemyStateType.IDLE_ROAM:
                    newState = gameObject.AddComponent<EnemyStateIdleRoam>();
                    break;
                case EnemyStateType.IDLE_PATROL:
                    newState = gameObject.AddComponent<EnemyStateIdlePatrol>();
                    break;
                case EnemyStateType.INSPECT:
                    newState = gameObject.AddComponent<EnemyStateInspect>();
                    break;
                case EnemyStateType.ATTACK:
                    newState = gameObject.AddComponent<EnemyStateAttack>();
                    break;
                default:
                    newState = gameObject.AddComponent<EnemyStateIdleStatic>();
                    break;
            }

            currentState = newState;
            currentStateType = newStateType;
        }

        public void SetWeapon(EnemyWeaponType newWeapon)
        {
            animator.SetTrigger("WeaponChange");
            animator.SetInteger("WeaponType", (int)weaponType);
            switch (weaponType)
            {
                case EnemyWeaponType.KNIFE:
                    weaponRange = 1.0f;
                    weaponActionTime = 0.2f;
                    weaponTime = 0.4f;
                    break;
                case EnemyWeaponType.RIFLE:
                    weaponRange = 20.0f;
                    weaponActionTime = 0.025f;
                    weaponTime = 0.05f;
                    break;
                case EnemyWeaponType.SHOTGUN:
                    weaponRange = 20.0f;
                    weaponActionTime = 0.35f;
                    weaponTime = 0.75f;
                    break;
            }
        }

        ////////////////////////// PUBLIC FUNCTIONS //////////////////////////
        public void SetAlertPos(Vector3 newPos)
        {
            if (idleState != EnemyStateType.IDLE_STATIC)
            {
                SetTargetPos(newPos);
            }
        }
        public void SetTargetPos(Vector3 newPos)
        {
            targetPosition = newPos;
            if (currentStateType != EnemyStateType.ATTACK)
            {
                SetState(EnemyStateType.INSPECT);
            }
        }
        public void OnDie()
        {
            navMeshAgent.velocity = Vector3.zero;
            //navMeshAgent.Stop ();
            animator.SetBool("Dead", true);
            scoreManager.AddScore(100);
            animator.transform.parent = null;
            Vector3 pos = animator.transform.position;
            pos.y = 0.2f;
            animator.transform.position = pos;
            enemyManager.EnemyDied();
            Destroy(gameObject);
        }

    }
}
