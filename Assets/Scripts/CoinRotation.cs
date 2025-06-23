using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    [SerializeField] private float flipSpeed = 2f; // kecepatan flip

    private void Update()
    {
        float yRotation = Mathf.PingPong(Time.time * flipSpeed, 180f); // balik 0 - 180 derajat
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}
