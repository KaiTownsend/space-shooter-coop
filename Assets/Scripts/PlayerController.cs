using UnityEngine;

// be able to shoot projectiles that damage enemy health. can be stationary enemy for now. when enemy health is 0, disable/destroy enemy.
public class PlayerController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5f;

    private void Update()
    {
        float inputDirection = GetPlayerInput();
        transform.position += new Vector3(movementSpeed * Time.deltaTime * inputDirection, 0, 0);
    }

    private float GetPlayerInput()
    { 
        return Input.GetAxisRaw("Horizontal");
    }
}
