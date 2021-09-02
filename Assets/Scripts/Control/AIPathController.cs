using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gameplay;
using Pathfinding;

public class AIPathController : MonoBehaviour
{
    Transform target;
    Vector3[] path;
    int targetIterator;
    Rigidbody2D shipRigidbody;
    public float distanceMargin = 0.5f;

    private void Awake()
    {
        shipRigidbody = GetComponent<Rigidbody2D>();
    }

    IEnumerator Start()
    {
        yield return null;
        target = GameplayManager.GetInstance().GetPlayerTransform();
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);   
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessfull)
    {
        if (pathSuccessfull)
        {
            path = newPath;
            StopAllCoroutines();
            StartCoroutine(FollowPath());
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];

        while (true)
        {
            if (Vector2.Distance(transform.position, path[path.Length - 1]) < distanceMargin)
                break;
            if (Vector2.Distance(transform.position, currentWaypoint) < distanceMargin)
            {
                targetIterator++;
                if (targetIterator >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIterator];
            }
            Debug.DrawRay(transform.position, target.position - transform.position, Color.green);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position);
            Debug.Log(hit.collider.tag);
            if (hit.collider.CompareTag("Player"))
            {
                //StartCoroutine(MoveTowardsPlayer());
                break;
            }

            MoveTowardsWaypoint(currentWaypoint);

            yield return null;
        }

        StartCoroutine(MoveTowardsPlayer());
    }

    private IEnumerator MoveTowardsPlayer()
    {
        while (true)
        {
            StopCoroutine(FollowPath());
            RaycastHit2D hit = Physics2D.Raycast(transform.position, target.position - transform.position);
            if (!hit.collider.CompareTag("Player"))
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                StopCoroutine(MoveTowardsPlayer());
            }
            if (Vector2.Distance(transform.position, target.position) <= 3)
            {
                //atack
                yield return null;
            }
        
            Vector2 direction = target.position - transform.position;

            if (Vector2.SignedAngle(transform.up, direction) < 180 && Vector2.SignedAngle(transform.up, direction) > 170)
            {
                Move();
            }
            else if (Vector2.SignedAngle(transform.up, direction) > 0)
            {
                Rotate(-1);
            }
            else if (Vector2.SignedAngle(transform.up, direction) < 0)
            {
                Rotate(1);
            }
            yield return null;
        }
    }

    private void MoveTowardsWaypoint(Vector3 currentWaypoint)
    {
        Vector2 direction = currentWaypoint - transform.position;

        if (Vector2.SignedAngle(transform.up, direction) < 180 && Vector2.SignedAngle(transform.up, direction) > 170)
        {
            Move();
        }
        else if (Vector2.SignedAngle(transform.up, direction) > 0)
        {
            Rotate(-1);
        }
        else if (Vector2.SignedAngle(transform.up, direction) < 0)
        {
            Rotate(1);
        }
    }

    private void Update()
    {

    }

    public void Move()
    {
        shipRigidbody.AddForce(-1 * (transform.up * 330 * Time.deltaTime));
    }

    public void Rotate(float axisValue)
    {
        Quaternion pretendedRotation = GetPretendedRotation(axisValue);

        shipRigidbody.SetRotation(pretendedRotation);
    }

    private Quaternion GetPretendedRotation(float directionMultiplier)
    {
        Quaternion currentRotation = transform.rotation;
        float rotateAmount = directionMultiplier * (100 * Time.deltaTime);
        Quaternion pretendedRotation = currentRotation;
        pretendedRotation.eulerAngles = currentRotation.eulerAngles += new Vector3(0, 0, rotateAmount);
        return pretendedRotation;
    }


    private void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIterator; i< path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], new Vector3(0.2f,0.2f,0.2f));

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