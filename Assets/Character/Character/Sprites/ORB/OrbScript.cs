using UnityEngine;

public class OrbScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HELLO");

        Character c = collision.GetComponent<Character>();

        if(c != null)
        {
            c.ActivateBoonPanel();
            Destroy(gameObject);
        }
    }
}
