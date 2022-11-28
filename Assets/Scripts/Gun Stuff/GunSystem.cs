using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GunSystem : MonoBehaviour
{
    // Gun stats
    public int damage;
    public float timeBetweenShooting,
        spread,
        range,
        reloadTime,
        timeBetweenShots;
    public int magazineSize,
        bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft,
        bulletsShot;

    bool shooting,
        readyToShoot,
        reloading;

    //Reference
    public Camera cam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public GameObject Gun;

    //  Graphics
    public GameObject muzzleFlash;
    public GameObject impactEffect;

    public TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        if(bulletsLeft < (.25 * magazineSize))
        {
            text.color = new Color(.698f, .133f, .133f, 1.0f);
        }
        else
        {
            text.color = new Color(1f, 1f, 1f, 1f);
        }
        // update bullet num
        text.SetText(bulletsLeft + " / " + magazineSize);

    }

    private void MyInput()
    {
        // Are they able to shoot by holding down or only tapping?
        if (allowButtonHold)
        {
            shooting = Input.GetKey(KeyCode.Mouse0);
        }
        else
        {
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            Reload();
        }

        // Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calc direction with spread
        Vector3 direction = cam.transform.forward + new Vector3(x, y, 0);

        // Ray Cast
        if (Physics.Raycast(cam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
                // Make sure enemies have the tag "Enemy" and have the TakeDamage function
                rayHit.collider.GetComponent<Enemy>().TakeDamage(damage);
        }


        // Graphics
        GameObject muzzFlash = (GameObject)Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        muzzFlash.transform.parent = Gun.transform;
        
        GameObject impactGO = Instantiate(impactEffect, rayHit.point, Quaternion.LookRotation(rayHit.normal));
        Destroy(impactGO, 2f);

        // Recoil
        StartCoroutine(StartRecoil());

        bulletsLeft--;
        bulletsShot--;

        // Calling the function with some delay - the time between shooting
        Invoke("ResetShot", timeBetweenShooting);

        // How many times to shoot for each tap?
        if (bulletsShot > 0 && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }

    IEnumerator StartRecoil()
    {
        Debug.Log(Gun.name + " recoil");
        if(Gun.name == "Pistol")
        {
            Gun.GetComponent<Animator>().Play("Pistol.Shoot");
            yield return new WaitForSeconds(0.10f);
            Gun.GetComponent<Animator>().Play("Pistol.New State");
        }
        else if(Gun.name == "AssaultRifle")
        {
            Gun.GetComponent<Animator>().Play("AssaultRifle.Shoot");
            yield return new WaitForSeconds(0.50f);
            Gun.GetComponent<Animator>().Play("AssaultRifle.New State");
        }
        else if(Gun.name == "SMG")
        {
            Gun.GetComponent<Animator>().Play("SMG.Shoot");
            yield return new WaitForSeconds(0.10f);
            Gun.GetComponent<Animator>().Play("SMG.New State");
        }
        else if(Gun.name == "procrastinator")
        {
            // Gun.GetComponent<Animator>().Play("Recoil");
            Gun.GetComponent<Animator>().Play("Procrastinator.Shoot");
            // yield return new WaitForSeconds(0.20f);
            yield return new WaitForSeconds(1.10f);
            Gun.GetComponent<Animator>().Play("Procrastinator.New State");
        }
        else if(Gun.name == "AK47")
        {
            Gun.GetComponent<Animator>().Play("AK47.Shoot");
            yield return new WaitForSeconds(0.10f);
            Gun.GetComponent<Animator>().Play("AK47.New State");
        }
    }
}
