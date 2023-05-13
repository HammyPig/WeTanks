using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseReticle : MonoBehaviour {
    private GameObject sourceObject;
    private Vector3 mousePosition;
    
    void Start() {
        sourceObject = GameObject.FindGameObjectWithTag("Player");
        Cursor.visible = false;
    }

    void Update() {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        transform.position = mousePosition;
    }
}
