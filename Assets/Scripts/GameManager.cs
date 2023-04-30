using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Mailbox> mailboxes = new List<Mailbox>();

    private int letters;
    private int playerHP = 10;

    public static GameManager main;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
    }

    void Start()
    {
        foreach (Mailbox mailbox in FindObjectsByType<Mailbox>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).ToList())
        {
            mailboxes.Add(mailbox);
        }
        Debug.Log(mailboxes.Count);
        letters = mailboxes.Count;
    }

    void Update()
    {

    }

    public void Deliver()
    {
        letters--;
        if (letters <= 0)
        {
            Debug.Log("You win!");
        }
    }

    public void TakeDamage()
    {
        playerHP--;
        Debug.Log($"CurrentHP: {playerHP}");
        if (playerHP <= 0)
        {
            Debug.Log("You died!");
        }
    }
}
