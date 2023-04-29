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
        this.controller = new PlayerController(this);
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

        this.controller.think();
        if (ammo < maxAmmo) {
            reload();
        }
    }

    public void throttle(int value) {
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

public interface Controller {
    public void think();
}

public class PlayerController : Controller {
    private Tank tank;

    public PlayerController(Tank tank) {
        this.tank = tank;
    }

    public void think() {
        float currentAngle = tank.transform.rotation.eulerAngles.z;
        Vector2 movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.DeltaAngle(currentAngle, targetAngle);

        int throttle = (int) movementDirection.magnitude * (Mathf.Abs(deltaAngle) > 90 ? -1 : 1);

        tank.throttle(throttle);

        if (throttle > 0) {
            tank.turnTowards(targetAngle);
        } else if (throttle < 0) {
            tank.turnTowards(targetAngle + 180);
        }
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDirection = (mousePosition - tank.transform.position).normalized;
        tank.rotateTurretTowards(mouseDirection);

        if (Input.GetMouseButtonDown(0)) {
            tank.shoot();
        }
    }
}

public class HardPlayerController : Controller {
    private Tank tank;

    public HardPlayerController(Tank tank) {
        this.tank = tank;
    }

    public void think() {
        int throttle = (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);
        tank.throttle(throttle);

        float angle = tank.transform.rotation.eulerAngles.z;
        float direction = (Input.GetKey(KeyCode.A) ? 90 : 0) + (Input.GetKey(KeyCode.D) ? -90 : 0);
        if (throttle >= 0) {
            angle += direction;
        } else {
            angle -= direction;
        }
        
        tank.turnTowards(angle);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDirection = (mousePosition - tank.transform.position).normalized;
        tank.rotateTurretTowards(mouseDirection);

        if (Input.GetMouseButtonDown(0)) {
            tank.shoot();
        }
    }
}

public interface AIController : Controller {
    public void seek();
    public void attack();
}