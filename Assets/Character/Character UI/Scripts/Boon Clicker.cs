using Unity.VisualScripting;
using UnityEngine;

public class BoonClicker : MonoBehaviour
{
    public GameObject boonPanel;

    public void SendBoon(int boonButton)
    {
        boonPanel.GetComponent<BoonManager>().AddBoonWeapon(boonButton);
    }
}
