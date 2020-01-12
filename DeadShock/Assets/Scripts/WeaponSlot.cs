using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [Tooltip("Name of this weapon")]
    public string weaponName;
    [Tooltip("Animator of weaponSlot")]
    public Animator weaponAnimator;
    [Tooltip("Animator of FOV")]
    public Animator fovAnimator;
    private bool ADSenabled = false;    //Do not modify. Determines state of ADS
    [Tooltip("Does current weapon have a scope? If so, show scope overlay")]
    public bool isScopedWeapon = false;
    [Tooltip("Scope overlay for scoped weapons")]
    public GameObject ScopeOverlay;
    [Tooltip("Do not modify; used for detecting current weapon")]
    public Weapon weapon;
    [Tooltip("Weapon name text")]
    public Text weaponNameText;
    [Tooltip("Text object of ammunition")]
    public Text ammoText;
    private int weaponClip = 0;
    [HideInInspector] public int carriedAmmo = 0;

    [Tooltip("Amount of 9mm ammo carried")]
    public int ammo9mm = 0;
    public GameObject AmmoEmpty;

    public void ChangeWeapon()
    {
        if(weapon.ammoType == Ammo.AmmoType.ammo9mm)
        {
            ammo9mm = carriedAmmo;
        }

        //change weapons

        if(weapon.ammoType == Ammo.AmmoType.ammo022mm)
        {
            carriedAmmo = ammo9mm;
        }
    }

    private void SetWeapon()
    {
        if (weapon.ammoType == Ammo.AmmoType.ammo022mm)
        {
            carriedAmmo = ammo9mm;
        }
    }

    private void Start()
    {
        SetWeapon();
    }

    public void Update()
    {

        if (weapon.ammoType == Ammo.AmmoType.ammo9mm)
        {
            carriedAmmo = ammo9mm;
            if(carriedAmmo == 0)
            {
                AmmoEmpty.SetActive(true);
            }
            else if(carriedAmmo != 0 && AmmoEmpty.activeSelf == true)
            {
                AmmoEmpty.SetActive(false);
            }
        }

        weaponName = weapon.name;
        weaponClip = weapon.inMag;

        weaponNameText.text = weaponName;
        ammoText.text = weaponClip.ToString() + "/" + carriedAmmo.ToString();

        if (Input.GetButton("ADS") || Input.GetAxisRaw("ADS") > 0)
        {
            if (ADSenabled == false)
            {
                ADSenabled = !ADSenabled;
                weaponAnimator.SetBool("ADS", ADSenabled);
                fovAnimator.ResetTrigger("ADSDisabled");
                fovAnimator.SetTrigger("ADSEnabled");
                weapon.reticule.SetActive(false);
                if (isScopedWeapon)
                {
                    StartCoroutine(OnScoped());
                }
            }
        }
        else if(Input.GetButtonUp("ADS") || Input.GetAxisRaw("ADS") == 0)
        {
            if (ADSenabled == true)
            {
                ADSenabled = !ADSenabled;
                weaponAnimator.SetBool("ADS", ADSenabled);
                fovAnimator.ResetTrigger("ADSEnabled");
                fovAnimator.SetTrigger("ADSDisabled");
                weapon.reticule.SetActive(true);

                if (isScopedWeapon)
                {
                    ScopeOverlay.SetActive(false);
                    weapon.weaponMesh.enabled = true;
                }
            }
        }
    }

    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(0.15f);
        ScopeOverlay.SetActive(true);
        weapon.weaponMesh.enabled = false;
        weapon.reticule.SetActive(false);
    }
}
