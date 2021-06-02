using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Wache_Verhalten : MonoBehaviour
{
    public Transform enemyLight;
    private Transform target;
    public CircleCollider2D circleCollider;
    public PolygonCollider2D polygonCollider2D;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    public Animator anim;

    public Transform licht;
    public PlayerRecognizer playerRecognizer;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        licht = GetComponent<Transform>();
        circleCollider = gameObject.GetComponent<CircleCollider2D>();
        circleCollider.enabled = false;

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        playerRecognizer = gameObject.GetComponent<PlayerRecognizer>();
        playerRecognizer.onTargetFound += StartFollow;
        playerRecognizer.onTargetLost += EndFollow;
    }

    private void StartFollow(Transform _target)
    {
        target = _target;
        polygonCollider2D.enabled = false;
        circleCollider.enabled = true;
    }

    private void EndFollow()
    {
        target = null;
        polygonCollider2D.enabled = true;
        circleCollider.enabled = false;
    }

    void UpdatePath() {
        if (target && seeker.IsDone()) {
            seeker.StartPath(rb.position, target.position, onPathComlete);
        }
    }

    void onPathComlete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y))
        {
            anim.SetFloat("speedX", Mathf.Abs(rb.velocity.x));
            anim.SetFloat("speedY", 0);
        }
        else
        {
            anim.SetFloat("speedX", 0);
            anim.SetFloat("speedY", rb.velocity.y);
        }




        if(path == null) {
            return;
        }

        if(currentWaypint >= path.vectorPath.Count) {
            reachedEndOfPath = true;
            return;
        } else {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if (target)
        {
            Debug.Log(Vector3.Distance(target.position, transform.position));
            if (Vector3.Distance(target.position, transform.position) < 2)
            {
                rb.velocity = Vector2.zero;
            }
            else
                rb.AddForce(force);
        }

        float disctance = Vector2.Distance(rb.position, path.vectorPath[currentWaypint]);

        if(disctance < nextWaypointDistance) {
            currentWaypint++;
        }

        if(rb.velocity.x > 0.3f) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        } else if(rb.velocity.x <= -0.3f) {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        //TODO licht nach oben und unten evtl mit direction

    }
}
