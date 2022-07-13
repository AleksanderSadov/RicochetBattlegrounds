using Unity.Ricochet.AI;
using Unity.Ricochet.Game;
using UnityEngine;
using UnityEngine.Events;

namespace Unity.Ricochet.Gameplay
{
    public enum PlayerWeaponType { KNIFE, PISTOL, NULL }

    public class PlayerBehavior : MonoBehaviour
    {
        public float moveSpeed = 10.0f;
        public Transform knifePivot, gunPivot;
        public GameObject mousePointer, projectilePrefab;
        public Animator animator;
        public UnityAction<PlayerWeaponType> OnWeaponChanged;

        [SerializeField] private GameCamera playerCamera;

        private Rigidbody myRigidBody;
        private int hashSpeed;
        private float attackTime = 0.4f;
        private PlayerWeaponType currentWeapon = PlayerWeaponType.NULL;
        private Timer attackTimer;
        private Damageable damageable;
        
        private void Awake()
        {
            InitTimers();
        }

        private void Start()
        {
            damageable = GetComponent<Damageable>();
            damageable.OnDie += OnDie;
            myRigidBody = GetComponent<Rigidbody>();
            hashSpeed = Animator.StringToHash("Speed");
            attackTimer.StartTimer(0.1f);
            SetWeapon(PlayerWeaponType.PISTOL);
        }

        private void Update()
        {
            HideCursor();
            SetAnimatorSpeed();
            HandleInput();
            HandleAim();
        }

        private void InitTimers()
        {
            if (attackTimer == null)
            {
                attackTimer = gameObject.AddComponent<Timer>();
            }
        }

        private void HideCursor()
        {
            if (Cursor.visible)
            {
                Cursor.visible = false;
            }
        }

        private void SetAnimatorSpeed()
        {
            animator.SetFloat(hashSpeed, myRigidBody.velocity.magnitude);
        }

        private void HandleInput()
        {
            HandleMovement();
            HandleWeaponSelection();
            HandleAttack();
        }

        private void HandleMovement()
        {
            float inputHorizontal = Input.GetAxis("Horizontal");
            float inputVertical = Input.GetAxis("Vertical");
            Vector3 newVelocity = new Vector3(inputVertical * moveSpeed, 0.0f, inputHorizontal * -moveSpeed);
            myRigidBody.velocity = newVelocity;
        }

        private void HandleAttack()
        {
            if (currentWeapon == PlayerWeaponType.NULL)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0) && attackTimer.isFinished)
            {
                Attack();
            }
        }

        private void HandleWeaponSelection()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetWeapon(PlayerWeaponType.KNIFE);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetWeapon(PlayerWeaponType.PISTOL);
            }
        }

        private void Attack()
        {
            switch (currentWeapon)
            {
                case PlayerWeaponType.KNIFE:
                    Invoke("KnifeHitCheck", 0.2f);
                    break;
                case PlayerWeaponType.PISTOL:
                    playerCamera.ToggleShake(0.1f);
                    GameObject bullet = Instantiate(projectilePrefab, gunPivot.position, gunPivot.rotation);
                    bullet.transform.LookAt(mousePointer.transform);
                    bullet.transform.Rotate(0, Random.Range(-7.5f, 7.5f), 0);
                    AlertEnemies();
                    break;
            }

            animator.SetBool("Attack", true);
            CancelInvoke("AttackOver");
            Invoke("AttackOver", attackTime);
            attackTimer.StartTimer(attackTime);
        }

        private void OnDie()
        {
            animator.SetBool("Dead", true);
            animator.transform.parent = null;
            enabled = false;
            myRigidBody.isKinematic = true;
            gameObject.GetComponent<Collider>().enabled = false;
            playerCamera.ToggleShake(0.3f);
            EventManager.Broadcast(new PlayerDeathEvent());
        }

        private void HandleAim()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.y = transform.position.y;
            mousePointer.transform.position = mousePos;
            float deltaY = mousePos.z - transform.position.z;
            float deltaX = mousePos.x - transform.position.x;
            float angleInDegrees = Mathf.Atan2(deltaY, deltaX) * 180 / Mathf.PI;
            transform.eulerAngles = new Vector3(0, -angleInDegrees, 0);
        }

        private void AlertEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(knifePivot.position, 20.0f, knifePivot.up);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != null && hit.collider.tag == "Enemy")
                {
                    hit.collider.GetComponent<Enemy>().SetAlertPosition(transform.position);
                }
            }
        }

        private void KnifeHitCheck()
        {
            RaycastHit[] hits = Physics.SphereCastAll(knifePivot.position, 2.0f, knifePivot.up);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != null && hit.collider.tag == "Enemy")
                {
                    RaycastHit forwarHit = new RaycastHit();
                    Physics.Raycast(knifePivot.position, hit.transform.position - transform.position, out forwarHit);
                    if (forwarHit.collider != null && forwarHit.collider.tag == "Enemy")
                    {
                        forwarHit.collider.GetComponent<Damageable>().Kill();
                    }
                }
            }
        }

        private void AttackOver()
        {
            animator.SetBool("Attack", false);
        }

        private void SetWeapon(PlayerWeaponType weaponType)
        {
            if (weaponType != currentWeapon)
            {
                currentWeapon = weaponType;
                animator.SetTrigger("WeaponChange");
                switch (weaponType)
                {
                    case PlayerWeaponType.KNIFE:
                        attackTime = 0.4f;
                        animator.SetInteger("WeaponType", 0);
                        break;
                    case PlayerWeaponType.PISTOL:
                        attackTime = 0.1f;
                        animator.SetInteger("WeaponType", 3);
                        break;
                }

                OnWeaponChanged.Invoke(weaponType);
            }
        }
    }
}

