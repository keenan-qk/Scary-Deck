using UnityEngine;

public class DamageMessage : MonoBehaviour
{
    public float timeToLive = 1f;
    float ttl = 1f;

    private void OnEnable()
    {
        ttl = timeToLive;
    }

    private void Update()
    {
        ttl -= Time.deltaTime;

        if(ttl < 0)
        {
            gameObject.SetActive(false);
        }
    }
}
