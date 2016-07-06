using UnityEngine;
using System.Collections;

public class botaoInicioLetsJam : MonoBehaviour
{
    public string tipo_de_botao; //pode ser formas_verbais,verbos ou lugares

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
        if (tipo_de_botao == "formas_verbais")
        {
        }
        else if (tipo_de_botao == "verbos")
        {
        }
        else if (tipo_de_botao == "lugares")
        {
        }
        RadioLetsJam radioLetsJam = GameObject.Find("radioLetsJam").GetComponent<RadioLetsJam>();
        radioLetsJam.voltarAPosicaoInicial();
        string[] arquivosDoRadio = new string[] { "hougaii.wav" };

        radioLetsJam.setarArquivosDoRadio(arquivosDoRadio, "Assets/modohougaii/audiosModoHougaii/letsjam");
    }
}