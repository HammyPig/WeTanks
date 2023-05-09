using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Controller
{
    protected GameObject target;
    public int shootInterval;
    protected float shootCount = 0;
    private LayerMask wallLayer;

    protected override void Start() {
        base.Start();
        wallLayer = LayerMask.GetMask("Wall");
        tank.maxTurretRotationSpeed = 50;
    }

    void Update() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        List<GameObject> playersInLineOfSight = new List<GameObject>();

        foreach (GameObject player in players) {
            if (canSee(player)) {
                playersInLineOfSight.Add(player);
            }
        }

        if (playersInLineOfSight.Count == 0) {
            target = null;
        } else {
            float closestDistance = Mathf.Infinity;
            GameObject closestPlayer = null;

            foreach (GameObject player in playersInLineOfSight) {
                float distance = Vector2.Distance(transform.position, player.transform.position);

                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestPlayer = player;
                }
            }

            target = closestPlayer;
        }

        if (target == null) {
            seek();
        } else {
            attack();
        }
    }

    protected bool canSee(GameObject player) {
        Vector2 raycastDirection = player.transform.position - transform.position;
        float raycastLength = Vector2.Distance(transform.position, player.transform.position);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, raycastLength, wallLayer);
        
        if (hit.collider != null) {
            return false;
        } else {
            return true;
        }
    }

    protected virtual void attack() {
        tank.rotateTurretTowards(target.transform.position - tank.turret.transform.position);

        if (shootCount >= shootInterval) {
            tank.shoot();
            shootCount = 0;
        }

        shootCount += Time.deltaTime;
    }

    protected virtual void seek() {
        tank.rotateTurretTowards(tank.turret.transform.eulerAngles.z - 90);
    }
}
