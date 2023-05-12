using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Scout : Bot
{
    public float wallDetectionRadius = 2;
    private Vector2 movementDirection;

    protected override void seek() {
        chooseDirection();
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

    private void chooseDirection() {
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
            if (!canSeeObstacle(direction, wallDetectionRadius)) {
                validDirections.Add(direction);
            }
        }

        if (validDirections.Count == 0) {
            validDirections = directions;
        }

        float[] probabilities = new float[validDirections.Count];
        for (int i = 0; i < validDirections.Count; i++) {
            float angleDifference = Vector2.Angle(movementDirection, validDirections[i]);
            float probability = angleDifference / 180;
            probabilities[i] = Mathf.Exp(-27 * probability);
        }

        int selectedIndex = weightedProbability(probabilities);
        movementDirection = validDirections[selectedIndex];
    }

    private int weightedProbability(float[] probabilities) {
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

    private bool canSeeObstacle(Vector2 direction, float distance) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleLayer);
        
        if (hit.collider == null) {
            return false;
        } else {
            return true;
        }
    }
}
