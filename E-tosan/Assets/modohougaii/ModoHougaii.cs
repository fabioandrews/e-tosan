﻿using UnityEngine;
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
    private SituacaoModoHougaii situacaoAtual; //a situacao que atualmente esta pasando na tela da situacao. Nessa classe, esse atribute eh util apenas para o botaoInicioLetsJam conseguir obter as quatro situacoes atuais e nao apenas 3
    private bool melodyEstahZangadaAtualmente;
    private string fecharLetsJamDeveVoltarAQualTela; //pode ser situacaoAtual, pode ser a tela onde o usuario escolhe a resposta correta ou pode ser 
    private int turnoDeQuatroSituacoesAtual; //de 4 em 4 situacoes esse valor aumenta
    private string ondeEstaoOsArquivosDeAudioDoLetsJam = "Assets/modohougaii/audiosModoHougaii/letsjam"; //esse atributo eh usado pelo botaoInicioLetsJam
    private string ondeEstaoOsArquivosDeAudioDeSituacoes = "Assets/modohougaii/audiosModoHougaii/situacao"; //usado por TelaSituacaoHougaii
    private string aoFecharLetsJamMostrarQualTela; //quando o usuario fechar a tela letsjam, qual tela deveria ser mostrada? Quem interpreta eh o fechar_letsjam_modo_hougaii
    private int scoreDaPartida; //o score atual da partida
    private int percentualDaBarrinhaAfeicaoMelody;//o percentual da barrinha de afeicao da melody atual. lah da tela da escolha do eto-san
    private int percentualDaBarrinhaBondade;
    private  int percentualCarinhaAfeicaoMelody; //percentual de uma carinha ainda nao cheia de afeicao da Melody
    private int quantasCarinhasAfeicaoMelodyEstaoCheias;
    private int percentualCarinhaBondade; //percentual de uma carinha ainda nao cheia de bondade
    private int quantasCarinhasBondadeEstaoCheias;
    private int quantasMoedasObtidasDuranteOJogo; //quantas moedas foram obtidas em uma partida do modo hougaii

    private AudioSource source; //util para qualuqer efeito sonoro que o jogo precise executar
    public AudioClip errou_alternativa_escolha_etosan;
    public AudioClip acertou_alternativa_escolha_etosan;
    public AudioClip obteu_moedinha_barra_bondade_ou_afeicao;

    // Use this for initialization
    void Start ()
    {
        this.fornecedorSituacoes = new FornecedorDeSituacoes();
        melodyEstahZangadaAtualmente = false;
        
        this.turnoDeQuatroSituacoesAtual = 0;
        this.scoreDaPartida = 0;
        this.quatroSituacoesAtuais = new LinkedList<SituacaoModoHougaii>();
        this.situacaoAtual = null;

        this.percentualDaBarrinhaAfeicaoMelody = 50;
        this.percentualDaBarrinhaBondade = 50;
        this.percentualCarinhaAfeicaoMelody = 0;
        this.quantasCarinhasAfeicaoMelodyEstaoCheias = 0;
        this.percentualCarinhaBondade = 0;
        this.quantasCarinhasBondadeEstaoCheias = 0;
        this.quantasMoedasObtidasDuranteOJogo = 0;


        this.tornarTodosOsPopupsDestaCenaInvisiveis();

        /*//SE QUISER MOSTRAR AO PROF APENAS O RAIDO DO LETSJAM FUNCIONANDO, 
          //BASTA DESCOMENTAR ISSO E COMENTAR TUDO QUE VIER DEPOIS NESTE START()
         * obterQuatroSituacoesAtuais();
          this.fazerPopupLetsJamAparecer(); */

        this.definirNovaSituacaoAtualTelaHougaii();
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    public string getondeEstaoOsArquivosDeAudioDoLetsJam()
    {
        return ondeEstaoOsArquivosDeAudioDoLetsJam;
    }

    public string getondeEstaoOsArquivosDeAudioDeSituacoes()
    {
        return ondeEstaoOsArquivosDeAudioDeSituacoes;
    }

    public void setaoFecharLetsJamMostrarQualTela(string novoValor)
    {
        this.aoFecharLetsJamMostrarQualTela = novoValor;
    }

    public string getaoFecharLetsJamMostrarQualTela()
    {
        return this.aoFecharLetsJamMostrarQualTela;
    }

    private void obterQuatroSituacoesAtuais()
    {
        this.quatroSituacoesAtuais = fornecedorSituacoes.fornecer4Situacoes(melodyEstahZangadaAtualmente);
    }

    public LinkedList<SituacaoModoHougaii> getquatroSituacoesAtuais() //essa linked list pode nao ter size = 4 sempre. Ela pode diminuir
    {
        return this.quatroSituacoesAtuais;
    }

    //funcao chamada apenas por botaoInicioLetsJam porque ele precisa tanto das quatroSituacoesAtuais como da situacao que ja foi removida de quatroSituacoesAtuais
    public SituacaoModoHougaii getsituacaoAtual()
    {
        return this.situacaoAtual;
    }

    public void setscoreDaPartida(int novoValor)
    {
        this.scoreDaPartida = novoValor;
    }
    public int getscoreDaPartida()
    {
        return this.scoreDaPartida;
    }
    public void setpercentualDaBarrinhaAfeicaoMelody(int novoValor)
    {
        this.percentualDaBarrinhaAfeicaoMelody = novoValor;
    }
    public void setpercentualDaBarrinhaBondade(int novoValor)
    {
        this.percentualDaBarrinhaBondade = novoValor;
    }
    public int getpercentualDaBarrinhaAfeicaoMelody()
    {
        return this.percentualDaBarrinhaAfeicaoMelody;
    }
    public int getpercentualDaBarrinhaBondade()
    {
        return this.percentualDaBarrinhaBondade;
    }

    //pega o ultimo percentual da lista. serve para encher essa carinha ou ver se ja eh hora de criar uma nova
    //pode retornar -1 se nao ha carinhas na lista
    public int getpercentualCarinhaAfeicaoMelody()
    {
        return this.percentualCarinhaAfeicaoMelody;
    }
    public int getpercentualCarinhaBondade()
    {
        return this.percentualCarinhaBondade;
    }

    //util p saber, na tela de escolha do etosan, quantas carinhas cheias devem ser mostradas
    public int getquantasCarinhasAfeicaoMelodyEstaoCheias()
    {
        return this.quantasCarinhasAfeicaoMelodyEstaoCheias;
    }
    public int getquantasCarinhasBondadeEstaoCheias()
    {
        return this.quantasCarinhasBondadeEstaoCheias;
    }
    public int getquantasMoedasObtidasDuranteOJogo()
    {
        return this.quantasMoedasObtidasDuranteOJogo;
    }
    //as vezes a barrinha de bondade e afeicao farah o usuario ganhar moedinhas novas!
    public void aumentarquantasMoedasObtidasDuranteOJogo(int quantasMoedasAMais)
    {
        this.quantasMoedasObtidasDuranteOJogo = this.quantasMoedasObtidasDuranteOJogo + quantasMoedasAMais;
    }

    //versao antiga! as carinhas apenas enchem!
    /*public void encherPercentualCarinhaAfeicaoMelodyComBaseNoPercentualDaBarrinha()
    {
        if (percentualCarinhaAfeicaoMelody + this.percentualDaBarrinhaAfeicaoMelody >= 100)
        {
            //mais uma carinha cheia! Devemos tambem esvaziar a carinha atual
            this.quantasCarinhasAfeicaoMelodyEstaoCheias = this.quantasCarinhasAfeicaoMelodyEstaoCheias + 1;
            this.percentualCarinhaAfeicaoMelody = 0;
        }
        else
        {
            //nao vamos esvaziar a carinha atual
            percentualCarinhaAfeicaoMelody = percentualCarinhaAfeicaoMelody + this.percentualDaBarrinhaAfeicaoMelody;
        } 
    }
    public void encherPercentualCarinhaBondadeComBaseNoPercentualDaBarrinha()
    {

        if (percentualCarinhaBondade + this.percentualDaBarrinhaBondade >= 100)
        {
            //mais uma carinha cheia! Devemos tambem esvaziar a carinha atual
            this.quantasCarinhasBondadeEstaoCheias = this.quantasCarinhasBondadeEstaoCheias + 1;
            this.percentualCarinhaBondade = 0;
        }
        else
        {
            //nao vamos esvaziar a carinha atual
            percentualCarinhaBondade = percentualCarinhaBondade + this.percentualDaBarrinhaBondade;
        }
    }*/

    //versao nova, as carinhas enchem e descem tb!
    public void encherPercentualCarinhaAfeicaoMelodyComBaseNoPercentualDaBarrinha()
    {
        if (percentualDaBarrinhaAfeicaoMelody >= 0 && percentualDaBarrinhaAfeicaoMelody < 25)
        {
            //diminuir 50% da carinha afeicao
            this.percentualCarinhaAfeicaoMelody = this.percentualCarinhaAfeicaoMelody - 50;
        }
        else if (percentualDaBarrinhaAfeicaoMelody >= 25 && percentualDaBarrinhaAfeicaoMelody < 50)
        {
            this.percentualCarinhaAfeicaoMelody = this.percentualCarinhaAfeicaoMelody - 25;
        }
        else if (percentualDaBarrinhaAfeicaoMelody == 50)
        {
            //nao faz nada. Carinha permanece intacta
        }
        else if (percentualDaBarrinhaAfeicaoMelody >= 51 && percentualDaBarrinhaAfeicaoMelody < 75)
        {
            this.percentualCarinhaAfeicaoMelody = this.percentualCarinhaAfeicaoMelody + 25;
        }
        else
        {
            //percentual barrinha maior que 75
            this.percentualCarinhaAfeicaoMelody = this.percentualCarinhaAfeicaoMelody + 50;
        }

        //serah que no fim de tudo extrapolou alguma coisa?
        if (percentualCarinhaAfeicaoMelody < 0)
        {
            percentualCarinhaAfeicaoMelody = 0;
        }
        else if (percentualCarinhaAfeicaoMelody >= 100)
        {
            //mais uma carinha cheia! Devemos tambem esvaziar a carinha atual
            this.quantasCarinhasAfeicaoMelodyEstaoCheias = this.quantasCarinhasAfeicaoMelodyEstaoCheias + 1;
            this.percentualCarinhaAfeicaoMelody = 0;
        }
    }

    //versao nova, as carinhas enchem e descem tb!
    public void encherPercentualCarinhaBondadeComBaseNoPercentualDaBarrinha()
    {
        if (percentualDaBarrinhaBondade >= 0 && percentualDaBarrinhaBondade < 25)
        {
            //diminuir 50% da carinha bondade
            this.percentualCarinhaBondade = this.percentualCarinhaBondade - 50;
        }
        else if (percentualDaBarrinhaBondade >= 25 && percentualDaBarrinhaBondade < 50)
        {
            this.percentualCarinhaBondade = this.percentualCarinhaBondade - 25;
        }
        else if (percentualDaBarrinhaBondade == 50)
        {
            //nao faz nada. Carinha permanece intacta
        }
        else if (percentualDaBarrinhaBondade >= 51 && percentualDaBarrinhaBondade < 75)
        {
            this.percentualCarinhaBondade = this.percentualCarinhaBondade + 25;
        }
        else
        {
            //percentual barrinha maior que 75
            this.percentualCarinhaBondade = this.percentualCarinhaBondade + 50;
        }

        //serah que no fim de tudo extrapolou alguma coisa?
        if (percentualCarinhaBondade < 0)
        {
            percentualCarinhaBondade = 0;
        }
        else if (percentualCarinhaBondade >= 100)
        {
            //mais uma carinha cheia! Devemos tambem esvaziar a carinha atual
            this.quantasCarinhasBondadeEstaoCheias = this.quantasCarinhasBondadeEstaoCheias + 1;
            this.percentualCarinhaBondade = 0;
        }
    }

    //quando o usuario escolhe a resposta ruim apenas para agradar a melody, a barrinha dela cresce,
    //mas ele eh penalizado com uma diminuicao de bondade e de percentual de bondade
    public void removerUmaCarinhaBondadeEEsvaziarCarinhaAtual()
    {
        if (this.quantasCarinhasBondadeEstaoCheias > 0)
        {
            this.quantasCarinhasBondadeEstaoCheias = this.quantasCarinhasBondadeEstaoCheias - 1;
        }
        this.percentualCarinhaBondade = 0;
    }



    private void fazerPopupLetsJamAparecer()
    {
        GameObject instanciainicioLetsJam = GameObject.Find("inicioLetsJam");
        PopupWindowBehavior instanciainicioLetsJamComTipoReal = instanciainicioLetsJam.GetComponent<PopupWindowBehavior>(); ;
        instanciainicioLetsJamComTipoReal.voltarAPosicaoInicial();
    }

    private void fazerTelaSituacaoHougaiiAparecer()
    {
        GameObject telaSituacaoHougaii = GameObject.Find("telaSituacaoHougaii");
        PopupWindowBehavior telaSituacaoHougaiiComTipoReal = telaSituacaoHougaii.GetComponent<PopupWindowBehavior>(); ;
        telaSituacaoHougaiiComTipoReal.voltarAPosicaoInicial();
    }

    private void fazerTelaSituacaoHougaiiDesaparecer()
    {
        GameObject telaSituacaoHougaii = GameObject.Find("telaSituacaoHougaii");
        PopupWindowBehavior telaSituacaoHougaiiComTipoReal = telaSituacaoHougaii.GetComponent<PopupWindowBehavior>(); ;
        telaSituacaoHougaiiComTipoReal.irParaPosicaoDeDesaparecer();
    }


    //esse metodo serah chamado no inicio do jogo e sempre que o usuario ja terminar com a situacao atual
    public void definirNovaSituacaoAtualTelaHougaii()
    {
        if (this.quatroSituacoesAtuais.Count > 0)
        {
            //vou remover aleatoriamente uma das 4 situacoes
            SituacaoModoHougaii situacaoVouRemover = null;
            System.Random rnd = new System.Random();
            int posicaoSituacaoVouRemover = rnd.Next(0, this.quatroSituacoesAtuais.Count); // creates a number between 1 and 12
                                                                                                  //em c# nao existe get em linkedlist. Logo, teremos de fazer isso:
            LinkedListNode<SituacaoModoHougaii> percorredorLista = this.quatroSituacoesAtuais.First;
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

            this.situacaoAtual = situacaoVouRemover;
            this.quatroSituacoesAtuais.Remove(situacaoVouRemover);
            GameObject telaSituacaoHougaii = GameObject.Find("telaSituacaoHougaii");
            TelaSituacaoHougaii telaSituacaoHougaiiComTipoReal = telaSituacaoHougaii.GetComponent<TelaSituacaoHougaii>();
            telaSituacaoHougaiiComTipoReal.iniciarNovamenteTelaSituacaoHougaii();
            telaSituacaoHougaiiComTipoReal.setarSituacaoAtualESeuArquivoDeAudio(situacaoVouRemover);
            telaSituacaoHougaiiComTipoReal.setUsuarioEstaDentroDeLetsJam(false);
            aoFecharLetsJamMostrarQualTela = "telaSituacaoHougaii";
            this.fazerTelaSituacaoHougaiiAparecer();

            //imprimirQuatroSituacaosAtuaisESituacaoAtual();
        }
        else
        {
            turnoDeQuatroSituacoesAtual = this.turnoDeQuatroSituacoesAtual + 1;
            if (this.turnoDeQuatroSituacoesAtual > 1) //SOH PARA TESTES! AQUI DEVERIA SER 2
            {
                //HORA DE TERMINAR O JOGO!!!
                this.terminarOJogo();
            }
            else
            {
                //vamos gerar 4 novas situacoes atuais
                obterQuatroSituacoesAtuais();

                //vou remover aleatoriamente uma das 4 situacoes
                SituacaoModoHougaii situacaoVouRemover = null;
                System.Random rnd = new System.Random();
                int posicaoSituacaoVouRemover = rnd.Next(0, this.quatroSituacoesAtuais.Count); // creates a number between 1 and 12
                                                                                               //em c# nao existe get em linkedlist. Logo, teremos de fazer isso:
                LinkedListNode<SituacaoModoHougaii> percorredorLista = this.quatroSituacoesAtuais.First;
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

                this.situacaoAtual = situacaoVouRemover;
                this.quatroSituacoesAtuais.Remove(situacaoVouRemover);
                
                GameObject telaSituacaoHougaii = GameObject.Find("telaSituacaoHougaii");
                TelaSituacaoHougaii telaSituacaoHougaiiComTipoReal = telaSituacaoHougaii.GetComponent<TelaSituacaoHougaii>(); ;
                telaSituacaoHougaiiComTipoReal.iniciarNovamenteTelaSituacaoHougaii();
                telaSituacaoHougaiiComTipoReal.setarSituacaoAtualESeuArquivoDeAudio(situacaoVouRemover);
                telaSituacaoHougaiiComTipoReal.setUsuarioEstaDentroDeLetsJam(true);

                //this.fazerTelaSituacaoHougaiiAparecer();
                this.fazerPopupLetsJamAparecer();
                aoFecharLetsJamMostrarQualTela = "telaSituacaoHougaii";

                //imprimirQuatroSituacaosAtuaisESituacaoAtual();
            }
        }
    }

    private void imprimirQuatroSituacaosAtuaisESituacaoAtual()
    {
        print("//////quatro atuais");
        LinkedListNode<SituacaoModoHougaii> percorredorLista = quatroSituacoesAtuais.First;
        while (percorredorLista != null && percorredorLista.Value != null)
        {
            print(percorredorLista.Value.getnomeArquivoWavMelody());
            percorredorLista = percorredorLista.Next;
        }

        print("/////a atual");
        print(this.situacaoAtual.getnomeArquivoWavMelody());

    }

    private void tornarTodosOsPopupsDestaCenaInvisiveis()
    {
        PopupWindowBehavior instanciaSituacaoHougaii = GameObject.Find("telaSituacaoHougaii").GetComponent<PopupWindowBehavior>();
        instanciaSituacaoHougaii.obterPosicaoInicial();
        instanciaSituacaoHougaii.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior inicioLetsJam = GameObject.Find("inicioLetsJam").GetComponent<PopupWindowBehavior>();
        inicioLetsJam.obterPosicaoInicial();
        inicioLetsJam.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior radioLetsJam = GameObject.Find("radioLetsJam").GetComponent<PopupWindowBehavior>();
        radioLetsJam.obterPosicaoInicial();
        radioLetsJam.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior nomeMusicaAtualRadioLetsJam = GameObject.Find("nomeMusicaAtualRadioLetsJam").GetComponent<PopupWindowBehavior>();
        nomeMusicaAtualRadioLetsJam.obterPosicaoInicial();
        nomeMusicaAtualRadioLetsJam.irParaPosicaoDeDesaparecer();
        PopupWindowBehavior traducaoMusicaAtualRadioLetsJam = GameObject.Find("traducaoMusicaAtualRadioLetsJam").GetComponent<PopupWindowBehavior>();
        traducaoMusicaAtualRadioLetsJam.obterPosicaoInicial();
        traducaoMusicaAtualRadioLetsJam.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior PopupPauseLetsJam = GameObject.Find("PopupPauseLetsJam").GetComponent<PopupWindowBehavior>();
        PopupPauseLetsJam.obterPosicaoInicial();
        PopupPauseLetsJam.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior PopupTemCertezaQueQuerIrAoMainMenu = GameObject.Find("PopupTemCertezaQueQuerIrAoMainMenu").GetComponent<PopupWindowBehavior>();
        PopupTemCertezaQueQuerIrAoMainMenu.obterPosicaoInicial();
        PopupTemCertezaQueQuerIrAoMainMenu.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior telaEscolhaEtosanHougaii = GameObject.Find("telaEscolhaEtosanHougaii").GetComponent<PopupWindowBehavior>();
        telaEscolhaEtosanHougaii.obterPosicaoInicial();
        telaEscolhaEtosanHougaii.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior escolhaEtosanHougaiiTexto = GameObject.Find("escolhaEtosanHougaiiTexto").GetComponent<PopupWindowBehavior>();
        escolhaEtosanHougaiiTexto.obterPosicaoInicial();
        escolhaEtosanHougaiiTexto.irParaPosicaoDeDesaparecer();

        UIBarScript barra_afeicao_melody = GameObject.Find("barra_afeicao_melody").GetComponent<UIBarScript>();
        barra_afeicao_melody.irParaPosicaoDeDesaparecer();
        UIBarScript barra_bondade = GameObject.Find("barra_bondade").GetComponent<UIBarScript>();
        barra_bondade.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior telaFimDeJogo = GameObject.Find("telaFimDeJogo").GetComponent<PopupWindowBehavior>();
        telaFimDeJogo.obterPosicaoInicial();
        telaFimDeJogo.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior telaFimDeJogoTexto = GameObject.Find("telaFimDeJogoTexto").GetComponent<PopupWindowBehavior>();
        telaFimDeJogoTexto.obterPosicaoInicial();
        telaFimDeJogoTexto.irParaPosicaoDeDesaparecer();

        moedaBarra moedaBarraBondade = GameObject.Find("moedaBarraBondade").GetComponent<moedaBarra>();
        moedaBarraBondade.obterPosicaoInicial(); //pq faco isso aqui? Porque embora moeda nao seja GUI, preciso da posicao inicial dela no comeco do jogo!
        //as moedas nao precisam voltar a sua posicao inicial porque isso ja acontece quando a barra de bondade e afeicao voltam a sua posicao inicial... elas voltam tb, que na verdade significa que elas setam o alpha
        moedaBarra moedaBarraAfeicaoMelody = GameObject.Find("moedaBarraAfeicaoMelody").GetComponent<moedaBarra>();
        moedaBarraAfeicaoMelody.obterPosicaoInicial(); 

    }

    //FALTA TERMINAR O JOGO!!!
    private void terminarOJogo()
    {
        PopupWindowBehavior telaFimDeJogo = GameObject.Find("telaFimDeJogo").GetComponent<PopupWindowBehavior>();
        telaFimDeJogo.voltarAPosicaoInicial();
        PopupWindowBehavior telaFimDeJogoTexto = GameObject.Find("telaFimDeJogoTexto").GetComponent<PopupWindowBehavior>();
        telaFimDeJogoTexto.voltarAPosicaoInicial();
        GameObject.Find("telaFimDeJogo").GetComponent<telaFimDeJogoModoHougaii>().mudarTextosDATelaFimDeJogoModoHougaii();
    }

    public void playEfeitoSonoro(string nomeEfeitoSonoro)
    {
        if (source == null) source = gameObject.AddComponent<AudioSource>();

        if (nomeEfeitoSonoro.CompareTo("errou_alternativa_escolha_etosan") == 0)
        {
            this.source.clip = errou_alternativa_escolha_etosan;
        }
        else if (nomeEfeitoSonoro.CompareTo("acertou_alternativa_escolha_etosan") == 0)
        {
            this.source.clip = acertou_alternativa_escolha_etosan;
        }
        else if (nomeEfeitoSonoro.CompareTo("obteu_moedinha_barra_bondade_ou_afeicao") == 0)
        {
            this.source.clip = obteu_moedinha_barra_bondade_ou_afeicao;
        }

        this.source.Play();
    }

    public void reiniciarJogoModoHougaii()
    {
        Application.LoadLevel(Application.loadedLevel);
    }

}
