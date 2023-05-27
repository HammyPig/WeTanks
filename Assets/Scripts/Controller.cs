using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public abstract class Controller : MonoBehaviour
{
    protected   PhotonView view;
    protected Tank tank;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        tank = gameObject.GetComponent<Tank>();
        view = GetComponent<PhotonView>();
    }
}
