using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    public GameObject playerJoined, playerReady, playerDisconnected;
    public bool playerIsConnected, playerIsReady;
    private PlayerInput playerInput;
    public void PlayerJoined(PlayerInput input)
    {
        playerDisconnected.SetActive(false);
        playerJoined.SetActive(true);
        playerInput = input;
        input.currentActionMap.actionTriggered += actionTriggered;
        playerIsConnected = true;
    }

    private void actionTriggered(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (ctx.action.name.Contains("Jump"))
        {
            if (!playerIsReady)
            {
                Debug.Log("Player Ready");
                playerReady.SetActive(true);
                playerIsReady = true;
                PlayerSelection.instance.PlayerReady();
            }
        }
        if (ctx.action.name.Contains("Interact"))
        {
            if (playerIsReady)
            {
                Debug.Log("Player Not Ready");
                playerReady.SetActive(false);
                playerIsReady = false;
                PlayerSelection.instance.PlayerNotReady();
            }
            else
            {
                Debug.Log("Removing user");
                Destroy(playerInput.gameObject);
            }
        }
    }

    public void PlayerLeft()
    {
        playerIsConnected = false;
        playerDisconnected.SetActive(true);
        playerReady.SetActive(false);
        playerJoined.SetActive(false);
    }
}
