using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : Bot
{
    NavMeshAgent agent;
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
    }

    protected override void seek() {
        ;
    }

    protected override void attack() {
        agent.SetDestination(new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z));
        Vector2 movementDirection = (agent.steeringTarget - transform.position).normalized;
        tank.moveTowards(movementDirection);

        tank.rotateTurretTowards(target.transform.position - tank.turret.transform.position);

        if (shootCount >= shootInterval) {
            tank.shoot();
            shootCount = 0;
        }

        shootCount += Time.deltaTime;
    }
}
