using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GunHandler : MonoBehaviour
{
    [SerializeField]
    private int weaponDamage;
    [SerializeField]
    private bool infiniteAmmo = false;
    [SerializeField]
    private GameObject ammoDisplay;
    [SerializeField]
    private Transform gunEnd;
    [SerializeField]
    private ParticleSystem muzzelFlash;
    [SerializeField]
    private GameObject impactEffect;
    [SerializeField]
    private GameObject shootBoober;


    private float weaponRange = 50f;
    private float reloadTime = 1.4f;
    private float magSize = 35f;
    private float curretMagCount;
    private float waitBetweenShot = 0;
    private bool aimState = false;
    private bool isReloading = false;
    private Camera fpsCam;
    private Animator anim;
    private Animator shootAnim;
    private GameObject crossHair;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        crossHair = GameObject.FindGameObjectWithTag("CrossHair");
        shootAnim = shootBoober.GetComponent<Animator>();
        fpsCam = Camera.main;

        curretMagCount = magSize;

        ammoDisplay.GetComponent<Text>().text = curretMagCount.ToString() + " ";

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && waitBetweenShot <= 0 && isReloading == false)
        {
            waitBetweenShot = 0.1f;
            if (curretMagCount > 0)
            {
                Shoot();
            }
            else if (curretMagCount <= 0)
            {
                StartCoroutine(Reload());
            }

        }

        //Manual reload call
        if (Input.GetKeyDown(KeyCode.R) && isReloading == false)
        {
            StartCoroutine(Reload());
        }

        if (Input.GetButtonUp("Fire1"))
            shootAnim.SetBool("isFiring", false);

        if (waitBetweenShot > 0)
            waitBetweenShot = waitBetweenShot - Time.deltaTime;
        if (waitBetweenShot < 0)
            waitBetweenShot = 0;

        //Handles aiming
        if (Input.GetButtonDown("Fire2"))
        {
            Aim();
            //fpsCam.fieldOfView = 40;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            Aim();
            //fpsCam.fieldOfView = 60;
        }
    }

    void Shoot()
    {
        shootAnim.speed = (1 / waitBetweenShot);
        shootAnim.SetBool("isFiring", true);
        muzzelFlash.Play();

        //Removes one bullet from magazine or starts reloading
        if (curretMagCount > 0 && infiniteAmmo == false)
            curretMagCount -= 1;

        //Spawns particles on hit and if hit is a enemy make it take damage
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, weaponRange))
        {
            Enemy target = hit.transform.GetComponent<Enemy>();
            if (target != null)
            {
                target.TakeDamage(weaponDamage);
            }

            GameObject impactEffectGO = Instantiate(impactEffect, hit.point - (0.1f * -hit.normal), Quaternion.LookRotation(hit.normal));
            Destroy(impactEffectGO, 0.15f);
        }

        //Updates displayed ammo
        ammoDisplay.GetComponent<Text>().text = curretMagCount.ToString() + " ";
    }

    void Aim()
    {
        aimState = !aimState;
        anim.SetBool("AimIn", aimState);
        crossHair.SetActive(!aimState);
    }

    IEnumerator Reload()
    {
        //Sets animation states for propper looks
        shootAnim.SetBool("isFiring", false);
        anim.SetBool("Reloading", true);
        if (aimState == true)
            anim.SetBool("AimIn", false);

        isReloading = true; //Tells that gun is reloading

        yield return new WaitForSeconds(reloadTime - 0.25f); //Workaround for animatino timing

        curretMagCount = magSize;
        ammoDisplay.GetComponent<Text>().text = curretMagCount.ToString() + " ";

        anim.SetBool("Reloading", false);
        if (aimState == true)
            anim.SetBool("AimIn", true);

        yield return new WaitForSeconds(0.5f);//Workaround for animatino timing

        

        isReloading = false; //Tells that gun is finished reloading
    }
}
