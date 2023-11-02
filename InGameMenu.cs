using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{

    [Header("GameObjects")]
    public GameObject character;
    public GameObject gameOverPanel;
    public GameObject statsPanel;
    public GameObject HealthPanel;
    public GameObject pausePanel;
    public Sprite health5;
    public Sprite health4;
    public Sprite health3;
    public Sprite health2;
    public Sprite health1;
    public Sprite health0;

    [Header("KeyBindings")]
    public KeyCode pause1 = KeyCode.P;
    public KeyCode pause2 = KeyCode.Escape;

    private Vector3 charStartingPos;

    private string currentSong;

    void Start()
    {
        
        this.charStartingPos= this.character.GetComponent<Transform>().position;
        
    }

    private void LateUpdate()
    {
        if (Input.GetKey(this.pause1)|| Input.GetKey(this.pause2))
        {
            this.pauseGame();
        }
    }


    public void resetGame()
    {
        /*this.charStatsObj.resetAll();
        this.gameOverPanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        FindObjectOfType<AudioManager>().Stop("Music1");
        FindObjectOfType<AudioManager>().Play("NatureBackground");
        FindObjectOfType<AudioManager>().Play("WindBackground");
        FindObjectOfType<AudioManager>().Play("Skiing");
        this.character.GetComponent<Transform>().position = this.charStartingPos;*/

        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("click reset");
    }

    public void pauseGame()
    {
        
        this.currentSong = this.character.GetComponent<GameLogic>().currentSong;
        FindObjectOfType<AudioManager>().tuneVolume(this.currentSong, 0.08f);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        float tempTstamp=FindObjectOfType<AudioManager>().pauseAll(this.currentSong);
        FindObjectOfType<AudioManager>().playTimestamp(tempTstamp,this.currentSong);
        this.pausePanel.SetActive(true);
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        this.pausePanel.SetActive(false);
        //FindObjectOfType<AudioManager>().Play("NatureBackground");
        FindObjectOfType<AudioManager>().Play("WindBackground");
        FindObjectOfType<AudioManager>().Play("Skiing");
        FindObjectOfType<AudioManager>().tuneVolume(this.currentSong, 0.4f);
    }



    public void updateHealthSprite(int newHealth)
    {
        if (newHealth == 5)
        {
            this.HealthPanel.GetComponent<Image>().sprite = this.health5;
        }
        if (newHealth == 4)
        {
            this.HealthPanel.GetComponent<Image>().sprite = this.health4;
        }
        if (newHealth==3)
        {
            this.HealthPanel.GetComponent<Image>().sprite = this.health3;
        }
        if (newHealth == 2)
        {
            this.HealthPanel.GetComponent<Image>().sprite = this.health2;
        }
        if (newHealth == 1)
        {
            this.HealthPanel.GetComponent<Image>().sprite = this.health1;
        }
        if (newHealth == 0)
        {
            this.HealthPanel.GetComponent<Image>().sprite = this.health0;
        }
    }

    public void loadMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void HoverButton()
    {
        FindObjectOfType<AudioManager>().Play("Hover");
    }
    public void SelectButton()
    {
        FindObjectOfType<AudioManager>().Play("Select");
    }


}
