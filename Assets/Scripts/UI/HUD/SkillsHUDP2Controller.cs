using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsHUDP2Controller : SkillsHUDController {
    // Use this for initialization
    protected override void Awake()
    {
        playerController = gameManagerInstance.player2.GetComponent<PlayerController>();
        MatchSprites();
        base.Awake();


    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }


    protected override void MatchSprites()
    {
        foreach (Sprite sprite in weakSprites)
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
                case "spearskilldead":
                    spearSprites.Add(sprite);
                    break;
                case "swordskilldead":
                    swordSprites.Add(sprite);
                    break;
                case "daggerskilldead":
                    daggersSprites.Add(sprite);
                    break;
                case "bowskilldead":
                    bowSprites.Add(sprite);
                    break;
            }
        }
    }
}
