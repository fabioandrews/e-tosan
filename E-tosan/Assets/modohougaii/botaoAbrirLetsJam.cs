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
        ModoHougaii modoHougaii = GameObject.Find("Main Camera").GetComponent<ModoHougaii>();

        GameObject instanciainicioLetsJam = GameObject.Find("inicioLetsJam");
        PopupWindowBehavior instanciainicioLetsJamComTipoReal = instanciainicioLetsJam.GetComponent<PopupWindowBehavior>();
        instanciainicioLetsJamComTipoReal.voltarAPosicaoInicial();

        //fazer a tela atual desaparecer, mas qual eh...?
        if (modoHougaii.getaoFecharLetsJamMostrarQualTela().CompareTo("telaSituacaoHougaii") == 0)
        {
            GameObject telaSituacaoModoHougaii = GameObject.Find("telaSituacaoHougaii");
            TelaSituacaoHougaii telaSituacaoHougaiiComTipoReal = telaSituacaoModoHougaii.GetComponent<TelaSituacaoHougaii>();
            telaSituacaoHougaiiComTipoReal.setUsuarioEstaDentroDeLetsJam(true);

            PopupWindowBehavior telaSituacaoHougaiiComTipoPopupWindow = telaSituacaoModoHougaii.GetComponent<PopupWindowBehavior>();
            telaSituacaoHougaiiComTipoPopupWindow.irParaPosicaoDeDesaparecer();

        }
        else if(modoHougaii.getaoFecharLetsJamMostrarQualTela().CompareTo("telaEscolhaEtosanHougaii") == 0)
        {
            PopupWindowBehavior telaEscolhaEtosanHougaii = GameObject.Find("telaEscolhaEtosanHougaii").GetComponent<PopupWindowBehavior>();
            telaEscolhaEtosanHougaii.irParaPosicaoDeDesaparecer();
            PopupWindowBehavior escolhaEtosanHougaiiTexto = GameObject.Find("escolhaEtosanHougaiiTexto").GetComponent<PopupWindowBehavior>();
            escolhaEtosanHougaiiTexto.irParaPosicaoDeDesaparecer();
            UIBarScript barra_afeicao_melody = GameObject.Find("barra_afeicao_melody").GetComponent<UIBarScript>();
            barra_afeicao_melody.irParaPosicaoDeDesaparecer();
            UIBarScript barra_bondade = GameObject.Find("barra_bondade").GetComponent<UIBarScript>();
            barra_bondade.irParaPosicaoDeDesaparecer();

        }

        

    }
}
