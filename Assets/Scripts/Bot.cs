using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Controller
{
    public GameObject target;
    public int shootInterval;
    protected float shootCount = 0;
    private LayerMask wallLayer;

    protected override void Start() {
        base.Start();
        wallLayer = LayerMask.GetMask("Wall");
        tank.maxTurretRotationSpeed = 50;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSeeTarget()) {
            attack();
        } else {
            seek();
        }
    }

    protected bool canSeeTarget() {
        Vector2 raycastDirection = target.transform.position - transform.position;
        float raycastLength = Vector2.Distance(transform.position, target.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, raycastLength, wallLayer);
        
        if (hit.collider != null) {
            return false;
        } else {
            return true;
        }
    }

    private void attack() {
        tank.rotateTurretTowards(target.transform.position - tank.turret.transform.position);

        if (shootCount >= shootInterval) {
            tank.shoot();
            shootCount = 0;
        }

        shootCount += Time.deltaTime;
    }

    private void seek() {
        tank.rotateTurretTowards(tank.turret.transform.eulerAngles.z - 90);
    }
}
