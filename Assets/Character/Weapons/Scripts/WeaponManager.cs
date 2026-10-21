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

    public void AddWeapon(WeaponData weaponData) //CHANGE TO ATTACK FOR LATER
    {
        GameObject weaponGameObject = Instantiate(weaponData.weaponPrefab, weaponsTransform);

        WeaponBase weaponBase = weaponGameObject.GetComponent<WeaponBase>();

        weaponBase.SetData(weaponData);

    }
}
