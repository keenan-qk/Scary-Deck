using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class AttackBehavior
{
    public float degreeOfRotation; //how much the weapon rotates
    public bool isRotate; //is it supposed to rotate
    public float timeToDestroy; //how long the duration of the attack is
    public bool isOneTimeDestroy; //is it supposed to destroy once it hits a single object
    public bool isItem; //is it an item
    public bool isSpawnObject; //should there be an extra object that is spawned
}

[Serializable]
public class WeaponStats
{
    public int damage;

    public int projectileSpeed;

    public AttackBehavior attackBehavior;

    //public int timeToAttack;
}

[CreateAssetMenu]
public class WeaponData : ScriptableObject //THE STATS, EFFECTS, AND FUNCTIONALITY OF ALL WEAPONS
{
    public string Name;

    public Sprite boonImage;

    public WeaponStats weaponStats;

    public GameObject weaponPrefab;
}
