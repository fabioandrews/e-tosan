using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//essa classe vai se comunicar bom o BD e vai fornecer situacoes para a classe ModoHougaii
public class FornecedorDeSituacoes
{
    private LinkedList<SituacaoModoHougaii> possiveisSituacoesTotal; //linkedlist estatica que permanece com todas as possiveis linkedlists do BD
    private LinkedList<SituacaoModoHougaii> possiveisSituacoesModificado; //linkedlist que vai sendo reduzida a medida que novas situacoes vao sendo pedidas
    public FornecedorDeSituacoes()
    {
        possiveisSituacoesTotal = new LinkedList<SituacaoModoHougaii>();
        possiveisSituacoesModificado = new LinkedList<SituacaoModoHougaii>();
        this.obterPossiveisSituacoes();
    }

    //se a melody estah zangada atualmente, a funcao retorna 2 situacoes favoraveis para a melody pelo menos
    //Atualmente sao 2, mas pode mudar para 4 mais tarde 
    public LinkedList<SituacaoModoHougaii> fornecer4Situacoes(bool melodyEstahZangadaAtualmente)
    {
        LinkedList<SituacaoModoHougaii> resposta = new LinkedList<SituacaoModoHougaii>();
        if (melodyEstahZangadaAtualmente == true)
        {
            //melhor pegar pelo menos 2 situacoes que agradem a melody! Mas existem? Vou checar apenas o 
            //possiveisSituacoesModificado. Se nao tiver, tudo bem... ela se zanga mais um pouquinho
            //(senao poderia entrar numa situacao onde eu nunca achava situacao que agradasse a melody)
            while (resposta.Count != 2)
            {
                SituacaoModoHougaii umaSituacao = obterUmaNovaSituacao(resposta, melodyEstahZangadaAtualmente);
                resposta.AddLast(umaSituacao);
            }
        }

        while (resposta.Count != 4)
        {
            //a melody pode ate estar ainda zangada, mas isso nao importa mais. Vou obter situacoes independente da vontade dela!
            SituacaoModoHougaii umaSituacao = obterUmaNovaSituacao(resposta,false);
            resposta.AddLast(umaSituacao);
        }

        return resposta;
    }

    private bool existeSituacaoEmpossiveisSituacoesModificadoQueAgradeAMelody()
    {
        bool existeSituacaoEmpossiveisSituacoesModificadoQueAgradeAMelody = false;
        LinkedListNode<SituacaoModoHougaii> percorredorLista = possiveisSituacoesModificado.First;
        while (percorredorLista != null && percorredorLista.Value != null)
        {
            if (percorredorLista.Value.getrespostaAgradaMelody() == true)
            {
                existeSituacaoEmpossiveisSituacoesModificadoQueAgradeAMelody = true;
                break;
            }

            percorredorLista = percorredorLista.Next;
        }

        return existeSituacaoEmpossiveisSituacoesModificadoQueAgradeAMelody;
    }

    //nova situacao: aquela que ainda nao estah em situacoes atuais. Normalmente eh soh pegar de possiveisSituacoesModificado, pois ele vai se reduzindo
    //porem, pode ser que o size de possiveisSituacoesModificado chegue a zero. nesse caso, ele tem de se resetar e temos de ir vendo se o que pegamos ja esta na lista passada como parametro ou nao
    //se amelody estah zangada, melhor eu pegar uma situacao que respostaCorretaAgradaMelody = true, mas so se possiveisSituacoesModificado ainda tem pelo menos 1 situacao. Se n tiver, tudo bem deixar a melody zangada 
    private SituacaoModoHougaii obterUmaNovaSituacao(LinkedList<SituacaoModoHougaii> situacoesAtuais, bool melodyEstahZangadaAtualmente)
    {
        if (this.possiveisSituacoesModificado.Count > 0)
        {
            SituacaoModoHougaii situacaoVouRemover = null;
            while (situacaoVouRemover == null)
            {
                System.Random rnd = new System.Random();
                int posicaoSituacaoVouRemover = rnd.Next(0, this.possiveisSituacoesModificado.Count); // creates a number between 1 and 12
                                                                                                      //em c# nao existe get em linkedlist. Logo, teremos de fazer isso:
                LinkedListNode<SituacaoModoHougaii> percorredorLista = possiveisSituacoesModificado.First;
                for (int i = 0; i <= posicaoSituacaoVouRemover; i++)
                {
                    if (i == posicaoSituacaoVouRemover)
                    {
                        situacaoVouRemover = percorredorLista.Value;

                    }
                    else
                    {
                        percorredorLista = percorredorLista.Next;
                    }
                }

                if (melodyEstahZangadaAtualmente == true && existeSituacaoEmpossiveisSituacoesModificadoQueAgradeAMelody() == true)
                {
                    //temos de checar se a situacaoVouRemover favorece a melody ou nao
                    if (situacaoVouRemover.getrespostaAgradaMelody() == false)
                    {
                        //nao posso usar esta situacao! Iria ferir os sentimentos da melody, tadinha!
                        situacaoVouRemover = null;
                    }
                }
            }

            possiveisSituacoesModificado.Remove(situacaoVouRemover);

            return situacaoVouRemover;

        }
        else
        {
            //fiquei sem situacoes em possiveisSituacoesModificado. Vou reinicia-lo!
            this.possiveisSituacoesModificado = this.possiveisSituacoesTotal;

            //agora eu tenho de pegar alguma situacao nova que nao exista ainda na linkedlist passada como entrada
            SituacaoModoHougaii situacaoVouRemover = null;
            while (situacaoVouRemover == null)
            {
                System.Random rnd = new System.Random();
                int posicaoSituacaoVouRemover = rnd.Next(0, this.possiveisSituacoesModificado.Count); // creates a number between 1 and 12
                                                                                                      //em c# nao existe get em linkedlist. Logo, teremos de fazer isso:
                LinkedListNode<SituacaoModoHougaii> percorredorLista = possiveisSituacoesModificado.First;
                SituacaoModoHougaii possivelSituacaoVouRemover = null;
                for (int i = 0; i <= posicaoSituacaoVouRemover; i++)
                {
                    if (i == posicaoSituacaoVouRemover)
                    {
                        possivelSituacaoVouRemover = percorredorLista.Value;

                    }
                    else
                    {
                        percorredorLista = percorredorLista.Next;
                    }
                }

                //achei um possivelSituacaoVouRemover, mas vou checar se ele esta na lista passada como parametro
                bool possivelSituacaoVouRemoverJaExisteNaListaDeEntrada = false;
                LinkedListNode<SituacaoModoHougaii> percorredorListaEntrada = situacoesAtuais.First;
                while (possivelSituacaoVouRemoverJaExisteNaListaDeEntrada == false && percorredorListaEntrada != null)
                {
                    SituacaoModoHougaii situacaoDaListaDeEntrada = percorredorListaEntrada.Value;
                    if (situacaoDaListaDeEntrada.getid() == possivelSituacaoVouRemover.getid())
                    {
                        //possivelSituacaoVouRemover ja existe na lista de entrada! nao posso usar ele!
                        possivelSituacaoVouRemoverJaExisteNaListaDeEntrada = true;
                    }
                    else
                    {
                        percorredorListaEntrada = percorredorListaEntrada.Next;
                    }
                }

                if (possivelSituacaoVouRemoverJaExisteNaListaDeEntrada == false)
                {
                    //essa situacao pode ser removida! Nesse caso, acho melhor nao se preocupar com a melody. Deu tanto trabalho para achar a situacao...
                    situacaoVouRemover = possivelSituacaoVouRemover;
                }
            }

            //falta remover o situacaoVouRemover
            possiveisSituacoesModificado.Remove(situacaoVouRemover);

            return situacaoVouRemover;
        }
    }

    private void obterPossiveisSituacoes() //essa funcao futuramente vai se comunicar com o BD e vai obter todas as possiveis situacoes que o sistema pode oferecer
    {
        possiveisSituacoesTotal.Clear();
        //entradas do construtor situacao: string id (que sera usado para achar os nomes dos arquivos wav_melody e _regras),LinkedList<string> temposVerbaisEstudados, string verbo, string lugar,
        //LinkedList<string> duasRespostasDoEtosan, string respostaCorretaDoEtosan, bool respostaAgradaMelody

        //～たいです");
        //～てはいけません
        string ids1 = "situacao1";
        LinkedList<string> temposVerbaiss1 = new LinkedList<string>();
        temposVerbaiss1.AddLast("～たいです");
        temposVerbaiss1.AddLast("～てはいけません");
        temposVerbaiss1.AddLast("～ほうがいい");
        temposVerbaiss1.AddLast("～ないほうがいい");
        string verbos1 = "およぐ";
        string lugars1 = "プール";
        LinkedList<string> duasRespostasDoEtosans1 = new LinkedList<string>();
        duasRespostasDoEtosans1.AddLast("あなた　は　およいだほうがいいです");
        duasRespostasDoEtosans1.AddLast("あなた　は　およがないほうがいいです");
        string respostaCorretaDoEtosans1 = "あなた　は　およがないほうがいいです";
        bool respostaAgradaMelodys1 = false;

        SituacaoModoHougaii s1 = new SituacaoModoHougaii(ids1,temposVerbaiss1, verbos1, lugars1, duasRespostasDoEtosans1,
                                    respostaCorretaDoEtosans1, respostaAgradaMelodys1);
        possiveisSituacoesTotal.AddLast(s1);

        string ids2 = "situacao2";
        LinkedList<string> temposVerbaiss2 = new LinkedList<string>();
        temposVerbaiss2.AddLast("～たいです");
        temposVerbaiss2.AddLast("～てはいけません");
        temposVerbaiss2.AddLast("～ほうがいい");
        temposVerbaiss2.AddLast("～ないほうがいい");
        string verbos2 = "あそぶ";
        string lugars2 = "えいがかん";
        LinkedList<string> duasRespostasDoEtosans2 = new LinkedList<string>();
        duasRespostasDoEtosans2.AddLast("えいがかんで　あそんだほうがいいです");
        duasRespostasDoEtosans2.AddLast("えいがかんで　あそばないほうがいいです");
        string respostaCorretaDoEtosans2 = "えいがかんで　あそばないほうがいいです";
        bool respostaAgradaMelodys2 = false;

        SituacaoModoHougaii s2 = new SituacaoModoHougaii(ids2, temposVerbaiss2, verbos2, lugars2, duasRespostasDoEtosans2,
                                    respostaCorretaDoEtosans2, respostaAgradaMelodys2);
        possiveisSituacoesTotal.AddLast(s2);

        string ids3 = "situacao3";
        LinkedList<string> temposVerbaiss3 = new LinkedList<string>();
        temposVerbaiss3.AddLast("～たいです");
        temposVerbaiss3.AddLast("～てはいけません");
        temposVerbaiss3.AddLast("～ほうがいい");
        temposVerbaiss3.AddLast("～ないほうがいい");
        string verbos3 = "うたう";
        string lugars3 = "きょうしつ";
        LinkedList<string> duasRespostasDoEtosans3 = new LinkedList<string>();
        duasRespostasDoEtosans3.AddLast("きょうしつで　うたったほうがいいです");
        duasRespostasDoEtosans3.AddLast("きょうしつで　うたわないほうがいいです");
        string respostaCorretaDoEtosans3 = "きょうしつで　うたわないほうがいいです";
        bool respostaAgradaMelodys3 = false;

        SituacaoModoHougaii s3 = new SituacaoModoHougaii(ids3, temposVerbaiss3, verbos3, lugars3, duasRespostasDoEtosans3,
                                    respostaCorretaDoEtosans3, respostaAgradaMelodys3);
        possiveisSituacoesTotal.AddLast(s3);


        //のがすきです
        //～たいです
        //～てもいいです
        string ids4 = "situacao4";
        LinkedList<string> temposVerbaiss4 = new LinkedList<string>();
        temposVerbaiss4.AddLast("～のがすきです");
        temposVerbaiss4.AddLast("～たいです");
        temposVerbaiss4.AddLast("～てもいいです");
        temposVerbaiss4.AddLast("～ほうがいい");
        temposVerbaiss4.AddLast("～ないほうがいい");
        string verbos4 = "きく";
        string lugars4 = "いえ";
        LinkedList<string> duasRespostasDoEtosans4 = new LinkedList<string>();
        duasRespostasDoEtosans4.AddLast("いえで　おんがくを　きいたほうがいいです");
        duasRespostasDoEtosans4.AddLast("いえで　おんがくを　きかないほうがいいです");
        string respostaCorretaDoEtosans4 = "いえで　おんがくを　きいたほうがいいです";
        bool respostaAgradaMelodys4 = true;

        SituacaoModoHougaii s4 = new SituacaoModoHougaii(ids4, temposVerbaiss4, verbos4, lugars4, duasRespostasDoEtosans4,
                                    respostaCorretaDoEtosans4, respostaAgradaMelodys4);
        possiveisSituacoesTotal.AddLast(s4);

        string ids5 = "situacao5";
        LinkedList<string> temposVerbaiss5 = new LinkedList<string>();
        temposVerbaiss5.AddLast("～のがすきです");
        temposVerbaiss5.AddLast("～たいです");
        temposVerbaiss5.AddLast("～てもいいです");
        temposVerbaiss5.AddLast("～ほうがいい");
        temposVerbaiss5.AddLast("～ないほうがいい");
        string verbos5 = "あるく";
        string lugars5 = "こうえん";
        LinkedList<string> duasRespostasDoEtosans5 = new LinkedList<string>();
        duasRespostasDoEtosans5.AddLast("こうえんを　あるいたほうがいいです");
        duasRespostasDoEtosans5.AddLast("こうえんを　あるかないほうがいいです");
        string respostaCorretaDoEtosans5 = "こうえんを　あるいたほうがいいです";
        bool respostaAgradaMelodys5 = true;

        SituacaoModoHougaii s5 = new SituacaoModoHougaii(ids5, temposVerbaiss5, verbos5, lugars5, duasRespostasDoEtosans5,
                                    respostaCorretaDoEtosans5, respostaAgradaMelodys5);
        possiveisSituacoesTotal.AddLast(s5);

        string ids6 = "situacao6";
        LinkedList<string> temposVerbaiss6 = new LinkedList<string>();
        temposVerbaiss6.AddLast("～のがすきです");
        temposVerbaiss6.AddLast("～たいです");
        temposVerbaiss6.AddLast("～てもいいです");
        temposVerbaiss6.AddLast("～ほうがいい");
        temposVerbaiss6.AddLast("～ないほうがいい");
        string verbos6 = "さんぽする";
        string lugars6 = "みち";
        LinkedList<string> duasRespostasDoEtosans6 = new LinkedList<string>();
        duasRespostasDoEtosans5.AddLast("みちを　さんぽしたほうがいいです");
        duasRespostasDoEtosans5.AddLast("みちを　さんぽしないほうがいいです");
        string respostaCorretaDoEtosans6 = "みちを　さんぽしたほうがいいです";
        bool respostaAgradaMelodys6 = true;

        SituacaoModoHougaii s6 = new SituacaoModoHougaii(ids6, temposVerbaiss6, verbos6, lugars6, duasRespostasDoEtosans6,
                                    respostaCorretaDoEtosans6, respostaAgradaMelodys6);
        possiveisSituacoesTotal.AddLast(s6);
    }
}
