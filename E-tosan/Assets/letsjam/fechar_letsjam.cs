using UnityEngine;
using System.Collections;

public class fechar_letsjam : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        GameObject instanciainicioLetsJam = GameObject.Find("inicioLetsJam");
        PopupWindowBehavior instanciainicioLetsJamComTipoReal = instanciainicioLetsJam.GetComponent<PopupWindowBehavior>(); ;
        instanciainicioLetsJamComTipoReal.irParaPosicaoDeDesaparecer();

    }
}
