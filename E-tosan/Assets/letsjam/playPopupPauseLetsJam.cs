﻿using UnityEngine;
using System.Collections;

public class playPopupPauseLetsJam : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnMouseDown()
    {
        GameObject instanciaPopupPauseLetsJam = GameObject.Find("PopupPauseLetsJam");
        PopupWindowBehavior instanciaPopupPauseLetsJamComTipoReal = instanciaPopupPauseLetsJam.GetComponent<PopupWindowBehavior>();
        instanciaPopupPauseLetsJamComTipoReal.irParaPosicaoDeDesaparecer();

        RadioLetsJam radioLetsJam = GameObject.Find("radioLetsJam").GetComponent<RadioLetsJam>();
        radioLetsJam.unPauseCurrent();
    }
}
