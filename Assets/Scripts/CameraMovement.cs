using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y, transform.position.z);
    }
}
