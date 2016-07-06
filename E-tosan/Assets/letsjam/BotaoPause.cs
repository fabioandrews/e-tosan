using UnityEngine;
using System.Collections;

public class BotaoPause : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    }

    void OnMouseDown()
    {
        GameObject instanciaPopupPauseLetsJam = GameObject.Find("PopupPauseLetsJam");
        PopupPauseLetsJam instanciaPopupPauseLetsJamComTipoReal = instanciaPopupPauseLetsJam.GetComponent<PopupPauseLetsJam>(); ;
        instanciaPopupPauseLetsJamComTipoReal.voltarAPosicaoInicial();

        RadioLetsJam radioLetsJam = GameObject.Find("radioLetsJam").GetComponent<RadioLetsJam>();
        radioLetsJam.pauseCurrent();
    }
}
