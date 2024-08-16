using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerNetwork : NetworkBehaviour
{
    [Header("Status")]
    public NetworkVariable<bool> IsReady = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] Vector2 moveInput;

    private GameManager gameManagerInstance;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Find the GameManager instance
        gameManagerInstance = FindObjectOfType<GameManager>();

        // Server listens to the change in readiness
        if (IsServer)
        {
            IsReady.OnValueChanged += HandleReadyStatusChanged;
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        transform.position += moveSpeed * Time.deltaTime * new Vector3(moveInput.y, 0, 0);
    }

    public void OnMovePlayer(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnReady()
    {
        if (IsOwner)
        {
            IsReady.Value = true; 
            Debug.Log("Player is ready!");
        }
    }

    // This method is called when the IsReady NetworkVariable changes
    private void HandleReadyStatusChanged(bool previousValue, bool newValue)
    {
        if (gameManagerInstance != null)
        {
            // Notify GameManager of the change
            gameManagerInstance.PlayerReadyStatusChanged(IsOwner, newValue);  
        }
    }
}