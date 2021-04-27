using UnityEngine;

public class SpinLight : MonoBehaviour
{
    private float speed;

    private void Start()
    {
        // Give the spin light a random speed
        speed = Random.Range(-180f, 180f);
    }

    private void Update()
    {
        // Rotate the spin light
        transform.Rotate(0, 0, speed * Time.deltaTime);
    }
}