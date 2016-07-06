using UnityEngine;
using System.Collections;

public class botao_nao_voltar_menu_inicial : MonoBehaviour {

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
        instanciaPopupTemCertezaQueQuerIrAoMainMenuComTipoReal.irParaPosicaoDeDesaparecer();
    }
}
