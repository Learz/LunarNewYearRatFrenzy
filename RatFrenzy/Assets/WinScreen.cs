using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    public AudioClip song;
    public Animator fadeAnimator;
    public List<BallController> players;
    public Cinemachine.CinemachineTargetGroup targetGroup;
    private List<Player.Identity> winners;
    int nextWinner = 0;
    private void Start()
    {
        GameManager.instance.PlayMusic(song);
        Physics.gravity = new Vector3(0, -1.62f, 0);
        winners = GameManager.instance.GetLeaders();
        players[(int)winners[0]].gameObject.SetActive(true);
        targetGroup.AddMember(players[(int)winners[0]].transform, 1, 2);
    }
    private void FadeToBlack()
    {
        fadeAnimator.Play("fadetoblack");
    }
    public void SendNextWinner()
    {
        nextWinner++;
        if (nextWinner >= winners.Count) return;
        players[(int)winners[nextWinner]].gameObject.SetActive(true);
        players[(int)winners[nextWinner]].GetComponent<Rigidbody>().isKinematic = false;
        targetGroup.AddMember(players[(int)winners[nextWinner]].transform, 1, 2);
    }
}
