using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;

public class WeaponSlot : MonoBehaviour
{
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

    public void Update()
    {
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
