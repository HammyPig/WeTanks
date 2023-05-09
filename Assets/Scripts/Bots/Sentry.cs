using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : Bot
{
    protected override void seek() {
        tank.rotateTurretTowards(tank.turret.transform.eulerAngles.z - 90);
    }

    protected override void attack() {
        tank.rotateTurretTowards(target.transform.position - tank.turret.transform.position);

        if (shootCount >= shootInterval) {
            tank.shoot();
            shootCount = 0;
        }

        shootCount += Time.deltaTime;
    }
}
