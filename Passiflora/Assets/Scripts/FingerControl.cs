using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerControl : MonoBehaviour
{

    public static FingerControl instance;

    Vector3 touchPosition;

    public GameObject first;
    public GameObject second;

    public float fingerOffset;

    private void Start()
    {
        if (!instance)
        {
            instance = this;
        }

        fingerOffset = Settings.fingerOffset;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (Touch touch in Input.touches)
        {
            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            RaycastHit2D hit = Physics2D.Raycast(touchPosition, Camera.main.transform.forward, 100);
            Debug.DrawRay(touchPosition, Camera.main.transform.forward * 50, Color.green, 5);

            if (Settings.isAllowedFingerOffset)
            touchPosition += new Vector3(0, fingerOffset, 0);

            if (touch.fingerId == 0)
            {
                if (hit && first == null && hit.collider.CompareTag("Player"))
                {
                    first = hit.collider.gameObject;
                }
                if (first)
                {
                    Vector2 direction = (touchPosition - first.transform.position).normalized;
                    first.GetComponent<Rigidbody2D>().linearVelocity = direction * 15000f * Time.fixedDeltaTime;
                }
            }
            if (touch.fingerId == 1)
            {
                if (hit && second == null && hit.collider.CompareTag("Player"))
                {
                    second = hit.collider.gameObject;
                }
                if (second)
                {
                    Vector2 direction = (touchPosition - second.transform.position).normalized;
                    second.GetComponent<Rigidbody2D>().linearVelocity = direction * 15000f * Time.fixedDeltaTime;
                }
            }
        }
    }

    private void Update()
    {
        foreach (Touch touch in Input.touches)

            if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
        {
            if (touch.fingerId == 0 && first)
            {
                first.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                first = null;
            }
            if (touch.fingerId == 1 && second)
            {
                second.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                second = null;
            }
        }
    }

    public void StopLights()
    {
        var lights = GameObject.FindGameObjectsWithTag("Player");

            lights[0].GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            lights[1].GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
 
    }

    public bool BouthTouched()
    {
        if (first && second)
        {
            return true;
        }
        else
        {
            return false;
        }
    } 

}