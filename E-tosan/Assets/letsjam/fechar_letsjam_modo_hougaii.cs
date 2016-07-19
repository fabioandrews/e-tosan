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
        GameObject modoHougaii = GameObject.Find("Main Camera");
        ModoHougaii modoHougaiiTipoReal = modoHougaii.GetComponent<ModoHougaii>();
        string qualTelaDeveAparecer = modoHougaiiTipoReal.getaoFecharLetsJamMostrarQualTela();
        if (qualTelaDeveAparecer.CompareTo("telaSituacaoHougaii") == 0)
        {
            GameObject telaSituacaoModoHougaii = GameObject.Find("telaSituacaoHougaii");
            TelaSituacaoHougaii telaSituacaoHougaiiComTipoReal = telaSituacaoModoHougaii.GetComponent<TelaSituacaoHougaii>();
            telaSituacaoHougaiiComTipoReal.setUsuarioEstaDentroDeLetsJam(false);

            //FALTOU FAZER A TELA DA SITUACAO REAPARECER
            PopupWindowBehavior telaSituacaoHougaiiComTipoJanelaPopup = telaSituacaoModoHougaii.GetComponent<PopupWindowBehavior>();
            telaSituacaoHougaiiComTipoJanelaPopup.voltarAPosicaoInicial();
        }
        else if (qualTelaDeveAparecer.CompareTo("telaEscolhaEtosanHougaii") == 0)
        {
            PopupWindowBehavior telaEscolhaEtosanHougaii = GameObject.Find("telaEscolhaEtosanHougaii").GetComponent<PopupWindowBehavior>();
            telaEscolhaEtosanHougaii.voltarAPosicaoInicial();
            //essa eh uma tela que vem acompanhada de uma tela de texto. por isso as duas devem aparecer juntas
            PopupWindowBehavior escolhaEtosanHougaiiTexto = GameObject.Find("escolhaEtosanHougaiiTexto").GetComponent<PopupWindowBehavior>();
            escolhaEtosanHougaiiTexto.voltarAPosicaoInicial();
            //e as barras de afeicao e bondade
            GUIBarScript barra_afeicao_melody = GameObject.Find("barra_afeicao_melody").GetComponent<GUIBarScript>();
            barra_afeicao_melody.voltarAPosicaoInicial();
            GUIBarScript barra_bondade = GameObject.Find("barra_bondade").GetComponent<GUIBarScript>();
            barra_bondade.voltarAPosicaoInicial();
        }
    }
}
