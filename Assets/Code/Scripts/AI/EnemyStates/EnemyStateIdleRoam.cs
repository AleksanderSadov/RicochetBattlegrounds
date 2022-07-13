using Unity.Ricochet.Game;
using UnityEngine;

namespace Unity.Ricochet.AI
{
    public class EnemyStateIdleRoam : EnemyStateBase
    {
        Timer idleTimer;
        Timer idleRotateTimer;
        bool idleWaiting, idleMoving;

        protected override void Start()
        {
            base.Start();

            if (idleTimer == null)
            {
                idleTimer = enemy.gameObject.AddComponent<Timer>();
                idleTimer.timerName = "idleTimer";
            }
            if (idleRotateTimer == null)
            {
                idleRotateTimer = enemy.gameObject.AddComponent<Timer>();
                idleRotateTimer.timerName = "idleRotateTimer";
            }

            enemy.navMeshAgent.speed = 7.0f;
            idleTimer.StartTimer(Random.Range(2.0f, 4.0f));
            idleWaiting = false;
            idleMoving = true;
            RandomRotate();
            AdvanceIdle();
        }

        protected override void Update()
        {
            base.Update();

            if (idleMoving)
            {
                if (HasReachedMyDestination())
                {
                    AdvanceIdle();
                }
            }
            else if (idleWaiting)
            {
                if (idleRotateTimer.isFinished)
                {
                    RandomRotate();
                    idleRotateTimer.StartTimer(Random.Range(1.5f, 3.25f));
                }

            }
            if (idleTimer.isFinished)
            {
                if (idleMoving)
                {
                    enemy.navMeshAgent.isStopped = true;
                    float waitTime = Random.Range(2.5f, 6.5f);
                    float randomTurnTime = waitTime / 2.0f;
                    idleRotateTimer.StartTimer(randomTurnTime);
                    idleTimer.StartTimer(waitTime);
                }
                else if (idleWaiting)
                {
                    idleTimer.StartTimer(Random.Range(2.0f, 4.0f));
                    AdvanceIdle();
                }

                idleMoving = !idleMoving;
                idleWaiting = !idleMoving;
            }
        }

        protected override void OnDestroy()
        {
            DestroyTimers(idleTimer.timerName, idleRotateTimer.timerName);
            base.OnDestroy();
        }

        private void AdvanceIdle()
        {
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(enemy.transform.position, enemy.transform.forward * 5.0f, out hit, 50.0f, enemy.hitTestLayer);

            if (hit.distance < 3.0f)
            {
                Vector3 dir = hit.point - enemy.transform.position;
                Vector3 reflectedVector = Vector3.Reflect(dir, hit.normal);
                Physics.Raycast(enemy.transform.position, reflectedVector, out hit, 50.0f, enemy.hitTestLayer);
            }

            enemy.navMeshAgent.isStopped = true;
            enemy.navMeshAgent.SetDestination(hit.point);
        }
    }
}
