using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private Rigidbody bulletRB;

    void OnCollisionEnter()
    {
        Debug.Log("hit");
    }
}
