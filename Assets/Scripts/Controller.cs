using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    protected Tank tank;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        tank = gameObject.GetComponent<Tank>();
    }
}
