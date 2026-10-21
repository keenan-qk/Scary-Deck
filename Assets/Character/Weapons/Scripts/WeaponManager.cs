using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class WeaponManager : MonoBehaviour
{
    public Transform weaponsTransform;
    List<WeaponBase> weapons;

    void Start()
    {
        weapons = new List<WeaponBase>(); //FOR LATER (probably not actually)
    }

    public void CardAttack(WeaponData weaponData, Vector3 mousePos) //CHANGE TO ATTACK FOR LATER
    {
        GameObject weaponGameObject = Instantiate(weaponData.weaponPrefab, weaponsTransform);

        WeaponBase weaponBase = weaponGameObject.GetComponent<WeaponBase>();

        weaponBase.SetData(weaponData, weaponsTransform.transform.position, mousePos);

        DirectionOfAttack(weaponGameObject, weaponsTransform, mousePos, weaponData.weaponStats.projectileSpeed);
    }

    private void DirectionOfAttack(GameObject wp, Transform wpTransform, Vector3 mousePos, float projSpeed)
    {
        Vector2 direction = (mousePos - wpTransform.transform.position).normalized;

        Rigidbody2D rb = wp.GetComponent<Rigidbody2D>();

        rb.linearVelocity = direction * projSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        wp.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
