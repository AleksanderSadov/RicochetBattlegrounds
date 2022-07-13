using Unity.Ricochet.Game;
using UnityEngine;
using static Unity.Ricochet.AI.Enemy;

namespace Unity.Ricochet.AI
{
    public class EnemyStateAttack : EnemyStateBase
    {
        private Timer attackActionTimer;
        private bool actionDone;

        protected override void Start()
        {
            base.Start();

            if (attackActionTimer == null)
            {
                attackActionTimer = enemy.gameObject.AddComponent<Timer>();
                attackActionTimer.timerName = "attackActionTimer";
            }

            enemy.navMeshAgent.isStopped = true;
            enemy.navMeshAgent.velocity = Vector3.zero;
            enemy.animator.SetBool("Attack", true);
            CancelInvoke("AttackAction");
            Invoke("AttackAction", enemy.weaponActionTime);
            attackActionTimer.StartTimer(enemy.weaponTime);

            actionDone = false;
        }

        protected override void Update()
        {
            base.Update();

            if (!actionDone && attackActionTimer.isFinished)
            {
                enemy.SetState(EnemyStateType.INSPECT);
                actionDone = true;
            }
        }

        protected override void OnDestroy()
        {
            DestroyTimers(attackActionTimer.timerName);
            enemy.animator.SetBool("Attack", false);
            base.OnDestroy();
        }

        private void AttackAction()
        {
            switch (enemy.weaponType)
            {
                case EnemyWeaponType.KNIFE:
                    RaycastHit[] hits = Physics.SphereCastAll(enemy.weaponPivot.position, 2.0f, enemy.weaponPivot.forward);
                    foreach (RaycastHit hit in hits)
                    {
                        if (hit.collider != null && hit.collider.tag == "Player")
                        {
                            hit.collider.GetComponent<Damageable>().Kill();
                        }
                    }
                    break;
                case EnemyWeaponType.RIFLE:
                    GameObject bullet = Instantiate(enemy.projectilePrefab, enemy.weaponPivot.position, enemy.weaponPivot.rotation);
                    bullet.transform.Rotate(0, Random.Range(-7.5f, 7.5f), 0);
                    break;
                case EnemyWeaponType.SHOTGUN:
                    for (int i = 0; i < 5; i++)
                    {
                        GameObject birdshot = Instantiate(enemy.projectilePrefab, enemy.weaponPivot.position, enemy.weaponPivot.rotation);
                        birdshot.transform.Rotate(0, Random.Range(-15, 15), 0);
                    }
                    break;
            }
        }
    }
}
