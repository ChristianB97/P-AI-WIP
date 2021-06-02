using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Wache_Verhalten : MonoBehaviour
{

    public Transform target;
    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    public Animator anim;

    private Transform enemyGFX;
    public Transform licht;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        enemyGFX = GetComponent<Transform>();
        licht = GetComponent<Transform>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);

    }

    void UpdatePath() {
        if (seeker.IsDone()) {
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

        anim.SetFloat("speedX", rb.velocity.x);
        anim.SetFloat("speedY", rb.velocity.y);



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

        rb.AddForce(force);

        float disctance = Vector2.Distance(rb.position, path.vectorPath[currentWaypint]);

        if(disctance < nextWaypointDistance) {
            currentWaypint++;
        }

        if(force.x >= 0.03f) {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        } else if(force.x <= -0.03f) {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        }

    }
}
