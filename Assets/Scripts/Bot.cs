using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Controller
{
    public GameObject target;
    public LayerMask wallLayer;

    void Start() {
        base.Start();
        wallLayer = LayerMask.GetMask("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 raycastDirection = target.transform.position - transform.position;
        float raycastLength = Vector2.Distance(transform.position, target.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, raycastLength, wallLayer);
        if (hit.collider != null) {
            Debug.Log("cannot see tank");
        } else {
            Debug.Log("can see tank");
        }
    }
}
