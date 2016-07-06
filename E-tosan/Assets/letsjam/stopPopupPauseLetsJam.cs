using UnityEngine;
using System.Collections;

public class stopPopupPauseLetsJam : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        GameObject instanciaPopupTemCertezaQueQuerIrAoMainMenu = GameObject.Find("PopupTemCertezaQueQuerIrAoMainMenu");
        PopupTemCertezaQueQuerIrAoMainMenu instanciaPopupTemCertezaQueQuerIrAoMainMenuComTipoReal = instanciaPopupTemCertezaQueQuerIrAoMainMenu.GetComponent<PopupTemCertezaQueQuerIrAoMainMenu>(); ;
        instanciaPopupTemCertezaQueQuerIrAoMainMenuComTipoReal.voltarAPosicaoInicial();
    }
}
