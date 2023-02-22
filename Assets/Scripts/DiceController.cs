using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiceController : MonoBehaviour
{
    public Die die1;
    public Die die2;
    public Die die3;

    public Vector3 playerPoint;
    public Vector3 enemyPoint;

    private float elapsedTime = 0f;
    private bool waitingForRoll = false;

    private bool playerTurn = true;

    // game stats

    private int playerChips;
    private int enemyChips;
    private int pot;
    private int targetNumber;

    public TMP_Text display;
    public GameObject turnIndicator;
    public Announcer announcer;
    public Announcer dialogue;

    private bool enemyDeciding = false;

    // sound
    public AudioSource audioSource;
    public AudioClip clickSound;
    public AudioClip voiceSound;
    public AudioClip rollSound;
    public AudioClip winSound;

    // Start is called before the first frame update
    void Start()
    {
        playerChips = 5;
        enemyChips = 5;
        pot = 0;
        targetNumber = 0;
        displayScore();
        playerPoint += transform.position;
        enemyPoint += transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (waitingForRoll)
        {
            elapsedTime -= Time.deltaTime;
            if (elapsedTime < 0f)
            {
                int result = getSum();
                if (result == -1)
                {
                    announcer.announce("Bad throw! Rerolling...");
                    beginRoll();
                }
                else
                {
                    waitingForRoll = false;
                    if (result >= targetNumber)
                    {
                        announcer.announce("Roll of " + result + " matches target. New target set!");
                        if (playerTurn)
                        {
                            dialogue.announce("Hmm... alright...");
                        }
                        else
                        {
                            dialogue.announce("Hah! Take that!");
                        }
                        targetNumber = result;
                    }
                    else
                    {
                        targetNumber = 0;
                        if (playerTurn)
                        {
                            enemyChips += pot;
                            announcer.announce("Roll of " + result + " less than target. You lose the pot!");
                            dialogue.announce("Hey, look at that! A win for me!");
                        }
                        else
                        {
                            playerChips += pot;
                            announcer.announce("Roll of " + result + " less than target. You win the pot!");
                            dialogue.announce("Damn... I was sure I had you that time...");
                        }
                        pot = 0;
                    }
                    playerTurn = !playerTurn;
                    displayScore();
                }
            }
        }
        else
        {
            // enemy AI
            if (!playerTurn)
            {
                if (!enemyDeciding)
                {
                    enemyDeciding = true;
                    elapsedTime = 3f;
                }
                else
                {
                    elapsedTime -= Time.deltaTime;
                    if (elapsedTime < 0f)
                    {
                        enemyDeciding = false;
                        enemyMakeMove();
                    }
                }
            }
            else
            {
                // player options
                if (Input.GetKeyDown(KeyCode.R))
                {
                    raiseMove();
                }
                if (Input.GetKeyDown(KeyCode.C))
                {
                    foldMove();
                    announcer.announce("You fold... get 'em next time.");
                    dialogue.announce("Didn't want to risk it? More chips for me then...");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            playerChips = 5;
            enemyChips = 5;
            pot = 0;
            targetNumber = 0;
            displayScore();
            playerTurn = true;
            waitingForRoll = false;
            elapsedTime = 0f;
        }
    }

    public void enemyMakeMove()
    {
        if (playerChips == 0 && pot == 0)
        {
            // run player loss dialogue
            announcer.announce("No more chips... you lose this round.");
            dialogue.announce("Haha, yes! A win! Finally! This is the best luck I’ve been having all day. Say, while my luck is up and your luck is down... care for a rematch?");
            foldMove();
            audioSource.PlayOneShot(winSound, 0.5f);
        }
        else if (enemyChips == 0)
        {
            foldMove();
            // run enemy loss dialogue
            announcer.announce("You have all the chips, so you win!");
            dialogue.announce("Hmph, seems like you've got a bad case of beginner's luck. Ah well, a deal’s a deal. Maybe outrunning the space mafia will give me more time to gamble later...");
            audioSource.PlayOneShot(winSound, 0.5f);
        }
        else if ((targetNumber > 13 && pot < 2))
        {
            foldMove();
            announcer.announce("They fold, the pot is yours!");
            dialogue.announce("Eh, I'm not feeling this one...");
        }
        else
        {
            raiseMove();
            announcer.announce("They raise!");
            dialogue.announce("I'm going for it!");
        }
    }

    public void raiseMove()
    {
        if (playerTurn)
        {
            if (playerChips > 0)
            {
                pot += 1;
                playerChips -= 1;
                displayScore();
                beginRoll();
                announcer.announce("You raise!");
            }
        }
        else
        {
            if (enemyChips > 0)
            {
                pot += 1;
                enemyChips -= 1;
                displayScore();
                beginRoll();
            }
        }
    }

    public void foldMove()
    {
        if (playerTurn)
        {
            enemyChips += pot;
        }
        else
        {
            playerChips += pot;
        }
        pot = 0;
        targetNumber = 0;
        playerTurn = !playerTurn;
        displayScore();
    }

    public void displayScore()
    {
        string score = "Your Chips: " + playerChips + "     Their Chips: " + enemyChips + 
            "\nChips in Pot: " + pot + "     Roll To Beat: " + targetNumber;
        display.SetText(score);
        Vector3 baseScale = turnIndicator.transform.localScale;
        if (playerTurn)
        {
            turnIndicator.transform.localScale = new Vector3(baseScale.x, baseScale.y, Mathf.Abs(baseScale.z));
        }
        else
        {
            turnIndicator.transform.localScale = new Vector3(baseScale.x, baseScale.y, -Mathf.Abs(baseScale.z));
        }
        audioSource.PlayOneShot(clickSound, 0.5f);
    }

    public void beginRoll()
    {
        if (playerTurn)
        {
            rollAll(playerPoint, -1);
        }
        else
        {
            rollAll(enemyPoint, 1);
        }
        waitingForRoll = true;
        elapsedTime = 1.5f;
        audioSource.PlayOneShot(rollSound, 0.5f);
    }

    public void rollAll(Vector3 position, int direction)
    {
        die1.roll(position, direction);
        die2.roll(position, direction);
        die3.roll(position, direction);
    }

    public void rollAll()
    {
        die1.roll();
        die2.roll();
        die3.roll();
    }

    public int getSum()
    {
        int result1 = die1.getResult();
        int result2 = die2.getResult();
        int result3 = die3.getResult();
        if (result1 == -1 || result2 == -1 || result3 == -1)
        {
            return -1;
        }
        return result1 + result2 + result3;
    }
}
