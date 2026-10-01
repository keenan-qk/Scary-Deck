using System;
using UnityEngine;

[Serializable]
public class WeaponStats
{
    public int damage;
}

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    public string Name;

    public WeaponStats stats;

    public GameObject weaponPrefab;
}
