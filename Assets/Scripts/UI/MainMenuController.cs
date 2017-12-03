using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuController : MonoBehaviour {

    public AudioClip hoverSound;
    public AudioClip validateSound;

    private Button[] buttons;

	// Use this for initialization
	void Start () {
        buttons = gameObject.GetComponentsInChildren<Button>(includeInactive: true);
        SoundManager.instance.PlayMusic(SoundManager.instance.titleMusic, playOnLoop: true);
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(delegate { SoundManager.instance.PlaySFX(validateSound); });
            EventTrigger eventTrigger = button.gameObject.AddComponent(typeof(EventTrigger)) as EventTrigger;
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener(delegate { SoundManager.instance.PlaySFX(hoverSound); });
            eventTrigger.triggers.Add(entry);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
