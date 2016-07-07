using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Quem define os nomes dos arquivos de audio do lets jam usados eh o botaoInicioLetsJam, eu seu metodo pegarNomesArquivosDeAudioParaRadio
//os nomes destes arquivos sao o que eles dizem + .wav, tipo ～のがすきです.wav ou さんぽする.wav

//Quem define os nomes dos arquivos das situacoes eh FornecedorDeSituacoes e o construtor de SituacaoModoHougaii
//Cada situacao tem dois arquivos cujo nome eh: "id dela(id eh fornecido pelo FornecedorDeSituacoes)" + ".wav" (essa extensao eh adiciona no construtor de SituacaoModoHougaii)



public class ModoHougaii : MonoBehaviour
{
    private FornecedorDeSituacoes fornecedorSituacoes; //classe que olha o BD e pega situacoes para o jogo
    private LinkedList<SituacaoModoHougaii> quatroSituacoesAtuais; //usuario treina de 4 em 4 situacoes
    private bool melodyEstahZangadaAtualmente;

    // Use this for initialization
    void Start ()
    {
        this.fornecedorSituacoes = new FornecedorDeSituacoes();
        melodyEstahZangadaAtualmente = false;
        obterQuatroSituacoesAtuais();
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    private void obterQuatroSituacoesAtuais()
    {
        this.quatroSituacoesAtuais = fornecedorSituacoes.fornecer4Situacoes(melodyEstahZangadaAtualmente);
    }

    public LinkedList<SituacaoModoHougaii> getquatroSituacoesAtuais()
    {
        return this.quatroSituacoesAtuais;
    }

}
