using Unity.VisualScripting;
using UnityEngine;

public class BoonClicker : MonoBehaviour
{
    public GameObject boonPanel;

    public void SendBoon(int boonButton) //for each button in the boon panel
    {
        boonPanel.GetComponent<BoonManager>().AddBoonWeapon(boonButton);
    }
}
