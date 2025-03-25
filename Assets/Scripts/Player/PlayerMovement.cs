using FishNet.Object;
using UnityEngine;
using FishNet.Object.Synchronizing;

public class PlayerMovement : MonoBehaviour
{
    public GameObject playerObject;
    private Player playerController;
    #region Unity Methods
    void Start()
    {
        playerObject = GameObject.FindWithTag("Player");
        playerController = playerObject.GetComponent<Player>();
    }
    void Update()
    {
        Movement();
        SpeedUpMovement();
        ZoomIn();
        ZoomOut();
    }
    #endregion
    #region Movement Methods
    private void Movement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        if (movement.magnitude > 1f)
        {
            movement = movement.normalized;
        }
        transform.Translate(movement * playerController.movementSpeed * Time.deltaTime, Space.World);
    }
    private void SpeedUpMovement()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(playerController.movementSpeed >= 30)
            {
                return;
            }
            playerController.movementSpeed = playerController.movementSpeed * 2;
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            playerController.movementSpeed = 15;
        }
    }
    private void ZoomIn()
    {
        if(Input.GetKey(KeyCode.E) && playerController.playerCamera.fieldOfView >= 11)
        {
            playerController.playerCamera.fieldOfView--;
        }
    }
    private void ZoomOut()
    {
        if(Input.GetKey(KeyCode.Q) && playerController.playerCamera.fieldOfView <= 62)
        {
            playerController.playerCamera.fieldOfView++;
        }
    }
    #endregion
}