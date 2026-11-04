using System.Collections;
using UnityEngine;

public class Fireball : WeaponBase
{
    public WeaponData explosion;
    public override void Attack()
    {
        GameObject weaponGameObject = Instantiate(explosion.weaponPrefab, gameObject.transform);

        WeaponBase weaponBase = weaponGameObject.GetComponent<WeaponBase>();

        weaponBase.SetData(explosion);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = Vector3.zero;

        StartCoroutine(Wait(explosion.weaponStats.attackBehavior.timeToDestroy));
    }

    private IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);

        yield return null;
    }
}
