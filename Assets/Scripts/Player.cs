using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject tankGameObject;
    private Tank tank;

    // Start is called before the first frame update
    void Start()
    {
        tank = tankGameObject.GetComponent<Tank>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if (movementDirection.magnitude > 0) {
            tank.move(movementDirection);
            tank.rotateTowards(movementDirection);
        } else {
            tank.stop();
        }
        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDirection = (mousePosition - tankGameObject.transform.position).normalized;
        tank.rotateTurretTowards(mouseDirection);

        if (Input.GetMouseButtonDown(0)) {
            tank.shoot();
        }
    }
}
