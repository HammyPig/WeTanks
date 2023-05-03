using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public Controller controller;
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

    public AudioSource[] sounds;
    public AudioSource fireSound;
    public AudioSource moveSound;

    public int maxHealth;
    private int health;
    public bool isDead;
    public GameObject explosionPrefab;

    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
        health = maxHealth;
        isDead = false;
        sounds = GetComponents<AudioSource>();
        fireSound = sounds[0];
        moveSound = sounds[1];
    }

    // Update is called once per frame
    void Update()
    {   
        if (isDead) {
            stop();
            return;
        }

        if (ammo < maxAmmo) {
            reload();
        }
    }

    public void throttle(float value) {
        rigidBody.velocity = (value * movementSpeed) * transform.right;
        if (value != 0) this.spawnTracks();
    }

    public void turnTowards(float targetRotation) {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0f, 0f, targetRotation), rotationSpeed * Time.deltaTime);
    }

    public void stop() {
        rigidBody.velocity = 0 * transform.right;
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
            
            fireSound.Play();

            float angle = bulletSpawner.transform.rotation.eulerAngles.z;
            angle -= 90;
            GameObject explosion = Instantiate(explosionPrefab, bulletSpawner.transform.position, Quaternion.Euler(0f, 0f, angle));
        }
    }

    public void reload() {
        if (reloadCount >= reloadInterval) {
            ammo += 1;
            reloadCount = 0;
        }

        reloadCount += Time.deltaTime;
    }

    public void loseHealth(int points) {
        health -= points;
        if (health <= 0) {
            isDead = true;
        }
    }

    private void spawnTracks() {
        if (trackCount >= trackInterval) {
            Instantiate(trackPrefab, trackSpawner.transform.position, trackSpawner.transform.rotation);
            trackCount = 0;
        }

        trackCount += Time.deltaTime;
    }
}
