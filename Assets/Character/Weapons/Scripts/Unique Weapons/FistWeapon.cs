using UnityEngine;

public class FistWeapon : WeaponBase
{
    public WeaponData fist;
    public GameObject charge;
    float change;

    private void Start()
    {
        StartFist();

        change = .02f;

        GameObject weaponGameObject = Instantiate(fist.weaponPrefab, gameObject.transform);

        WeaponBase weaponBase = weaponGameObject.GetComponent<WeaponBase>();

        weaponBase.SetData(fist);

        weaponBase.SetInitVel(gameObject.GetComponent<Rigidbody2D>().linearVelocity);

        charge.transform.position += new Vector3(-.01f, 0, 0);
        charge.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void Update()
    {
        charge.transform.position = GameObject.FindWithTag("Player").transform.position + new Vector3(change, 0, 0);
        charge.transform.rotation = Quaternion.Euler(Vector3.zero);

        change *= -1;

        base.Update();
    }

    public override void Attack()
    {
        charge.SetActive(false);
        Fist();
    }

    public override void OnDeath()
    {
        
    }
}

