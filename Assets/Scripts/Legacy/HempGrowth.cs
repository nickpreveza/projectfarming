using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HempGrowth : MonoBehaviour
{
   public bool hasGrown = false;
    GameObject Player;
    private InputManager inputManager;
    Collider2D playerCollider;

    bool canWater = false;
    bool isBeingWatered = false;

    public Sprite Stage1;
    public Sprite Stage2;
    public Sprite Stage3;
    public Sprite Stage4;
    public Sprite Stage5;

    private float health = 5f;
    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        inputManager = Player.GetComponent<InputManager>();
        playerCollider = Player.GetComponent<Collider2D>();
    }
    private void Update()
    {

        CheckIfBeingWatered();
    }
    void CheckIfBeingWatered()
    {
      
        if(Player.GetComponent<ShootingManager>().waterGunOn && inputManager.GetWateringInput())
        {
            isBeingWatered = true;
        }
        
    }
  
    public float Health
    {
        set
        {
            health = value;
            if (health <= 0f)
            {
                Killed();
            }
        }
        get
        {
            return health;
        }
    }

    private void Killed()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            if (isBeingWatered == true)
            {
                if (hasGrown == false)
                {
                    this.GetComponent<SpriteRenderer>().sprite = Stage2;
                    this.gameObject.transform.position += new Vector3(0, 0.5f, 0);
                    hasGrown = true;
                }
            }
        }
        
    }


}
