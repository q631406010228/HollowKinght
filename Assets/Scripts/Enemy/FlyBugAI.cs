using Pathfinding;
using UnityEngine;

public class FlyBugAI : MonoBehaviour
{
    public Transform Traget;

    public float Speed = 200f;
    public float NextWaypointDistance = 3f;

    Path FlyButPath;
    Seeker FlyBugSeeker;
    Rigidbody2D rb;
    int CurrentWaypoint = 0;
    bool ReachedEndOfPath = false;

    // Start is called before the first frame update
    void Start()
    {
        FlyBugSeeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if(FlyBugSeeker.IsDone())
            FlyBugSeeker.StartPath(transform.position, Traget.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            FlyButPath = p;
            CurrentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FlyButPath == null)
            return;
        if (CurrentWaypoint >= FlyButPath.vectorPath.Count)
        {
            ReachedEndOfPath = true;
            return;
        }
        else
        {
            ReachedEndOfPath = false;
        }
        Vector2 direction = (FlyButPath.vectorPath[CurrentWaypoint] - transform.position).normalized;
        Vector2 force = direction * Speed * Time.deltaTime;
        rb.AddForce(force);
        float distance = Vector2.Distance(transform.position, FlyButPath.vectorPath[CurrentWaypoint]);
        if(distance < NextWaypointDistance)
        {
            CurrentWaypoint++;
        }
        if(force.x >= 0.01f)
        {
            transform.localScale = new Vector3(-0.6f, 0.6f, 0.6f);
        }
        else if(force.x <= 0.01f)
        {
            transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        }
    }
}
