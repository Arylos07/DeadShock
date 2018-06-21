//Written by Michael "Arylos" Cox

using UnityEngine;
using System.Collections;

public class DamageStage : MonoBehaviour
{
    //This script is a placeholder for future enemy scripts.
    //Disregard this script in final gameplay

    public int currentHealth = 3;

    public void Damage(int damageAmount)
    {

        //subtract damage amount when Damage function is called

        currentHealth -= damageAmount;

        //Check if health has fallen below zero

        if (currentHealth <= 0) 
        {

            //if health has fallen below zero, deactivate it 

            gameObject.SetActive(false);

        }

    }

}