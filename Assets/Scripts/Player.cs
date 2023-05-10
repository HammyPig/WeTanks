using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Controller
{
    // Update is called once per frame
    void Update()
    {
        Vector2 movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        tank.moveTowards(movementDirection);
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDirection = (mousePosition - tank.transform.position).normalized;
        tank.rotateTurretTowards(mouseDirection);

        if (Input.GetMouseButtonDown(0)) {
            tank.shoot();
        }
    }
}
