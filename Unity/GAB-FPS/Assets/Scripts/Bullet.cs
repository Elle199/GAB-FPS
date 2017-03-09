using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float lifeSpand = 10;

    private void Update()
    {
        lifeSpand = lifeSpand - Time.deltaTime;
        if (lifeSpand <= 0)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collided" + collision);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bullet Hit");
    }
}
