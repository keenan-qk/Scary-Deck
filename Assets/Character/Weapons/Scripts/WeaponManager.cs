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
        weapons = new List<WeaponBase>(); //FOR LATER
    }

    public void AddWeapon(WeaponData weaponData)
    {
        GameObject weaponGameObject = Instantiate(weaponData.weaponPrefab, weaponsTransform);

        WeaponBase weaponBase = weaponGameObject.GetComponent<WeaponBase>();

        weaponBase.SetData(weaponData);

    }
}
