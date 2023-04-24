using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public int bounceLimit;
    private int bounceCount;

    private AudioSource bulletBounce;
    public GameObject explosionPrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        bounceCount = 0;

        bulletBounce = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = rigidBody.velocity;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Wall")) {
            bulletBounce.Play();

            if (bounceCount >= bounceLimit) {
                Destroy(gameObject);
            }

            bounceCount += 1;
        }

        if (collision.gameObject.CompareTag("Bullet")) {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
