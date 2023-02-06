using UnityEngine;

public class PointersAnimator : MonoBehaviour
{
    public Vector3 minimum;
    public Vector3 maximum;

    public float speed = 0.2f;

    // starting value for the Lerp
    float t;

    private void Start()
    {
        t = 0.0f;
    }
    void FixedUpdate()
    {
        // animate the position of the game object...
        transform.localPosition = Vector3.Lerp(minimum, maximum, t);

        // .. and increase the t interpolater
        t += speed * Time.deltaTime;

        // now check if the interpolator has reached 1.0
        // and swap maximum and minimum so game object moves
        // in the opposite direction.
        if (t > 1.0f)
        {
            Vector3 temp = maximum;
            maximum = minimum;
            minimum = temp;
            t = 0.0f;
        }
    }

    private void OnEnable()
    {
        t = 0.0f;
    }
}
