using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class botaoTelaEscolhaEtosan : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void mudarTextoRelacionado(string novoTexto, string nomeDesseBotao)
    {

        if (nomeDesseBotao.CompareTo("botaoTelaEscolhaEtosan1") == 0)
        {
            Text textRelacionadoAEsseBotaoTelaEscolhaETosan = GameObject.Find("textoBotaoTelaEscolhaEtosan1").GetComponent<Text>();
            textRelacionadoAEsseBotaoTelaEscolhaETosan.text = novoTexto;
        }
        else
        {
            Text textRelacionadoAEsseBotaoTelaEscolhaETosan = GameObject.Find("textoBotaoTelaEscolhaEtosan2").GetComponent<Text>();
            textRelacionadoAEsseBotaoTelaEscolhaETosan.text = novoTexto;
        }
    }

    void OnMouseDown()
    {
        telaEscolhaEtosanHougaii telaEscolhaEtosanHougaii =
                            GameObject.Find("telaEscolhaEtosanHougaii").GetComponent<telaEscolhaEtosanHougaii>();
        string texto = "";
        if (this.gameObject.name.CompareTo("botaoTelaEscolhaEtosan1") == 0)
        {
            Text textObject = GameObject.Find("textoBotaoTelaEscolhaEtosan1").GetComponent<Text>();
            texto = textObject.text;
        }
        else if (this.gameObject.name.CompareTo("botaoTelaEscolhaEtosan2") == 0)
        {
            Text textObject = GameObject.Find("textoBotaoTelaEscolhaEtosan2").GetComponent<Text>();
            texto = textObject.text;
        }
        telaEscolhaEtosanHougaii.usuarioEscolheuUmaResposta(texto);
    }
}