using System;
using UnityEngine;
using UnityEngine.UIElements;

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

    public Sprite boonImage;

    public WeaponStats weaponStats;

    public GameObject weaponPrefab;
}
