using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float movementSpeed;
    public float rotationSpeed;

    public GameObject turret;
    public float turretRotationSpeed;

    public GameObject bulletSpawner;
    public int maxAmmo;
    private int ammo;
    public float reloadInterval;
    private float reloadCount;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    
    public GameObject trackPrefab;
    public GameObject trackSpawner;
    public float trackInterval;
    private float trackCount;

    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {   
        if (ammo < maxAmmo) {
            reload();
        }
    }

    public void move(Vector2 direction) {
        float currentAngle = transform.rotation.eulerAngles.z;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.DeltaAngle(currentAngle, targetAngle);

        if (deltaAngle > 90) {
            rigidBody.velocity = -movementSpeed * transform.right;
        } else {
            rigidBody.velocity = movementSpeed * transform.right;
        }

        spawnTracks();
    }

    public void stop() {
        rigidBody.velocity = 0 * transform.right;
    }

    public void rotateTowards(Vector2 direction) {
        float currentAngle = transform.rotation.eulerAngles.z;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.DeltaAngle(currentAngle, targetAngle);

        if (Mathf.Abs(deltaAngle) > 90) {
            deltaAngle -= 180;
        }

        currentAngle = ((currentAngle + deltaAngle + 180) % 360) - 180;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, currentAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void rotateTurretTowards(Vector2 direction) {
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, targetAngle);
        turret.transform.rotation = Quaternion.RotateTowards(turret.transform.rotation, targetRotation, turretRotationSpeed * Time.deltaTime);
    }

    public void shoot() {
        if (ammo > 0) {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawner.transform.position, bulletSpawner.transform.rotation);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = bulletSpeed * bullet.transform.right;

            ammo -= 1;
        }
    }

    public void reload() {
        if (reloadCount >= reloadInterval) {
            ammo += 1;
            reloadCount = 0;
        }

        reloadCount += Time.deltaTime;
    }

    private void spawnTracks() {
        if (trackCount >= trackInterval) {
            Instantiate(trackPrefab, trackSpawner.transform.position, trackSpawner.transform.rotation);
            trackCount = 0;
        }

        trackCount += Time.deltaTime;
    }
}
