using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent nav;
    private int health = 100;
    private float hitTimer = 0f;
    private bool canDamaged = true;
    private int hitDamage = 30;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        nav.SetDestination(player.transform.position);

        if(canDamaged == false)
        {
            if (hitTimer > 0)
                hitTimer -= Time.deltaTime;
            if(hitTimer <= 0)
            {
                canDamaged = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            if(hitTimer <= 0)
            {
                hitTimer = 5f;
                canDamaged = false;
                PlayerController target = collision.transform.GetComponent<PlayerController>();
                target.TakeDamage(hitDamage);
                nav.isStopped = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        nav.isStopped = false;
    }

    public void TakeDamage(int damage)
    {
            health -= damage;
            if (health <= 0)
                Destroy(gameObject);
    }
}
