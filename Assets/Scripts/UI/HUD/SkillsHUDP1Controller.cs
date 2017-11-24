using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsHUDP1Controller : SkillsHUDController {
    
    // Use this for initialization
    protected override void Awake () {
        playerController = gameManagerInstance.player1.GetComponent<PlayerController>();
        MatchSprites();
        base.Awake();
        

	}
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
	}


    protected override void MatchSprites()
    {
        foreach(Sprite sprite in weakSprites)
        {
            switch (sprite.name.ToLower())
            {
                case "spearweak":
                    spearSprites.Add(sprite);
                    break;
                case "swordweak":
                    swordSprites.Add(sprite);
                    break;
                case "daggerweak":
                    

                    daggersSprites.Add(sprite);
                    break;
                case "bowweak":
                    

                    bowSprites.Add(sprite);
                    break;
            }
        }
        foreach (Sprite sprite in strongSprites)
        {
            switch (sprite.name.ToLower())
            {
                case "spearstrong":
                    spearSprites.Add(sprite);
                    break;
                case "swordstrong":
                    swordSprites.Add(sprite);
                    break;
                case "daggerstrong":
                    daggersSprites.Add(sprite);
                    break;
                case "bowstrong":
                    bowSprites.Add(sprite);
                    break;
            }
        }
        foreach (Sprite sprite in skillSprites)
        {
            switch (sprite.name.ToLower())
            {
                case "spearskillliving":
                    spearSprites.Add(sprite);
                    break;
                case "swordskillliving":
                    swordSprites.Add(sprite);
                    break;
                case "daggerskillliving":
                    daggersSprites.Add(sprite);
                    break;
                case "bowskillliving":
                    bowSprites.Add(sprite);
                    break;
            }
        }
    }
}
