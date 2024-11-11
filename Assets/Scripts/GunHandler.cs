using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    private enum GunID { Magnum = 0, Shotgun = 1, AssaultRifle = 3 };

    //Components
    [SerializeField] private AimController aimScript;
    [SerializeField] private Transform bulletTransform;
    [SerializeField] private ObjectPooler poolScript;

    // Modifiers
    [SerializeField] private int bulletSpd = 1;
    [SerializeField] private int damagePerBullet;
    [SerializeField] private int magazineSize;
    [SerializeField] private int bulletsInMag;
    [SerializeField] private float fireDelayBetweenShots;
    [SerializeField] private float reloadTime;
    [SerializeField] private int shellsPerSalvo;
    [SerializeField] private float shotgunSpreadAngle = 30f;

    public int DamagePerBullet { get { return damagePerBullet; } set { damagePerBullet = value; } }

    // Variables
    private GunID primaryId;
    private bool m_fireDelay;
    private bool m_reloading;


    // Start is called before the first frame update
    void Start()
    {
        primaryId = GunID.AssaultRifle;
        SetWeapon(primaryId);
        m_fireDelay = false;
        m_reloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.m_gameOver && !MenuMain.Instance.paused)
        {
            Fire();
        }
    }

    private void Fire()
    {
        Vector3 aim = aimScript.GetAimDirection().normalized;
        transform.forward = aim;
        bulletTransform.transform.position = transform.position + aim * 2;
        if (Input.GetMouseButton(0) && !m_fireDelay && !m_reloading)
        {
            switch(primaryId)
            {
                case GunID.Magnum:
                    Magnum();
                    break;
                case GunID.Shotgun:
                    Shotgun();
                    break;
                case GunID.AssaultRifle:
                    Magnum(); // we can just use Magnum
                    break;
                default: Magnum(); break;
            }
        }
        if (bulletsInMag == 0 || (Input.GetKey(KeyCode.R) && bulletsInMag <= magazineSize))
        {
            m_reloading = true;
            StartCoroutine(Reload());
        }
    }

    private void SetWeapon(GunID gunId)
    {
        switch(gunId)
        {
            case GunID.Magnum:
                damagePerBullet = 4;
                magazineSize = 10;
                bulletsInMag = 10;
                fireDelayBetweenShots = 0.7f;
                reloadTime = 1f;
                break;
            case GunID.Shotgun:
                damagePerBullet = 1;
                magazineSize = 5;
                fireDelayBetweenShots = 1.2f;
                reloadTime = 2.5f;
                shellsPerSalvo = 5;
                break;
            case GunID.AssaultRifle:
                damagePerBullet = 1;
                magazineSize = 20;
                bulletsInMag = 20;
                fireDelayBetweenShots = 0.2f;
                reloadTime = 2f;
                break;
            default:
                damagePerBullet = 4;
                magazineSize = 10;
                bulletsInMag = 10;
                fireDelayBetweenShots = 1f;
                reloadTime = 1f;
                break;
        }
    }

    private void Magnum()
    {
        GameObject bullet = poolScript.GetPooledObject();
        bullet.transform.position = bulletTransform.transform.position;
        bullet.SetActive(true);
        //Debug.Log(aimScript.GetAimDirection().normalized);
        bullet.GetComponent<Rigidbody>().velocity = aimScript.GetAimDirection().normalized * bulletSpd;
        m_fireDelay = true;
        StartCoroutine(FireDelay());
        bulletsInMag--;
    }

    private void Shotgun()
    {
        Vector3 aim = aimScript.GetAimDirection().normalized * bulletSpd;
        for (int i=0; i<shellsPerSalvo; i++)
        {
            // Make a cone about the y-axis (xz-plane)
            Quaternion rotation = Quaternion.Euler(0,
                Random.Range(-shotgunSpreadAngle, shotgunSpreadAngle),
                0);
            GameObject bullet = poolScript.GetPooledObject();
            bullet.transform.position = bulletTransform.transform.position;
            bullet.SetActive(true);
            // By multiplying quaternion with the position of the mouse relative to the maincam, we are able to make this cone
            Vector3 direction = (rotation * aim); 
            bullet.GetComponent<Rigidbody>().velocity = (aim + direction);
        }
        m_fireDelay = true;
        StartCoroutine (FireDelay());
        bulletsInMag--;
    }

    // Coroutines
    IEnumerator Reload()
    {
        Debug.Log("Reloading");
        yield return new WaitForSeconds(reloadTime);
        bulletsInMag = magazineSize;
        m_reloading = false;
    }
    IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(fireDelayBetweenShots);
        m_fireDelay = false;
    }
}
