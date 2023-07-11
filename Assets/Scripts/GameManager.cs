using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] Slider staminaBar;
    [SerializeField] GameObject player;
    [SerializeField] GameObject startMenuUI;
    private PlayerController playerCom;
    [SerializeField] bool gameOver = false;
    public bool gameStarted = false;
    public bool movingBack = false;
    public int audioPlay = 0;
    public bool movingUp = false;
    public float durationReset = 7.0f;
    public float elapsedTime;
    private Vector3 startPlayerLerp;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0.0f;
        playerCom = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
{
        

        if (playerCom.lives == 0 && !gameOver)
        {
            GameOver();
        }


        UpdateStaminaBar();
        CursedReset();

    }

    public void UpdatePlayerLives(int life)
    {
        playerCom.lives += life;
        UpdateLivesText();
    }

    public void UpdateLivesText()
    {
        livesText.text = $"Lives: {playerCom.lives}";
    }

    public void UpdatePlayerScore(int scoreToAdd)
    {
        playerCom.score += scoreToAdd;
        UpdateScoresText();
    }

    public void UpdateScoresText()
    {
        scoreText.text = $"Score: {playerCom.score}";
    }

    public void GameOver()
    {
        gameOver = true;
        PauseGame();
        gameOverUI.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        Debug.Log($"timescale = : {Time.timeScale}");
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdateStaminaBar()
    {
        staminaBar.value = playerCom.GetStaminaRate();

        if (playerCom.pl.Exhausted)
        {
            Debug.Log($"exhuasted: {playerCom.stamina} exhuasted: {playerCom.pl.Exhausted}");
            staminaBar.fillRect.GetComponent<Image>().color = Color.blue;
            return;
        }


        if (playerCom.stamina < playerCom.pl.Exhaustion)
        {
            Debug.Log($"playerstam: {playerCom.stamina} cutoff: {playerCom.pl.Exhaustion}");
            staminaBar.fillRect.GetComponent<Image>().color = Color.red;
        }
        else if (playerCom.stamina < playerCom.pl.ExhaustRecovery)
        {
            Debug.Log($"playerstam: {playerCom.stamina} cutoffrecoveerrr: {playerCom.pl.ExhaustRecovery}");
            staminaBar.fillRect.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            Debug.Log($"playerstam: {playerCom.stamina}");
            staminaBar.fillRect.GetComponent<Image>().color = Color.green;
        }

    }

    public void StartGame()
    {
        Time.timeScale = 1.0f;
        UpdatePlayerScore(0);
        playerCom.lives = 3;
        UpdateLivesText();
        staminaBar.gameObject.SetActive(true);
        startMenuUI.SetActive(false);
        gameStarted = true;
        audioPlay = 0;
    }

    public void CursedReset()
    {
       
        if(playerCom.transform.position.z > 650)
        {
            gameStarted = false;
            movingUp = true;
            // Time.timeScale = 0.0f;
        }

        if(movingBack || movingUp)
        {
            GameObject[] listMeteor = GameObject.FindGameObjectsWithTag("Meteor");
            foreach (GameObject meteor in listMeteor)
            {
                meteor.SetActive(false);
            }

            staminaBar.gameObject.SetActive(false);

            ResetPlayer();
        }
    }

    public void ResetPlayer()
    {
        StartCoroutine(MovePlayer());
    }

    IEnumerator MovePlayer()
    {
        if(player.transform.position.y < 10 && movingUp)
        {
            audioPlay++;
            if(audioPlay == 1)
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
            //room for further attempts
            player.transform.Translate(Vector3.up * Time.deltaTime);
            startPlayerLerp = player.transform.position;
        }
        else
        {
            movingUp = false;
            movingBack = true;
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / durationReset;
            player.transform.position = Vector3.Lerp(startPlayerLerp, playerCom.startPos, percentageComplete);
            //player.transform.Translate((playerCom.startPos - player.transform.position).normalized * Time.deltaTime * resetSpeed);
        }
        
        if(player.transform.position == playerCom.startPos) 
        {
            player.transform.rotation = playerCom.startRot;
            gameStarted = true;
            movingBack = false;
            movingUp = false;
            StartGame();
            elapsedTime = 0;
        }

        yield return new WaitForSeconds(Time.deltaTime);
    }
}
