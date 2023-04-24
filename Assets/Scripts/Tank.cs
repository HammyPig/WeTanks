using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float movementSpeed;
    public float rotationSpeed;
    public GameObject turret;
    public GameObject bulletSpawner;
    public int maxAmmo;
    private int ammo;
    public float reloadInterval;
    private float reloadCount;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float spriteAngle = 0f;
    public GameObject trackPrefab;
    public GameObject trackSpawner;
    public float trackInterval;
    private float trackCount;

    public AudioSource[] sounds;
    public AudioSource fireSound;
    public AudioSource moveSound;

    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
        sounds = GetComponents<AudioSource>();
        fireSound = sounds[0];
        moveSound = sounds[1];
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rigidBody.velocity = movementSpeed * direction;

        if (direction.magnitude > 0.1f) { updateSpriteAngle(direction); }
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDirection = (mousePosition - transform.position).normalized;

        updateTurretAngle(mouseDirection);

        if (Input.GetMouseButtonDown(0)) {
            if (ammo > 0) {
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawner.transform.position, Quaternion.identity);
                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
                bulletRb.velocity = bulletSpeed * mouseDirection;

                if (fireSound == null) {
                    Debug.Log("No audio");
                }
                fireSound.Play();

                ammo -= 1;
            }
        }

        if (ammo < maxAmmo) {
            if (reloadCount >= reloadInterval) {
                ammo += 1;
                reloadCount = 0;
            }

            reloadCount += Time.deltaTime;
        }

        if (trackCount >= trackInterval) {
            Instantiate(trackPrefab, trackSpawner.transform.position, trackSpawner.transform.rotation);
            trackCount = 0;
        }

        trackCount += Time.deltaTime;
    }

    private void updateSpriteAngle(Vector3 direction) {
        float newSpriteAngle1 = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float newSpriteAngle2 = oppositeAngle(newSpriteAngle1);

        float relativeAngle;
        if ((Mathf.Abs(spriteAngle - newSpriteAngle1) % 280) < (Mathf.Abs(spriteAngle - newSpriteAngle2) % 280)) {
            relativeAngle = newSpriteAngle1 - spriteAngle;
        } else {
            relativeAngle = newSpriteAngle2 - spriteAngle;
        }

        spriteAngle = ((spriteAngle + relativeAngle + 180) % 360) - 180;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, spriteAngle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private float oppositeAngle(float angle) {
        if (angle > 0) {
            return angle - 180;
        } else {
            return angle + 180;
        }
    }

    private void updateTurretAngle(Vector3 mouseDirection) {
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;
        turret.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
