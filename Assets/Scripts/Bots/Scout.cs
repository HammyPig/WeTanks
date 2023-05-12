using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scout : Bot
{
    public float obstacleDetectionRadius = 2;
    private Vector2 movementDirection;

    protected override void seek() {
        movementDirection = getClearPathDirection(movementDirection, obstacleDetectionRadius);
        tank.moveTowards(movementDirection);
    }

    protected override void attack() {
        tank.stop();
        
        tank.rotateTurretTowards(target.transform.position - tank.turret.transform.position);

        if (shootCount >= shootInterval) {
            tank.shoot();
            shootCount = 0;
        }

        shootCount += Time.deltaTime;
    }
}
