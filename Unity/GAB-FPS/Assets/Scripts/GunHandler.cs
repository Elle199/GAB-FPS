using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GunHandler : MonoBehaviour
{
    [SerializeField]
    private int weaponDamage;
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
    private double rateOfFire = 0;
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
        if (Input.GetButton("Fire1") && rateOfFire <= 0 && isReloading == false)
        {
            rateOfFire = 0.1;
            Shoot();
        }

        if (Input.GetButtonUp("Fire1"))
            shootAnim.SetBool("isFiring", false);

        if (rateOfFire > 0)
            rateOfFire = rateOfFire - Time.deltaTime;

        if (Input.GetButtonDown("Fire2"))
        {
            Aim();
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            Aim();
        }

        if (Input.GetKeyDown(KeyCode.R) && isReloading == false)
        {
            StartCoroutine(Reload());
        }


    }

    void Shoot()
    {
        shootAnim.SetBool("isFiring", true);
        muzzelFlash.Play();

        ammoDisplay.GetComponent<Text>().text = curretMagCount.ToString() + " ";

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, weaponRange))
        {
            Enemy target = hit.transform.GetComponent<Enemy>();
            if (target != null)
            {
                target.TakeDamage(weaponDamage);
            }

            GameObject impactEffectGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactEffectGO, 0.15f);
        }

        if (curretMagCount > 0)
            curretMagCount -= 1;
        else if (curretMagCount <= 0)
            StartCoroutine(Reload());
    }

    void Aim()
    {
        aimState = !aimState;
        anim.SetBool("AimIn", aimState);
        crossHair.SetActive(!aimState);
    }

    IEnumerator Reload()
    {
        shootAnim.SetBool("isFiring", false);
        isReloading = true;
        anim.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime - 0.25f);
        curretMagCount = magSize;
        ammoDisplay.GetComponent<Text>().text = curretMagCount.ToString() + " ";
        anim.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);
        isReloading = false;
    }
}
