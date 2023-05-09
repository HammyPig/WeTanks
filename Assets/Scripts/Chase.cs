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

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z));
        Vector2 movementDirection = (transform.position - agent.steeringTarget).normalized;
        tank.moveTowards(movementDirection);
        
        if (canSeeTarget()) {
            tank.rotateTurretTowards(target.transform.position - tank.turret.transform.position);

            if (shootCount >= shootInterval) {
                tank.shoot();
                shootCount = 0;
            }

            shootCount += Time.deltaTime;
        }
    }
}
