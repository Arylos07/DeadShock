//Written by Michael "Arylos" Cox

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public bool isPlayer;
    [HideInInspector]
    public int currentHealth;
    public int maxHealth;
    [HideInInspector]
    public Image healthBar;

	// Use this for initialization
	void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isPlayer)
        {
            healthBar.fillAmount = (float)currentHealth / (float)maxHealth;
            if (currentHealth > (maxHealth / 2))
            {
                healthBar.color = new Color((1.0f - ((float)(currentHealth) / (float)maxHealth))*2.0f, 1, 0);
            }
            else if(currentHealth < (maxHealth / 2))
            {
                healthBar.color = new Color(1, ((float)(currentHealth) / (float)maxHealth) * 2.0f, 0);
            }
        }
	}
}

[CustomEditor(typeof(Health))]
public class HealthEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        {
            var script = target as Health;

            if (script.isPlayer)
            {
                script.healthBar = (Image)EditorGUILayout.ObjectField("Health bar:", script.healthBar, typeof(Image), true);
                script.currentHealth = EditorGUILayout.IntSlider("DebugHealth:", script.currentHealth, 1, script.maxHealth);
            }
        }
    }
}

