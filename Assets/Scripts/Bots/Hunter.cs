using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hunter : Bot
{
    public float obstacleDetectionRadius = 2;
    private Vector2 movementDirection;
    private NavMeshAgent agent;

    protected override void Start() {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
    }

    protected override void seek() {
        agent.enabled = false;
        movementDirection = getClearPathDirection(movementDirection, obstacleDetectionRadius);
        tank.moveTowards(movementDirection);
    }

    protected override void attack() {
        agent.enabled = true;
        agent.SetDestination(new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z));
        movementDirection = (agent.steeringTarget - transform.position).normalized;
        tank.moveTowards(movementDirection);

        tank.rotateTurretTowards(target.transform.position - tank.turret.transform.position);

        if (shootCount >= shootInterval) {
            tank.shoot();
            shootCount = 0;
        }

        shootCount += Time.deltaTime;
    }
}
