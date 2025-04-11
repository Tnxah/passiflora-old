using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : Obstacle, IActivatable
{

    private bool active;

    private Transform target;
    private float distance;

    public float speed;
    public void Activate()
    {
        active = true;
    }

    private void Awake()
    {
        CheckDistance();
    }

    void FixedUpdate()
    {
        if (!active)
            return;


        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        Vector2 direction = (target.position - transform.position).normalized;
        GetComponent<Rigidbody2D>().linearVelocity = direction * speed * Time.fixedDeltaTime;

        CheckDistance();
    }

    private void CheckDistance()
    {
        var targets = GameObject.FindGameObjectsWithTag("Player");

        target = targets[0]?.transform;
        if (target)
            distance = Vector3.Distance(targets[0].transform.position, transform.position);
        else
            return;

        foreach (var targ in targets)
        {
            var currentDistance = Vector3.Distance(targ.transform.position, transform.position);
            if (currentDistance < distance)
            {
                target = targ.transform;
                distance = currentDistance;
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(collision.gameObject);

            if(active)
            Destroy(gameObject);
        }
    }
}
