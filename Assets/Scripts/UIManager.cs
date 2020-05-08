using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //LIVES
    [SerializeField]
    private Sprite[] livesSprites;
    [SerializeField]
    private Image livesImg;
    //AMMOCOUNT
    public Text ammoText;
    public Text outOfAmmoText;

    //THRUSTERS
    public Image thrustersBar;
    public Color functionalThurstersColor;


    //GAMEOVER
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject gameOverText;
    [SerializeField]
    float flickerTime = 0.2f;
    [SerializeField]
    private GameManager gameManager;

    //SCORE
    public Text scoreText;

    //PLAYERREF

    public Player player;


    
        
        void Start()
    {
        functionalThurstersColor = thrustersBar.GetComponent<Image>().color;
        scoreText.text = "Score: 0";
        gameOverPanel.SetActive(false);
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        player = GameObject.Find("Player").GetComponent<Player>();
        if( gameManager == null)
        {
            Debug.LogError("Gamemanager is null");
        }
        if (player == null)
        {
            Debug.LogError("playerscript is null");
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateThrustersBar();
       
            if ( player.ammoCount <= 0)
        {
            outOfAmmoText.gameObject.SetActive(true);
        }
        else if (player.ammoCount > 0)
        {
            outOfAmmoText.gameObject.SetActive(false);
        }
        ammoText.text = "Ammo: " + player.ammoCount;
    }

    public void UpdateThrustersBar()
    {
        thrustersBar.fillAmount = player.currentThrustersEnergy / player.maxThrustersEnergy;
        if (player.hasOvercharged == true)
        {
            thrustersBar.GetComponent<Image>().color = Color.red;
        }
        else if (player.hasOvercharged == false)
        {
            thrustersBar.GetComponent<Image>().color = functionalThurstersColor;
        }
    }
    public void UpdateScore(int playerScore)
    {
        scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        livesImg.sprite = livesSprites[currentLives];
    }

    public void OnGameOver() 
    {
        gameOverPanel.SetActive(true);
        StartCoroutine(TextFlicker());
        gameManager.GameOver();
    }

    IEnumerator TextFlicker()
    {
        while(true)
        {
            gameOverText.SetActive(true);
            yield return new WaitForSeconds(flickerTime);
            gameOverText.SetActive(false);
            yield return new WaitForSeconds(flickerTime);
        }
        
    }
        
}
