using System;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class WeaponStats
{
    public int damage;

    public int projectileSpeed;

    public int timeToAttack;
}

[CreateAssetMenu]
public class WeaponData : ScriptableObject //THE STATS, EFFECTS, AND FUNCTIONALITY OF ALL WEAPONS
{
    public string Name;

    public Sprite boonImage;

    public WeaponStats weaponStats;

    public GameObject weaponPrefab;
}
