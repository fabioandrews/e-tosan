using UnityEngine;
using System.Collections;

public class fechar_letsjam_modo_hougaii : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        GameObject telaSituacaoModoHougaii = GameObject.Find("telaSituacaoHougaii");
        TelaSituacaoHougaii telaSituacaoHougaiiComTipoReal = telaSituacaoModoHougaii.GetComponent<TelaSituacaoHougaii>();
        telaSituacaoHougaiiComTipoReal.setUsuarioEstaDentroDeLetsJam(false);

        //FALTOU FAZER A TELA DA SITUACAO REAPARECER
        PopupWindowBehavior telaSituacaoHougaiiComTipoJanelaPopup = telaSituacaoModoHougaii.GetComponent<PopupWindowBehavior>();
        telaSituacaoHougaiiComTipoJanelaPopup.voltarAPosicaoInicial();
    }
}
