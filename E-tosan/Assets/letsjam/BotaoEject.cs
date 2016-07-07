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
        radioLetsJam.irParaPosicaoDeDesaparecer();
        radioLetsJam.stopCurrent();

        nomeMusicaAtualRadioLetsJam nomeMusicaAtualRadioLetsJam = GameObject.Find("nomeMusicaAtualRadioLetsJam").GetComponent<nomeMusicaAtualRadioLetsJam>();
        nomeMusicaAtualRadioLetsJam.irParaPosicaoDeDesaparecer();

        traducaoMusicaAtualLetsJam traducaoMusicaAtualRadioLetsJam = GameObject.Find("traducaoMusicaAtualRadioLetsJam").GetComponent<traducaoMusicaAtualLetsJam>();
        traducaoMusicaAtualRadioLetsJam.irParaPosicaoDeDesaparecer();

    }
}
