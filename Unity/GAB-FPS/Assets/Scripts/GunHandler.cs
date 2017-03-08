using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody bullet;
    [SerializeField] private Transform gunEnd;
    [SerializeField] private float thrust;
    private double rateOfFire = 0;

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1") && rateOfFire <= 0)
        {
            Rigidbody bulletCopy;
            bulletCopy = Instantiate(bullet, gunEnd.position, gunEnd.rotation) as Rigidbody;
            bulletCopy.AddForce(gunEnd.forward * thrust);
            rateOfFire = 0.2;
            //bulletCopy.transform.parent = gameObject.transform;
        }
        if (rateOfFire > 0)
            rateOfFire = rateOfFire - Time.deltaTime;
	}
  /*public Rigidbody rocketPrefab;
    public Transform barrelEnd;


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody rocketInstance;
            rocketInstance = Instantiate(rocketPrefab, barrelEnd.position, barrelEnd.rotation) as Rigidbody;
            rocketInstance.AddForce(barrelEnd.forward * 2);
        }
    }*/
}
