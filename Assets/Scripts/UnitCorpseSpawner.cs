using System;
using UnityEngine;

public class UnitCorpseSpawner : MonoBehaviour
{
    [SerializeField] private Transform corpsePrefab;
    HealthSystem healthSystem;

    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        Transform corpseTransform = Instantiate(corpsePrefab,transform.position,transform.rotation);
    }
}
