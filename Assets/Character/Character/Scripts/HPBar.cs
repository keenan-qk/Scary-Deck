using UnityEngine;

public class HPBar : MonoBehaviour
{
    public Transform bar;
    public void setState(float current, float max)
    {
        float state = current;
        state /= max;
        if (state < 0f) { state = 0f; }
        if(state > 1)
        {
            state = 1;
        }
        bar.transform.localScale = new Vector3(state, 1f, 1f);
    }
}
