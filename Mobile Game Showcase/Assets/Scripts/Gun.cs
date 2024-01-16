using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [Min(0) ,SerializeField] private float bulletLifeTime = 1, bulletSpeed = 1;
    [Min(0)] public int damageToDeal = 1;
    [SerializeField] private AudioClip shootSFX;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = -Camera.main.transform.position.z;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 shootDir = mousePosition - transform.position;
        shootDir.Normalize();
        bullet.GetComponent<Rigidbody2D>().velocity = 10 * bulletSpeed * shootDir;
        Camera.main.GetComponent<AudioSource>().PlayOneShot(shootSFX);
        Destroy(bullet, bulletLifeTime);
    }
}
