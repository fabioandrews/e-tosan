using UnityEngine;
using System.Collections;

public class botaoAbrirLetsJam : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        GameObject instanciainicioLetsJam = GameObject.Find("inicioLetsJam");
        PopupWindowBehavior instanciainicioLetsJamComTipoReal = instanciainicioLetsJam.GetComponent<PopupWindowBehavior>();
        instanciainicioLetsJamComTipoReal.voltarAPosicaoInicial();

        GameObject telaSituacaoModoHougaii = GameObject.Find("telaSituacaoHougaii");
        TelaSituacaoHougaii telaSituacaoHougaiiComTipoReal = telaSituacaoModoHougaii.GetComponent<TelaSituacaoHougaii>(); ;
        telaSituacaoHougaiiComTipoReal.setUsuarioEstaDentroDeLetsJam(true);


    }
}
