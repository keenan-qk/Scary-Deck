using System;
using UnityEngine;

[Serializable]
public class WeaponStats
{
    public int damage;

    public int timeToAttack;
}

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    public string Name;

    public WeaponStats weaponStats;

    public GameObject weaponPrefab;
}
