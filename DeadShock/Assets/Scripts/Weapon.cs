//Written by Michael "Arylos" Cox

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Weapon : MonoBehaviour
{
    [Tooltip("Damage per attack for this weapon")]
    public int gunDamage = 1;
    public Ammo.AmmoType ammoType;
    [Tooltip("Whether or not the weapon is automatic (can be fired by only holding down 'fire')")]
    public bool isAuto;
    [Tooltip("Time delay during reload")]
    public float reloadDelay;
    [Tooltip("Currently in mag")]
    public int inMag;
    [Tooltip("Mag size")]
    public int magSize;
    [Tooltip("Shots per second")]
    public float fireRate = 0.25f;
    [Tooltip("Range of the weapon")]
    public float range = 50f;
    [Tooltip("force each attack impacts on hit target")]
    public float hitForce = 100f;
    [Tooltip("location of the gun barrel, where raycasting begins")]
    public Transform gunBarrel;
    public Camera playerCamera;
    [Tooltip("Mesh renderer of object")]
    public MeshRenderer weaponMesh;
    [Tooltip("Reticule on-screen")]
    public GameObject reticule;
    public bool canFire;
    public WeaponSlot slot;

    //This value is used to show a raycast line after the gun fires to show the bullet location. Do not change this value;
    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);
    private AudioSource weaponAudio;
    private LineRenderer laserLine;
    private float nextFire;


	// Use this for initialization
	void Start ()
    {
        laserLine = GetComponent<LineRenderer>();
        weaponAudio = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(inMag != 0)
        {
            canFire = true;
        }
        else if (inMag == 0)
        {
            canFire = false;
        }

        if(Input.GetButtonDown("Reload") && inMag != magSize)
        {
            Invoke("Reload", reloadDelay);
        }

        if(isAuto == true && canFire == true)
        {
            if (Input.GetButton("Fire1") || Input.GetAxisRaw("Fire1") > 0)
            {
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    StartCoroutine(ShotEffect());

                    Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
                    RaycastHit hit;
                    laserLine.SetPosition(0, gunBarrel.position);

                    if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, range))
                    {
                        laserLine.SetPosition(1, hit.point);
                        DamageStage health = hit.collider.GetComponent<DamageStage>();
                        if (health != null)
                        {
                            health.Damage(gunDamage);
                        }
                        if (hit.rigidbody != null)
                        {
                            hit.rigidbody.AddForce(-hit.normal * hitForce);
                        }
                    }
                    else
                    {
                        laserLine.SetPosition(1, rayOrigin + (playerCamera.transform.forward * range));
                    }
                }
            }
        }
        else if(isAuto == false && canFire == true)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetAxisRaw("Fire1") > 0)
            {
                canFire = false;
                StartCoroutine(ShotEffect());

                Vector3 rayOrigin = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hit;
                laserLine.SetPosition(0, gunBarrel.position);

                if (Physics.Raycast(rayOrigin, playerCamera.transform.forward, out hit, range))
                {
                    laserLine.SetPosition(1, hit.point);
                    DamageStage health = hit.collider.GetComponent<DamageStage>();
                    if (health != null)
                    {
                        health.Damage(gunDamage);
                    }
                    if (hit.rigidbody != null)
                    {
                        hit.rigidbody.AddForce(-hit.normal * hitForce);
                    }
                }
                else
                {
                    laserLine.SetPosition(1, rayOrigin + (playerCamera.transform.forward * range));
                }
            }
            if(canFire == false && inMag != 0)
            {
                if(Input.GetButtonUp("Fire1") || Input.GetAxisRaw("Fire1") == 0)
                {
                    canFire = true;
                }
            }
            
        }
	}

    private IEnumerator ShotEffect()
    {
        inMag--;
        laserLine.enabled = true;
        weaponAudio.Play();
        yield return shotDuration;
        laserLine.enabled = false;
        if (inMag == 0)
        {
            canFire = false;
            yield return new WaitForSeconds(reloadDelay);
            Reload();
        }
    }

    public void Reload()
    {
        if (magSize < slot.carriedAmmo)
        {
            inMag = magSize;
            slot.carriedAmmo -= magSize;
            canFire = true;
        }
        else if (magSize > slot.carriedAmmo && slot.carriedAmmo != 0)
        {
            inMag = slot.carriedAmmo;
            slot.carriedAmmo -= inMag;
            canFire = true;
        }
        else
        {
            canFire = false;
        }
    }
}
