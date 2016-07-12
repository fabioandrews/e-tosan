using UnityEngine;
using System.Collections;

public class BotaoEject : MonoBehaviour {

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
        PopupWindowBehavior radioLetsJamComoPopupWindow = GameObject.Find("radioLetsJam").GetComponent<PopupWindowBehavior>();
        radioLetsJamComoPopupWindow.irParaPosicaoDeDesaparecer();
        radioLetsJam.stopCurrent();

        PopupWindowBehavior nomeMusicaAtualRadioLetsJam = GameObject.Find("nomeMusicaAtualRadioLetsJam").GetComponent<PopupWindowBehavior>();
        nomeMusicaAtualRadioLetsJam.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior traducaoMusicaAtualRadioLetsJam = GameObject.Find("traducaoMusicaAtualRadioLetsJam").GetComponent<PopupWindowBehavior>();
        traducaoMusicaAtualRadioLetsJam.irParaPosicaoDeDesaparecer();

    }
}
