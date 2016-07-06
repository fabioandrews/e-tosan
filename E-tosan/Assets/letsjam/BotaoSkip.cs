using UnityEngine;
using System.Collections;

public class BotaoSkip : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown()
    {
        RadioLetsJam radioLetsJam = GameObject.Find("radioLetsJam").GetComponent<RadioLetsJam>();
        radioLetsJam.Seek(RadioLetsJam.SeekDirection.Forward);
        radioLetsJam.PlayCurrent();
    }

}
