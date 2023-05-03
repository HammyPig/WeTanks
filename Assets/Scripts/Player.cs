using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Controller
{
    // Update is called once per frame
    void Update()
    {
        Vector2 movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        float currentAngle = tank.transform.rotation.eulerAngles.z;
        float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.DeltaAngle(currentAngle, targetAngle);

        float throttle = movementDirection.magnitude * (Mathf.Abs(deltaAngle) > 90 ? -1 : 1);
        tank.throttle(throttle);

        if (throttle > 0) {
            tank.turnTowards(targetAngle);
        } else if (throttle < 0) {
            tank.turnTowards(targetAngle + 180);
        }
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDirection = (mousePosition - tank.transform.position).normalized;
        tank.rotateTurretTowards(mouseDirection);

        if (Input.GetMouseButtonDown(0)) {
            tank.shoot();
        }
    }
}
