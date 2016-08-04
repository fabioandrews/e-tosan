using UnityEngine;
using System.Collections;

public class moedaBarra : MonoBehaviour
{
    public Sprite moedaApagada;
    public Sprite moedaAcesa;
    public float percentual; //o percentual da barrinha(bondade ou afeicao) associado a essa moeda. Pode ser de 0.00 a 1.00
    private Vector3 posicaoInicial;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    //funcao chamada la no ModoHougaii, pois eh la onde todas as popup windows obtem sua posicao inicial. Moeda pode n ser popup, mas parece uma 
    public void obterPosicaoInicial()
    {
        posicaoInicial = transform.localPosition;
    }

    //funcao chamada na telaEscolhaEtosan, visto que antes da moeda ser criada em alguma posicao, ela deve voltar a sua posicao inicial
    public void voltarAPosicaoInicial()
    {
        transform.localPosition = posicaoInicial;
        /*foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().enabled = true;
        }*/
    }

    public void mudarSpriteAtual(string apagadaOuAcesa)
    {
        if (apagadaOuAcesa.CompareTo("apagada") == 0)
        {
            this.GetComponent<SpriteRenderer>().sprite = moedaApagada;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = moedaAcesa;
        }
    }

    public void setPercentual(float novoValor)
    {
        this.percentual = novoValor;
    }
}
