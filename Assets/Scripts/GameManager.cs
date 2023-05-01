using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private AudioSource deliverySfx;

    [SerializeField]
    private List<GameObject> hearts;

    [SerializeField]
    private List<GameObject> letterNumbers;

    [SerializeField]
    private GameObject DialogBG;

    [SerializeField]
    private GameObject VictoryText;

    [SerializeField]
    private GameObject GameOverText;

    private List<Mailbox> mailboxes = new List<Mailbox>();

    private int letters;
    private int playerHP = 10;

    public static GameManager main;

    public bool GameOver = false;

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

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Update()
    {
        for (int i = 0; i < 10; i++)
        {
            hearts[i].SetActive(true);

            if (i >= playerHP)
            {
                hearts[i].SetActive(false);
            }
        }

        for (int i = 0; i < 6; i++)
        {
            letterNumbers[i].SetActive(false);

            if (i == letters)
            {
                letterNumbers[i].SetActive(true);
            }
        }
    }

    public void Deliver()
    {
        deliverySfx.PlayOneShot(deliverySfx.clip);
        letters--;
        if (letters <= 0)
        {
            Debug.Log("You win!");
            DialogBG.SetActive(true);
            VictoryText.SetActive(true);
            GameOver = true;
        }
    }

    public void TakeDamage()
    {
        playerHP--;
        Debug.Log($"CurrentHP: {playerHP}");
        if (playerHP <= 0)
        {
            Debug.Log("You died!");
            DialogBG.SetActive(true);
            GameOverText.SetActive(true);
            GameOver = true;
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}
