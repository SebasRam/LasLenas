using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterStats : MonoBehaviour
{
    //this variable is used to measure the character's health. It will determine if the game is over.
    [Header("Health variables")]
    public int health;

    //this variable is used to measure the character's speed.
    [Header("Speed variables")]
    public float speed;
    public TextMeshProUGUI speedUI;

    //this variable is used to measure the distance traveled by the character.
    [Header("Distance variables")]
    public float distance;
    public TextMeshProUGUI distanceUI;

    //these variables are used to determine how much time the player has left.
    public int maxTime;
    public int remainingTime;

    //These variables are used to controll the character's animations
    [Header("CharacterMesh")]
    public GameObject charMesh;
    private Animator charAnimator;

    

    // Start is called before the first frame update
    void Start()
    {
        this.health = 5;
        this.speed = 0;
        this.distance = 0;
        this.charAnimator = this.charMesh.GetComponent<Animator>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        this.speedUI.SetText(""+(int)this.speed);
        this.distanceUI.SetText(""+(int)this.distance);

        float speedPercentage = this.speed / 60;
        //FindObjectOfType<AudioManager>().tuneVolume("Skiing",0.8f*speedPercentage);


        if (this.speed<=2)
        {
            FindObjectOfType<AudioManager>().Mute("Skiing");
            this.charAnimator.SetBool("IsMoving", false);
        }
        if (this.speed>2)
        {
            FindObjectOfType<AudioManager>().UnMute("Skiing");
            this.charAnimator.SetBool("IsMoving",true);
        }
    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }
    public float getSpeed()
    {
        return this.speed;
    }

    public void decreaseHealth(int amount)
    {
        this.health -= amount;
    }
    public void increaseHealth(int amount)
    {
        this.health += amount;
    }

    public void setDistance(int dist)
    {
        this.distance = dist;
    }
    public void increaseDistance(float dist)
    {
        this.distance += dist;
    }

    public int getHealth()
    {
        return this.health;
    }
    public void setHealth(int h)
    {
        this.health = h;
    }
    public void resetAll()
    {
        this.health = 3;
        this.distance = 0;

    }





}
