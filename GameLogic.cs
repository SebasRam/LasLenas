using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameLogic : MonoBehaviour
{
    [Header("Health Controlls")]
    public int currentHealth;

    [Header("Distance Traveled")]
    public Vector3 startPosition;
    public Vector3 prevPosition;
    public Vector3 currentPosition;

    [Header("GameObjects")]
    public GameObject characterMesh;
    public CharacterStats charStatsObj;
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject gameUI;
    public TextMeshProUGUI time;
    private float gameTime;
    public TextMeshProUGUI livesLeft;
    public GameObject snowManPrefab;
    public GameObject InGameUI;
    private InGameMenu gameUIControllObject;

    [Header("Collision Controller")]
    public bool readyCollide;
    public float collisionCoolDown;

    public string currentSong= "Fast_lane";


    // Start is called before the first frame update
    void Start()
    {
        this.startPosition = this.characterMesh.GetComponent<Transform>().position;
        this.currentPosition = this.startPosition;
        this.prevPosition = this.startPosition;
        this.charStatsObj = this.GetComponent<CharacterStats>();
            
        
        this.collisionCoolDown = 0.5f;
        this.readyCollide = true;

        this.gameUIControllObject = this.InGameUI.GetComponent<InGameMenu>();

        //FindObjectOfType<AudioManager>().Play("NatureBackground");
        FindObjectOfType<AudioManager>().Play("WindBackground");
        FindObjectOfType<AudioManager>().Play("Skiing");
        float temp = Random.Range(1,4);
        if (temp==1)
        {
            this.currentSong = "Dig_it";
        }
        if (temp == 2)
        {
            this.currentSong = "Distressed";
        }
        if (temp == 3)
        {
            this.currentSong = "Fast_lane";
        }
        FindObjectOfType<AudioManager>().Play(this.currentSong);
    }

    // Update is called once per frame
    void Update()
    {
        this.calculateDistanceTravelled();
        this.currentHealth = this.charStatsObj.getHealth();
        this.gameTime += Time.deltaTime;
    }

    public void calculateDistanceTravelled()
    {
        this.currentPosition= this.characterMesh.GetComponent<Transform>().position;
        //float distanceTravelled = Vector3.Distance(this.currentPosition, this.prevPosition);
        float distanceTravelled = Vector2.Distance(new Vector2(this.currentPosition.x, this.currentPosition.z),new Vector2(this.prevPosition.x, this.prevPosition.z));
        this.prevPosition = this.currentPosition;
        this.charStatsObj.increaseDistance(distanceTravelled);
    }


    //gameover when life=0 and when time=0
    public void GameOver(string reason)
    {
        //Debug.Log("gameover because: "+reason);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0.3f;
        this.gameOverPanel.SetActive(true);
        FindObjectOfType<AudioManager>().muteAll();
        FindObjectOfType<AudioManager>().Play("Music1");

    }

    public void victory()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Time.timeScale = 0.4f;
        this.livesLeft.SetText("Lives left: "+this.currentHealth+".");
        float minutes= Mathf.FloorToInt(this.gameTime / 60);
        float seconds= Mathf.FloorToInt(this.gameTime % 60);
        if (seconds<10)
        {
            this.time.SetText("Total time: " + minutes + ":0" + seconds);
        }
        else { this.time.SetText("Total time: " + minutes + ":" + seconds); }
        
        this.winPanel.SetActive(true);
        this.gameUI.SetActive(false);
        FindObjectOfType<AudioManager>().muteAll();
        FindObjectOfType<AudioManager>().Play("Music1");
        Invoke(nameof(this.freezeGame), 5);
    }

    


    private void OnCollisionEnter(Collision collisionInfo)
    {
        //Health is reduced only when hitting rocks, fallen trees, snowmen
        //Obstacles:
        //4-NPC->reduce speed 50%, no impact on health.
        //2-Small snow mountains->reduce speed 90%, no impact on health.
        //3-Non-snow ground->reduce speed 10% per second, no impact on health.
        //1-Rocks, trees, snowman->reduce speed to 0, -1 health.
        //5-Snowy borders->reduce speed 15% per second, no impact on health.
        //Targets:
        //1-Ramps->gives points.
        //2-Flags, Arches->gives points.
        //Awards:
        //1-avoid obstacle, 10 points.
        //2-Jump over small mountain, 10 points.
        //3-Distance, 5 points per 100m.
        //4-time remaining, 1 point per second
        //5-flags, arches, 5 points.
        //6-ramp,2 points.
        //Debug.Log("collided"+ collisionInfo.collider.tag);
        if (collisionInfo.collider.tag=="Obstacle01" && this.readyCollide)
        {
            this.currentHealth--;
            this.gameUIControllObject.updateHealthSprite(this.currentHealth);
            this.charStatsObj.setHealth(this.currentHealth);
            if (this.charStatsObj.getHealth() <= 0)
            {
                this.GameOver("health");
            }
            //Debug.Log("obstacle");
            this.readyCollide = false;
            FindObjectOfType<AudioManager>().Play("CrashRock");
            Invoke(nameof(this.collisionCoolDownTrigger),this.collisionCoolDown);
            
        }
        if (collisionInfo.collider.tag == "Obstacle02" && this.readyCollide)
        {
            this.currentHealth--;
            this.gameUIControllObject.updateHealthSprite(this.currentHealth);
            this.charStatsObj.setHealth(this.currentHealth);
            if (this.charStatsObj.getHealth() <= 0)
            {
                this.GameOver("health");
            }
            this.readyCollide = false;
            FindObjectOfType<AudioManager>().Play("CrashRock");
            Invoke(nameof(this.collisionCoolDownTrigger), this.collisionCoolDown);
            //delete snowman and create balls, then delete balls after 20 seconds
            GameObject temp = collisionInfo.collider.gameObject;
            Destroy(temp);
            GameObject temp2 = Instantiate(this.snowManPrefab, new Vector3(temp.transform.position.x, temp.transform.position.y + 3, temp.transform.position.z), Quaternion.identity);
            StartCoroutine(MyFunction(temp2, 20f));
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Finish")
        {
            this.victory();
        }
    }

    public void freezeGame()
    {
        Time.timeScale = 0;
    }

    IEnumerator MyFunction(GameObject toDestroy, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(toDestroy);
    }

    public void destroyObject(GameObject toDestroy)
    {
        Destroy(toDestroy);
    }

    private void collisionCoolDownTrigger()
    {
        if (this.readyCollide)
        {
            this.readyCollide = false;
        }
        else
        {
            this.readyCollide = true;
        }
    }

}
