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
            currentState.UpdateState();
            animator.SetFloat(hashSpeed, navMeshAgent.velocity.magnitude);
        }

        public void SetState(EnemyStateType newStateType)
        {
            if (currentStateType == newStateType)
            {
                return;
            }

            EnemyStateBase newState;
            switch (newStateType)
            {
                case EnemyStateType.IDLE_STATIC:
                    newState = new EnemyStateIdleStatic(this);
                    break;
                case EnemyStateType.IDLE_ROAM:
                    newState = new EnemyStateIdleRoam(this);
                    break;
                case EnemyStateType.IDLE_PATROL:
                    newState = new EnemyStateIdlePatrol(this);
                    break;
                case EnemyStateType.INSPECT:
                    newState = new EnemyStateInspect(this);
                    break;
                case EnemyStateType.ATTACK:
                    newState = new EnemyStateAttack(this);
                    break;
                default:
                    newState = new EnemyStateIdleStatic(this);
                    break;
            }

            if (currentState != null)
            {
                currentState.EndState();
            }
            
            currentState = newState;
            currentState.InitState();
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

        private void AttackAction()
        {
            switch (weaponType)
            {
                case EnemyWeaponType.KNIFE:
                    RaycastHit[] hits = Physics.SphereCastAll(weaponPivot.position, 2.0f, weaponPivot.forward);
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider != null && hit.collider.tag == "Player")
                        {
                            hit.collider.GetComponent<Damageable>().Kill();
                        }
                    }
                    break;
                case EnemyWeaponType.RIFLE:
                    GameObject bullet = Instantiate(projectilePrefab, weaponPivot.position, weaponPivot.rotation);
                    bullet.transform.Rotate(0, Random.Range(-7.5f, 7.5f), 0);
                    break;
                case EnemyWeaponType.SHOTGUN:
                    for (int i = 0; i < 5; i++)
                    {
                        GameObject birdshot = Instantiate(projectilePrefab, weaponPivot.position, weaponPivot.rotation);
                        birdshot.transform.Rotate(0, Random.Range(-15, 15), 0);
                    }
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
