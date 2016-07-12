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
    private string fecharLetsJamDeveVoltarAQualTela; //pode ser situacaoAtual, pode ser a tela onde o usuario escolhe a resposta correta ou pode ser 
    private int turnoDeQuatroSituacoesAtual; //de 4 em 4 situacoes esse valor aumenta
    private string ondeEstaoOsArquivosDeAudioDoLetsJam = "Assets/modohougaii/audiosModoHougaii/letsjam"; //esse atributo eh usado pelo botaoInicioLetsJam
    private string ondeEstaoOsArquivosDeAudioDeSituacoesDoLetsJam = "Assets/modohougaii/audiosModoHougaii/situacao"; //usado por TelaSituacaoHougaii

    // Use this for initialization
    void Start ()
    {
        this.fornecedorSituacoes = new FornecedorDeSituacoes();
        melodyEstahZangadaAtualmente = false;
        
        this.turnoDeQuatroSituacoesAtual = 0;
        this.quatroSituacoesAtuais = new LinkedList<SituacaoModoHougaii>();

        //this.definirNovaSituacaoAtualTelaHougaii(); EH O NORMAL! SOH ESTOU TESTANDO COM OS METODOS ABAIXO PARA VER SE LETS JAM AINDA FUNCIONA!
        obterQuatroSituacoesAtuais();


        this.tornarTodosOsPopupsDestaCenaInvisiveis();

        this.fazerPopupLetsJamAparecer();
    }
	
	// Update is called once per frame
	void Update ()
    {
    }

    public string getondeEstaoOsArquivosDeAudioDoLetsJam()
    {
        return ondeEstaoOsArquivosDeAudioDoLetsJam;
    }

    public string getondeEstaoOsArquivosDeAudioDeSituacoesDoLetsJam()
    {
        return ondeEstaoOsArquivosDeAudioDeSituacoesDoLetsJam;
    }

    private void obterQuatroSituacoesAtuais()
    {
        this.quatroSituacoesAtuais = fornecedorSituacoes.fornecer4Situacoes(melodyEstahZangadaAtualmente);
    }

    public LinkedList<SituacaoModoHougaii> getquatroSituacoesAtuais()
    {
        return this.quatroSituacoesAtuais;
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

            this.quatroSituacoesAtuais.Remove(situacaoVouRemover);
            GameObject telaSituacaoHougaii = GameObject.Find("telaSituacaoHougaii");
            TelaSituacaoHougaii telaSituacaoHougaiiComTipoReal = telaSituacaoHougaii.GetComponent<TelaSituacaoHougaii>(); ;
            telaSituacaoHougaiiComTipoReal.setarSituacaoAtualESeuArquivoDeAudio(situacaoVouRemover);
        }
        else
        {
            turnoDeQuatroSituacoesAtual = this.turnoDeQuatroSituacoesAtual + 1;
            if (this.turnoDeQuatroSituacoesAtual > 2)
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

                this.quatroSituacoesAtuais.Remove(situacaoVouRemover);
                
                GameObject telaSituacaoHougaii = GameObject.Find("telaSituacaoHougaii");
                TelaSituacaoHougaii telaSituacaoHougaiiComTipoReal = telaSituacaoHougaii.GetComponent<TelaSituacaoHougaii>(); ;
                telaSituacaoHougaiiComTipoReal.setarSituacaoAtualESeuArquivoDeAudio(situacaoVouRemover);
                telaSituacaoHougaiiComTipoReal.setUsuarioEstaDentroDeLetsJam(true);

                this.fazerTelaSituacaoHougaiiAparecer();
                this.fazerPopupLetsJamAparecer();
            }
        }
    }

    private void tornarTodosOsPopupsDestaCenaInvisiveis()
    {
        PopupWindowBehavior instanciaSituacaoHougaii = GameObject.Find("telaSituacaoHougaii").GetComponent<PopupWindowBehavior>();
        instanciaSituacaoHougaii.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior inicioLetsJam = GameObject.Find("inicioLetsJam").GetComponent<PopupWindowBehavior>(); 
        inicioLetsJam.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior radioLetsJam = GameObject.Find("radioLetsJam").GetComponent<PopupWindowBehavior>();
        radioLetsJam.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior nomeMusicaAtualRadioLetsJam = GameObject.Find("nomeMusicaAtualRadioLetsJam").GetComponent<PopupWindowBehavior>();
        nomeMusicaAtualRadioLetsJam.irParaPosicaoDeDesaparecer();
        PopupWindowBehavior traducaoMusicaAtualRadioLetsJam = GameObject.Find("traducaoMusicaAtualRadioLetsJam").GetComponent<PopupWindowBehavior>();
        traducaoMusicaAtualRadioLetsJam.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior PopupPauseLetsJam = GameObject.Find("PopupPauseLetsJam").GetComponent<PopupWindowBehavior>();
        PopupPauseLetsJam.irParaPosicaoDeDesaparecer();

        PopupWindowBehavior PopupTemCertezaQueQuerIrAoMainMenu = GameObject.Find("PopupTemCertezaQueQuerIrAoMainMenu").GetComponent<PopupWindowBehavior>();
        PopupTemCertezaQueQuerIrAoMainMenu.irParaPosicaoDeDesaparecer();

    }

    //FALTA TERMINAR O JOGO!!!
    private void terminarOJogo()
    {
    }

}
