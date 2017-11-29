using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SkillsHUDController : MonoBehaviour {

    public List<Sprite> weakSprites;
    public List<Sprite> strongSprites;
    public List<Sprite> skillSprites;
    public List<Sprite> trinketSprites;


    private Image weakImageSpot;
    private Image strongImageSpot;
    private Image skillImageSpot;
    private Image trinketImageSpot;


    public List<Sprite> spearSprites = new List<Sprite>();
    public List<Sprite> swordSprites = new List<Sprite>();
    public List<Sprite> daggersSprites = new List<Sprite>();
    public List<Sprite> bowSprites = new List<Sprite>();

    protected GameManager gameManagerInstance = GameManager.gameManager;
    protected PlayerController playerController;

    private string oldWeaponName = null;
    private WeaponController weapon;
    private TrinketController trinket;


	// Use this for initialization
	protected virtual void Awake () {
        weakImageSpot = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        strongImageSpot = gameObject.transform.GetChild(1).gameObject.GetComponent<Image>();
        skillImageSpot = gameObject.transform.GetChild(2).gameObject.GetComponent<Image>();
        trinketImageSpot = gameObject.transform.GetChild(3).gameObject.GetComponent<Image>();
       
    }

    // Update is called once per frame
    protected virtual void Update () {
        weapon = playerController.GetMyWeapon();
        
        if (weapon != null)
        {
            
            if (oldWeaponName == null  || oldWeaponName != weapon.weaponName)
            {
                
                UpdateWeaponSprites();
                oldWeaponName = weapon.weaponName;
            }
        }
        UpdateTrinketSprite();
		
	}

    void UpdateWeaponSprites()
    {
        switch (weapon.weaponName)
        {
            case "Sword":
                DisplayWeaponSprites(swordSprites);
                break;
            case "Spear":
                DisplayWeaponSprites(spearSprites);
                break;
            case "Daggers":
                DisplayWeaponSprites(daggersSprites);
                break;
            case "Bow":
                DisplayWeaponSprites(bowSprites);
                break;

        }
    }

    void UpdateTrinketSprite()
    {
        DisplayTrinketSprite();
    }

    protected virtual void DisplayTrinketSprite()
    {
        trinketImageSpot.sprite = trinketSprites[0];
    }

    private void DisplayWeaponSprites(List<Sprite> sprites)
    {
        
        if(sprites.Count == 3)
        {
            weakImageSpot.sprite = sprites[0];
            strongImageSpot.sprite = sprites[1];
            skillImageSpot.sprite = sprites[2];
        }
    }

    protected abstract void MatchSprites();



}
