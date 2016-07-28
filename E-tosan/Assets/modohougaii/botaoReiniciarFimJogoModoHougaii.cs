using UnityEngine;
using System.Collections;

public class botaoReiniciarFimJogoModoHougaii : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {
        PopupWindowBehavior telaFimDeJogo = GameObject.Find("telaFimDeJogo").GetComponent<PopupWindowBehavior>();
        telaFimDeJogo.irParaPosicaoDeDesaparecer();
        PopupWindowBehavior telaFimDeJogoTexto = GameObject.Find("telaFimDeJogoTexto").GetComponent<PopupWindowBehavior>();
        telaFimDeJogoTexto.irParaPosicaoDeDesaparecer();
        ModoHougaii modoHougaii = GameObject.Find("Main Camera").GetComponent<ModoHougaii>();
        modoHougaii.reiniciarJogoModoHougaii();
    }
}