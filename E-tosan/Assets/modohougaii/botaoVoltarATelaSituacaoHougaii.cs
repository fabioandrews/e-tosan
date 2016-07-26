using UnityEngine;
using System.Collections;

public class botaoVoltarATelaSituacaoHougaii : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        //Fazer a tela da escolha do etosan desaparecer
        GameObject telaEscolhaEtosanHougaii = GameObject.Find("telaEscolhaEtosanHougaii");
        PopupWindowBehavior telaEscolhaEtosanHougaiiTipoReal = telaEscolhaEtosanHougaii.GetComponent<PopupWindowBehavior>();
        telaEscolhaEtosanHougaiiTipoReal.irParaPosicaoDeDesaparecer();
        //e os seus textos...
        GameObject escolhaEtosanHougaiiTexto = GameObject.Find("escolhaEtosanHougaiiTexto");
        PopupWindowBehavior escolhaEtosanHougaiiTextoTipoReal = escolhaEtosanHougaiiTexto.GetComponent<PopupWindowBehavior>();
        escolhaEtosanHougaiiTextoTipoReal.irParaPosicaoDeDesaparecer();
        //e as barras de afeicao e bondade
        UIBarScript barra_afeicao_melody = GameObject.Find("barra_afeicao_melody").GetComponent<UIBarScript>();
        barra_afeicao_melody.irParaPosicaoDeDesaparecer();
        UIBarScript barra_bondade = GameObject.Find("barra_bondade").GetComponent<UIBarScript>();
        barra_bondade.irParaPosicaoDeDesaparecer();

        //fazer a tela da situacao atual aparecer
        GameObject telaSituacaoHougaii = GameObject.Find("telaSituacaoHougaii");
        TelaSituacaoHougaii telaSituacaoHougaiiComTipoTelaSituacao = telaSituacaoHougaii.GetComponent<TelaSituacaoHougaii>();
        telaSituacaoHougaiiComTipoTelaSituacao.pararTodosOsAudios();
        //telaSituacaoHougaiiComTipoTelaSituacao.recomecarAudioSituacaoAtual();
        telaSituacaoHougaiiComTipoTelaSituacao.setUsuarioEstaDentroDeLetsJam(false);
        PopupWindowBehavior telaSituacaoHougaiiComTipoReal = telaSituacaoHougaii.GetComponent<PopupWindowBehavior>();
        telaSituacaoHougaiiComTipoReal.voltarAPosicaoInicial();

        //fazer o ModoHougaii decidir que a tela que deverah ser aberta apos ser fechado um letsjam, por enquanto, eh a telaSituacaoHougaii
        GameObject modoHougaii = GameObject.Find("Main Camera");
        ModoHougaii modoHougaiiTipoReal = modoHougaii.GetComponent<ModoHougaii>();
        modoHougaiiTipoReal.setaoFecharLetsJamMostrarQualTela("telaSituacaoHougaii");

    }
}
