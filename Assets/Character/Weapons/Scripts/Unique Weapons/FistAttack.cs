using UnityEditor.Analytics;
using UnityEngine;

public class FistAttack : WeaponBase
{
    private void Start()
    {
        Stall(.5f);
    }

    public override void Attack()
    {
        
    }

    public override void OnDeath()
    {
        
    }
}
