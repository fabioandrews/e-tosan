using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        ModoHougaii modoHougaii = GameObject.Find("Main Camera").GetComponent<ModoHougaii>();
        LinkedList<SituacaoModoHougaii> quatroSituacoesAtuais = modoHougaii.getquatroSituacoesAtuais();
        SituacaoModoHougaii situacaoAtual = modoHougaii.getsituacaoAtual(); //a situacao atual n faz parte das quatro situacoes atuais(ela eh removida da linkedlist assim que eh usada), mas deveria

        string[] nomesArquivosParaRadio;
        if (quatroSituacoesAtuais.Count < 4)
        {
            //certeza que a situacao atual foi deixada de lado. Vamos inseri-la! Agora, nao podemos alterar a linkedlist. Vamos criar uma nova
            //se nao criarmos uam nova e fizermos addlast, a lista de origem tb serah aumentada
            LinkedList<SituacaoModoHougaii> quatroSituacoesAtuaisNovinha = new LinkedList<SituacaoModoHougaii>();
            LinkedListNode<SituacaoModoHougaii> percorredorLista = quatroSituacoesAtuais.First;
            while (percorredorLista != null && percorredorLista.Value != null)
            {
                quatroSituacoesAtuaisNovinha.AddLast(percorredorLista.Value);
                percorredorLista = percorredorLista.Next;
            }

            quatroSituacoesAtuaisNovinha.AddLast(situacaoAtual);
            nomesArquivosParaRadio = pegarNomesArquivosDeAudioParaRadio(this.tipo_de_botao, quatroSituacoesAtuaisNovinha);

        }
        else
        {
            nomesArquivosParaRadio = pegarNomesArquivosDeAudioParaRadio(this.tipo_de_botao, quatroSituacoesAtuais);
        }

        RadioLetsJam radioLetsJam = GameObject.Find("radioLetsJam").GetComponent<RadioLetsJam>();
        PopupWindowBehavior radioLetsJamComoPopupWindow = GameObject.Find("radioLetsJam").GetComponent<PopupWindowBehavior>();
        radioLetsJamComoPopupWindow.voltarAPosicaoInicial();

        string ondeEstaoOsArquivosDeAudioDoLetsJam = modoHougaii.getondeEstaoOsArquivosDeAudioDoLetsJam();
        radioLetsJam.setarArquivosDoRadio(nomesArquivosParaRadio, ondeEstaoOsArquivosDeAudioDoLetsJam);

        radioLetsJam.PlayCurrent(); //para playar a musica 1 dessa lista de musicas

        PopupWindowBehavior nomeMusicaAtualRadioLetsJam = GameObject.Find("nomeMusicaAtualRadioLetsJam").GetComponent<PopupWindowBehavior>();
        nomeMusicaAtualRadioLetsJam.voltarAPosicaoInicial(); //faz o texto coma musica atual aparecer tb!

        PopupWindowBehavior traducaoMusicaAtualRadioLetsJam = GameObject.Find("traducaoMusicaAtualRadioLetsJam").GetComponent<PopupWindowBehavior>();
        traducaoMusicaAtualRadioLetsJam.voltarAPosicaoInicial(); //faz o texto com a traducao da musica atual aparecer tb!
    }

    //vou extrair todos os arquivos de audio necessarios dentro das quatro situacoes atuais
    //e apenas para exercitar verbos, lugares ou formas verbais
    //esse atributo formas_verbaisVerbosOuLugares pode ser "formas_verbais", "verbos" ou "lugares"
    //tudo bem se uma situacao nao tiver tempos verbais ou lugares...eu checo isso antes de incluir na lista
    private string[] pegarNomesArquivosDeAudioParaRadio(string formas_verbaisVerbosOuLugares, LinkedList<SituacaoModoHougaii> quatroSituacoesAtuais)
    {
        LinkedList<string> arquivosDeAudioLinkedList = new LinkedList<string>();
        LinkedListNode<SituacaoModoHougaii> percorredorLista = quatroSituacoesAtuais.First;
        while (percorredorLista != null && percorredorLista.Value != null)
        {
            SituacaoModoHougaii umaSituacao = percorredorLista.Value;

            if (formas_verbaisVerbosOuLugares.CompareTo("formas_verbais") == 0)
            {
                LinkedList<string> temposVerbaisEstudados = umaSituacao.gettemposVerbaisEstudados();
                if (temposVerbaisEstudados != null && temposVerbaisEstudados.Count > 0)
                {
                    //existem tempos verbais nessa situacao
                    LinkedListNode<string> percorredorTemposVerbais = temposVerbaisEstudados.First;
                    while (percorredorTemposVerbais != null && percorredorTemposVerbais.Value != null)
                    {
                        arquivosDeAudioLinkedList.AddLast(percorredorTemposVerbais.Value + ".wav");
                        percorredorTemposVerbais = percorredorTemposVerbais.Next;
                    }
                }
            }
            else if (formas_verbaisVerbosOuLugares.CompareTo("verbos") == 0)
            {
                string verbo = umaSituacao.getverbo();
                if (verbo != null && verbo.Length > 0)
                {
                    arquivosDeAudioLinkedList.AddLast(verbo + ".wav");
                }
            }
            else if (formas_verbaisVerbosOuLugares.CompareTo("lugares") == 0)
            {
                string lugar = umaSituacao.getlugar();
                if (lugar != null && lugar.Length > 0)
                {
                    arquivosDeAudioLinkedList.AddLast(lugar + ".wav");
                }
            }

            percorredorLista = percorredorLista.Next;
        }

        //vamos remover elementos repetidos da lista
        List<string> arquivosDeAudioLinkedListSemRepeticoesList = arquivosDeAudioLinkedList.Distinct().ToList();
        LinkedList<string> arquivosDeAudioLinkedListSemRepeticoes = new LinkedList<string>(arquivosDeAudioLinkedListSemRepeticoesList);


        //agora so falta transformar linkedlist em array
        string[] arquivosDeAudio = new string[arquivosDeAudioLinkedListSemRepeticoes.Count];
        int percorredorArquivosDeAudio = 0;
        LinkedListNode<string> percorredorarquivosDeAudioLinkedListSemRepeticoes = arquivosDeAudioLinkedListSemRepeticoes.First;
        while (percorredorarquivosDeAudioLinkedListSemRepeticoes != null && percorredorarquivosDeAudioLinkedListSemRepeticoes.Value != null)
        {
            arquivosDeAudio[percorredorArquivosDeAudio] = percorredorarquivosDeAudioLinkedListSemRepeticoes.Value;
            percorredorarquivosDeAudioLinkedListSemRepeticoes = percorredorarquivosDeAudioLinkedListSemRepeticoes.Next;
            percorredorArquivosDeAudio = percorredorArquivosDeAudio + 1;
        }

        return arquivosDeAudio;
    }
}