using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Weapon : MonoBehaviour
{
    [Tooltip("Damage per attack for this weapon")]
    public int gunDamage = 1;
    [Tooltip("Shots per second")]
    public float fireRate = 0.25f;
    [Tooltip("Range of the weapon")]
    public float range = 50f;
    [Tooltip("force each attack impacts on hit target")]
    public float hitForce = 100f;
    [Tooltip("location of the gun barrel, where raycasting begins")]
    public Transform gunBarrel;
    public Camera playerCamera;

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
        if (Input.GetButton("Fire1") && Time.time > nextFire)
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
                if(health != null)
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

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        weaponAudio.Play();
        yield return shotDuration;
        laserLine.enabled = false;
    }
}
