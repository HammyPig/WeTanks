using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Bot : Controller
{
    protected GameObject target;
    public int shootInterval = 5;
    protected float shootCount = 0;
    protected LayerMask wallLayer;
    protected LayerMask botLayer;
    protected LayerMask obstacleLayer;

    protected override void Start() {
        base.Start();
        wallLayer = LayerMask.GetMask("Wall");
        botLayer = LayerMask.GetMask("Bot");
        obstacleLayer = wallLayer | botLayer;
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, raycastDirection, raycastLength, obstacleLayer);
        
        if (hit.collider != null) {
            return false;
        } else {
            return true;
        }
    }

    protected Vector2 getClearPathDirection(Vector2 currentMovementDirection, float obstacleDetectionRadius) {
        int numDirections = 8;
        float angleInterval = 360 / numDirections;

        List<Vector2> directions = new List<Vector2>();
        for (int i = 0; i < numDirections; i++) {
            float angle = i * angleInterval;
            Vector2 direction = Quaternion.Euler(0f, 0f, angle) * Vector2.right;
            directions.Add(direction);
        }

        List<Vector2> validDirections = new List<Vector2>();
        foreach (Vector2 direction in directions) {
            if (!canSeeObstacle(direction, obstacleDetectionRadius)) {
                validDirections.Add(direction);
            }
        }

        if (validDirections.Count == 0) {
            validDirections = directions;
        }

        float[] probabilities = new float[validDirections.Count];
        for (int i = 0; i < validDirections.Count; i++) {
            float angleDifference = Vector2.Angle(currentMovementDirection, validDirections[i]);
            float probability = angleDifference / 180;
            probabilities[i] = Mathf.Exp(-27 * probability);
        }

        int selectedIndex = weightedProbability(probabilities);
        return validDirections[selectedIndex];
    }

    protected int weightedProbability(float[] probabilities) {
        float randomValue = Random.Range(0, probabilities.Sum());
        float cumulativeProbability = 0f;

        for (int i = 0; i < probabilities.Length; i++) {
            cumulativeProbability += probabilities[i];

            if (randomValue <= cumulativeProbability) {
                return i;
            }
        }

        return -1;
    }

    protected bool canSeeObstacle(Vector2 direction, float distance) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleLayer);
        
        if (hit.collider == null) {
            return false;
        } else {
            return true;
        }
    }

    protected void shootAt(Vector3 position) {
        tank.rotateTurretTowards(position - tank.turret.transform.position);

        if (shootCount >= shootInterval) {
            tank.shoot();
            shootCount = 0;
        }

        shootCount += Time.deltaTime;
    }

    protected abstract void seek();
    protected abstract void attack();
}
