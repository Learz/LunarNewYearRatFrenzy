using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Player.Identity identity;
    public Player.Color defaultColor;
    public GameObject playerJoined, playerReady, playerDisconnected;
    public bool playerIsConnected, playerIsReady;
    public Image background, playerPose;
    private PlayerInput playerInput;
    private RatManager mgr;
    private enum moveDirection
    {
        right = 0,
        up = 1,
        left = 2,
        down = 3
    }
    private moveDirection lastDirection;
    private bool isMoving;

    private void Start()
    {
        GameManager.instance.playerJoined.AddListener(PlayerJoined);
    }
    public void PlayerJoined(PlayerInput input)
    {
        if (mgr != null) return;
        mgr = GameManager.instance.GetRatManager(identity);
        if (mgr == null) return;
        playerDisconnected.SetActive(false);
        playerJoined.SetActive(true);
        playerInput = input;
        mgr.onJumpDown.AddListener(Jump);
        mgr.onInteractDown.AddListener(Interact);
        mgr.onMove.AddListener(Move);
        mgr.onMoveCancelled.AddListener(Move);
        mgr.color = defaultColor;
        mgr.poseIndex = (int)identity;
        background.color = mgr.GetPlayerColor();
        playerPose.sprite = mgr.GetPoseSprite();
        //input.currentActionMap.actionTriggered += actionTriggered;
        playerIsConnected = true;
    }
    private void Move()
    {
        if (playerIsReady) return;
        if (mgr.move.magnitude < 0.5)
        {
            isMoving = false;
            return;
        }
        if (isMoving) return;
        isMoving = true;
        float angle = Mathf.Atan2(mgr.move.y, mgr.move.x);
        int octant = (int)Mathf.Round(4 * angle / (2 * Mathf.PI) + 4) % 4;

        lastDirection = (moveDirection)octant;

        switch (lastDirection)
        {
            case moveDirection.up:
                PrevColor();
                break;
            case moveDirection.down:
                NextColor();
                break;
            case moveDirection.left:
                PrevChar();
                break;
            case moveDirection.right:
                NextChar();
                break;
        }

    }
    private void NextColor()
    {
        do
        {
            mgr.color = mgr.color.Next();
        } while (PlayerSelection.instance.selectedColors.Contains(mgr.color));
        background.color = mgr.GetPlayerColor();
    }
    private void PrevColor()
    {
        do
        {
            mgr.color = mgr.color.Prev();
        } while (PlayerSelection.instance.selectedColors.Contains(mgr.color));
        background.color = mgr.GetPlayerColor();
    }
    private void NextChar()
    {

        mgr.poseIndex = (mgr.poseIndex + 1 > GameManager.instance.playerPoses.Length - 1) ? 0 : mgr.poseIndex + 1;
        playerPose.sprite = mgr.GetPoseSprite();
    }
    private void PrevChar()
    {
        mgr.poseIndex = (mgr.poseIndex - 1 < 0) ? GameManager.instance.playerPoses.Length - 1 : mgr.poseIndex - 1;
        playerPose.sprite = mgr.GetPoseSprite();
    }
    private void Jump()
    {
        if (!playerIsReady)
        {
            if (PlayerSelection.instance.selectedColors.Contains(mgr.color)) return;
            Debug.Log("Player Ready");
            playerReady.SetActive(true);
            playerIsReady = true;
            PlayerSelection.instance.PlayerReady();
            PlayerSelection.instance.selectedColors.Add(mgr.color);
        }
    }
    private void Interact()
    {
        if (playerIsReady)
        {
            Debug.Log("Player Not Ready");
            playerReady.SetActive(false);
            playerIsReady = false;
            PlayerSelection.instance.PlayerNotReady();
            PlayerSelection.instance.selectedColors.Remove(mgr.color);
        }
        else
        {
            Debug.Log("Removing user");
            Destroy(playerInput.gameObject);
        }
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
    private void OnEnable()
    {
        if (playerInput != null)
        {
            //playerInput.currentActionMap.actionTriggered += actionTriggered;
            playerIsReady = false;
            playerReady.SetActive(false);
            playerDisconnected.SetActive(false);
            playerJoined.SetActive(true);
            mgr.onJumpDown.AddListener(Jump);
            mgr.onInteractDown.AddListener(Interact);
            mgr.onMove.AddListener(Move);
            mgr.onMoveCancelled.AddListener(Move);
        }

    }
    private void OnDisable()
    {
        //if (playerInput != null) playerInput.currentActionMap.actionTriggered -= actionTriggered;
        if (mgr != null)
        {
            mgr.onJumpDown.RemoveListener(Jump);
            mgr.onInteractDown.RemoveListener(Interact);
            mgr.onMove.RemoveListener(Move);
            mgr.onMoveCancelled.RemoveListener(Move);
        }

    }
}
