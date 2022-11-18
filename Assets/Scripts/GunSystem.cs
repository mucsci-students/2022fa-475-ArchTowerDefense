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

    //  Graphics

    public CameraShake camShake;
    public float camShakeMagnitude,
        camShakeDuration;
    public GameObject muzzleFlash;
    public TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

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

        // Shake Camera
        camShake.Shake(camShakeDuration, camShakeMagnitude);

        // Graphics
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

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
}
