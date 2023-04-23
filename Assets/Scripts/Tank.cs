using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public float movementSpeed;
    public float rotationSpeed;
    public GameObject bulletPrefab;
    public float bulletSpeed;
    private float spriteAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        rigidBody.velocity = movementSpeed * direction;

        if (direction.magnitude > 0.1f) { updateSpriteAngle(direction); }

        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mouseDirection = (mousePosition - transform.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

            bulletRb.velocity = bulletSpeed * mouseDirection;
        }
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
}
