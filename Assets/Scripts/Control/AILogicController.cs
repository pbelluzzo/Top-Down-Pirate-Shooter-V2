using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;
using Pathfinding;

namespace Control
{
    public class AILogicController : MonoBehaviour
    {
        [SerializeField] float timeBeforeInitialization = 1f;
        [SerializeField] float waypointDetectionDistance = 0.5f;
        [SerializeField] float minimumMoveDistance = 1f;
        [SerializeField] bool hasCannon;
        [SerializeField] float shootRange = 5f;

        public delegate void NotifyActionValue(float value);
        public delegate void NotifyAction();

        public event NotifyActionValue MovementAction;
        public event NotifyActionValue RotateAction;
        public event NotifyAction ShootAction;

        Vector3[] path;
        Transform target;
        int targetIterator;
        float timeSinceLastPath;
        ShipController shipController;

        enum BehaviourCycle { requestingPath, followingPath, chasingPlayer, attackingPlayer }
        BehaviourCycle currentLoop;

        private void Awake()
        {
            shipController = GetComponent<ShipController>();
            target = GameplayManager.GetInstance().GetPlayerTransform();
            RegisterInputEvents();
        }

        void RegisterInputEvents()
        {
            MovementAction += shipController.OnMove;
            RotateAction += shipController.OnRotate;
            ShootAction += shipController.OnShoot;
        }

        private void OnEnable()
        {
            StartCoroutine(Initialize());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        IEnumerator Initialize()
        {
            yield return new WaitForSeconds(timeBeforeInitialization);
            currentLoop = BehaviourCycle.requestingPath;
            SwitchLoop();
        }

        private void Update()
        {
            if (currentLoop == BehaviourCycle.followingPath || currentLoop == BehaviourCycle.requestingPath)
            {
                RequestNewPathOverTime();
                CheckForPlayerInLineOfSight();
            }

        }

        void SwitchLoop()
        {
            switch (currentLoop)
            {
                case BehaviourCycle.requestingPath:
                    targetIterator = 0;
                    StopAllCoroutines();
                    PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                    break;

                case BehaviourCycle.followingPath:
                    StopAllCoroutines();
                    StartCoroutine(FollowPath());
                    break;

                case BehaviourCycle.chasingPlayer:
                    StopAllCoroutines();
                    StartCoroutine(ChasePlayer());
                    break;
            }
        }

        private void RequestNewPathOverTime()
        {
            timeSinceLastPath += Time.deltaTime;
            if (timeSinceLastPath >= 1f)
            {
                ChangeLoopBehaviour(BehaviourCycle.requestingPath);
                timeSinceLastPath = 0f;
            }
        }

        void ChangeLoopBehaviour(BehaviourCycle nextState)
        {
            currentLoop = nextState;
            SwitchLoop();
        }

        private void CheckForPlayerInLineOfSight()
        {
            Debug.DrawRay(transform.position, target.position - transform.position, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position);

            if (hit.collider.CompareTag("Player") && currentLoop == BehaviourCycle.followingPath)
            {
                ChangeLoopBehaviour(BehaviourCycle.chasingPlayer);
            }
        }

        public void OnPathFound(Vector3[] newPath, bool pathSuccessfull)
        {
            if (pathSuccessfull)
            {
                path = newPath;
                ChangeLoopBehaviour(BehaviourCycle.followingPath);
            }
        }

        IEnumerator FollowPath()
        {
            Vector3 currentWaypoint = Vector3.zero;

            if (path[0] != null)
            {
                currentWaypoint = path[0];
            }
            else
                yield return null;

            while (currentLoop == BehaviourCycle.followingPath)
            {
                if (Vector2.Distance(transform.position, currentWaypoint) < waypointDetectionDistance)
                {
                    targetIterator++;
                    if (targetIterator >= path.Length)
                    {
                        ChangeLoopBehaviour(BehaviourCycle.requestingPath);
                        SwitchLoop();
                        StopCoroutine(FollowPath());
                        yield break;
                    }
                    currentWaypoint = path[targetIterator];
                }

                MoveTowardsPoint(currentWaypoint);
                yield return null;
            }
        }
        IEnumerator ChasePlayer()
        {
            while (currentLoop == BehaviourCycle.chasingPlayer)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position);
                if (!hit.collider.CompareTag("Player"))
                {
                    ChangeLoopBehaviour(BehaviourCycle.requestingPath);
                }

                MoveTowardsPoint(target.position);

                yield return null;

                if (Vector2.Distance(transform.position, target.position) < shootRange)
                {
                    ShootAction?.Invoke();
                }
            }
        }

        private void MoveTowardsPoint(Vector3 point)
        {
            Vector2 direction = point - transform.position;
            float signedAngle = Vector2.SignedAngle(transform.up, direction);
            float f = 0;
            bool playerInAttackRange = hasCannon && Vector2.Distance(transform.position, target.position) < shootRange;

            if (signedAngle > 170 || signedAngle < -170 && playerInAttackRange == false)
            {
                MovementAction?.Invoke(-1f);
            }
            if ((!(signedAngle > 170 || signedAngle < -170) && Vector2.Distance(transform.position, point) <= minimumMoveDistance) ||
                playerInAttackRange)
            {
                MovementAction?.Invoke(f);
            }

            if (signedAngle > 0 && signedAngle < 171)
            {
                RotateAction?.Invoke(-1f);
                return;
            }
            if (signedAngle > -171 && signedAngle < 0)
            {
                RotateAction?.Invoke(1f);
                return;
            }

            RotateAction?.Invoke(f);
        }

        private void OnDrawGizmosSelected()
        {
            if (path != null)
            {
                for (int i = targetIterator; i < path.Length; i++)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(path[i], new Vector3(0.2f, 0.2f, 0.2f));

                    if (i == targetIterator)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                    else
                        Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }

    }
}

