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
    [SerializeField] private int fireDelayBetweenShots;
    [SerializeField] private int reloadTime;

    // Variables
    private bool m_fireDelay;

    public int DamagePerBullet { get { return damagePerBullet; } set { damagePerBullet = value; } }

    // Variables
    private GunID primaryId;

    // Start is called before the first frame update
    void Start()
    {
        primaryId = GunID.Magnum;
        SetWeapon(primaryId);
        m_fireDelay = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.m_gameOver)
        {
            Fire();
        }
    }

    private void Fire()
    {
        Vector3 aim = aimScript.GetAimDirection().normalized;
        transform.forward = aim;
        bulletTransform.transform.position = transform.position + aim * 2;
        if (Input.GetMouseButtonDown(0) && !m_fireDelay && bulletsInMag > 0)
        {
            GameObject bullet = poolScript.GetPooledObject();
            bullet.transform.position = bulletTransform.transform.position;
            bullet.SetActive(true);
            bullet.GetComponent<Rigidbody>().velocity = aimScript.GetAimDirection().normalized * bulletSpd;
            m_fireDelay = true;
            StartCoroutine(FireDelay());
            bulletsInMag--;
        }
        if (bulletsInMag == 0)
        {
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
                fireDelayBetweenShots = 1;
                reloadTime = 3;
                break;
            default:
                damagePerBullet = 4;
                magazineSize = 10;
                bulletsInMag = 10;
                fireDelayBetweenShots = 1;
                reloadTime = 3;
                break;
        }
    }

    IEnumerator Reload()
    {
        Debug.Log("Reloading");
        yield return new WaitForSeconds(reloadTime);
        bulletsInMag = magazineSize;
    }
    IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(fireDelayBetweenShots);
        m_fireDelay = false;
    }
}
