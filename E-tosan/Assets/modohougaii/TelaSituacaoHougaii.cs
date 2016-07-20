using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;

public class TelaSituacaoHougaii : MonoBehaviour
{
    private bool usuarioEstaDentroDeLetsJam; //se o usuario estiver dentro do letsjam, a situacao atual deve parar e continuar apenas quando ele voltar
    private SituacaoModoHougaii situacaoAtual;
    private AudioSource arquivoAudioSituacaoAtualMelody;
    private AudioSource arquivoAudioSituacaoAtualRegras;
    private bool arquivoAudioSituacaoAtualMelodyEstaPausado; //infelizmente nao existe um audiosource,ispaused, entao uma musica isplaying = false pode significar pausada ou nunca iniciada ou stoped
    private bool arquivoAudioSituacaoAtualRegrasEstaPausado;

    private string arquivoDeAudioQueEstaTocandoAgoraEhmelodyOUregras; //pode ser "melody" ou "regras"
    private LinkedList<FileInfo> soundFiles; //necessario para quando for carregar arquivos de audio do diretorio
    private bool esperarSituacaoRegrasTerminarParaPassarParaProximaTelaDeveriaTerminarSemFazerNada;
    //a corotina acima eh bem sensivel: ela termina assim que os dois audios param de tocar e ja passa para a
    //tela final. Porem, todos sabemos que assim que o usuario clica no botao reiniciar, os audios param e nao devemos ir para a tela final, nao eh?
    //Entao, esse booleano vai me ajudar a fazer essa corotina andar na linha. Ele sera alterado para
    //false assim que a corotina iniciar e devera ser true assim que o botao de reiniciar for apertado
    //(ou seja, a funcao recomecarAudioSituacaoAtual() for tocada) e deve voltar a ser false pela corotina que o checou
    //se ele for false quando ela checar

    public Sprite sprite_para_figura_central_melody;
    public Sprite sprite_para_figura_central_regras;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    //chamado pela telaEscolhaEtosanHougaii
    public SituacaoModoHougaii getSituacaoAtual()
    {
        return this.situacaoAtual;
    }

    /*funcao chamada sempre que uma nova situacao deve aparecer na tela e estas variaveis booleanas devem ser resetadas*/
    public void iniciarNovamenteTelaSituacaoHougaii()
    {
        arquivoAudioSituacaoAtualMelody = null;
        arquivoAudioSituacaoAtualRegras = null;
        arquivoAudioSituacaoAtualMelodyEstaPausado = false; 
        arquivoAudioSituacaoAtualRegrasEstaPausado = false;
        arquivoDeAudioQueEstaTocandoAgoraEhmelodyOUregras = null;
        esperarSituacaoRegrasTerminarParaPassarParaProximaTelaDeveriaTerminarSemFazerNada = false;

    }

    public void setUsuarioEstaDentroDeLetsJam(bool novoValor)
    {
        usuarioEstaDentroDeLetsJam = novoValor;

        if (usuarioEstaDentroDeLetsJam == true)
        {
            //TEMOS DE PARAR O AUDIO DA SITUACAO ATUAL SE HOUVER(NO INICIO DO JOGO, NAO HA)!
            if (arquivoDeAudioQueEstaTocandoAgoraEhmelodyOUregras != null && 
                arquivoDeAudioQueEstaTocandoAgoraEhmelodyOUregras.CompareTo("melody") == 0)
            {
                this.arquivoAudioSituacaoAtualMelody.Pause();
                arquivoAudioSituacaoAtualMelodyEstaPausado = true;
            }
            else if(arquivoDeAudioQueEstaTocandoAgoraEhmelodyOUregras != null)
            {
                this.arquivoAudioSituacaoAtualRegras.Pause();
                arquivoAudioSituacaoAtualRegrasEstaPausado = true;
            }
        }
        else
        {
            //hora de continuar o arquivo de audio da melody/regras ou comecar
            //SE NADA DER CERTO, TIRE ESSES BOOLEANS arquivoAudioSituacaoAtualMelodyEstaPausado E AS CHECAGENS DELES ABAIXO
            //E FACA SEMPRE QUE O USUARIO CLICAR NO RADIO, ELE PARA E RECOMECA TODAS AS MUSICAS DO ZERO MESMO!
            if (arquivoAudioSituacaoAtualMelodyEstaPausado == true)
            {
                arquivoAudioSituacaoAtualMelodyEstaPausado = false;
                this.arquivoAudioSituacaoAtualMelody.UnPause();
            }
            else if (arquivoAudioSituacaoAtualRegrasEstaPausado == true)
            {
                arquivoAudioSituacaoAtualRegrasEstaPausado = false;
                this.arquivoAudioSituacaoAtualRegras.UnPause();
            }
            else if ((arquivoAudioSituacaoAtualRegrasEstaPausado == false && arquivoAudioSituacaoAtualMelodyEstaPausado == false) &&
                this.arquivoAudioSituacaoAtualMelody.isPlaying == false && this.arquivoAudioSituacaoAtualRegras.isPlaying == false)
            {
                this.comecarArquivoDeAudioDaMelodyEMudarFiguraCentral();
            }
        }
    }

    //assim que o arquivo de audio com a fala da melody parar, o da regras deve comecar
    IEnumerator esperarSituacaoMelodyTerminarParaDarPlayNaSituacaoRegras()
    {
        while (this.arquivoAudioSituacaoAtualRegras.isPlaying == false)
        {
            if (this.usuarioEstaDentroDeLetsJam == false && this.arquivoAudioSituacaoAtualMelody.isPlaying == false)
            {
                //hora de comecar o outro audio!
                this.comecarArquivoDeAudioRegrasEMudarFiguraCentral();
            }

            yield return new WaitForSeconds(.1f);
        }
    }
    public void setarSituacaoAtualESeuArquivoDeAudio(SituacaoModoHougaii situacaoNova)
    {
        this.situacaoAtual = situacaoNova;

        //falta procurar, carregar e setar o arquivo de audio
        GameObject modoHougaii = GameObject.Find("Main Camera");
        ModoHougaii modoHougaiiComTipoReal = modoHougaii.GetComponent<ModoHougaii>(); ;

        string ondeEstaoOsArquivosDeAudioDeSituacoes=
            modoHougaiiComTipoReal.getondeEstaoOsArquivosDeAudioDeSituacoes();

        if (soundFiles == null)
        {
            soundFiles = new LinkedList<FileInfo>();
        }
        else
        {
            soundFiles.Clear();
        }
        // get all valid files
        var info = new DirectoryInfo(ondeEstaoOsArquivosDeAudioDeSituacoes);
        FileInfo[] fileInfos = info.GetFiles();

        for (int i = 0; i < fileInfos.Length; i++)
        {
            FileInfo umFileInfo = fileInfos[i];
            if ((this.situacaoAtual.getnomeArquivoWavMelody().CompareTo(umFileInfo.Name) == 0) ||
                    (this.situacaoAtual.getnomeArquivoWavRegras().CompareTo(umFileInfo.Name) == 0))
            {
                soundFiles.AddLast(umFileInfo);
            }
        }


        foreach (var s in soundFiles)
        {
            if (s.Name.Contains("melody") == true)
            {
                StartCoroutine(LoadFile(s.FullName, "melody"));
            }
            else if (s.Name.Contains("regras") == true)
            {
                StartCoroutine(LoadFile(s.FullName, "regras"));
            }
        }

        //StartCoroutine(LoadFile(ondeEstaoOsArquivosDeAudioDeSituacoesDoLetsJam + "/" + this.situacaoAtual.getnomeArquivoWavMelody(),"melody"));
        //StartCoroutine(LoadFile(ondeEstaoOsArquivosDeAudioDeSituacoesDoLetsJam + "/" + this.situacaoAtual.getnomeArquivoWavRegras(), "regras"));

    }

    IEnumerator LoadFile(string path,string melodyOuRegras)
    {
        WWW www = new WWW("file://" + path);
        print("loading " + path);

        AudioClip clip = www.GetAudioClip(false);

        while (!clip.isReadyToPlay)
            yield return www;

        print("done loading");
        clip.name = Path.GetFileName(path);

        if (melodyOuRegras.CompareTo("melody") == 0)
        {
            if (this.arquivoAudioSituacaoAtualMelody == null)
            {
                this.arquivoAudioSituacaoAtualMelody = gameObject.AddComponent<AudioSource>();
            }
            this.arquivoAudioSituacaoAtualMelody.clip = clip;
        }
        else
        {
            if (this.arquivoAudioSituacaoAtualRegras == null)
            {
                this.arquivoAudioSituacaoAtualRegras = gameObject.AddComponent<AudioSource>();
            }

            this.arquivoAudioSituacaoAtualRegras.clip = clip;
        }
        
    }
    

    private void comecarArquivoDeAudioDaMelodyEMudarFiguraCentral()
    {
        //hora de comecar algum arquivo de audio: o da melody
        this.arquivoAudioSituacaoAtualMelody.Play();
        arquivoDeAudioQueEstaTocandoAgoraEhmelodyOUregras = "melody";

        //mudar figura central
        SpriteRenderer spriteRendererFiguraCentral =
            GameObject.Find("figuraCentralSituacaoAtualHougaii").GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject
        spriteRendererFiguraCentral.sprite = this.sprite_para_figura_central_melody;

        StartCoroutine(esperarSituacaoMelodyTerminarParaDarPlayNaSituacaoRegras());
        //inicio uma thread para assim que o arquivo de audio com a fala da melody parar, o da regras comecar
    }

    private void comecarArquivoDeAudioRegrasEMudarFiguraCentral()
    {
        //hora de comecar o outro audio!
        this.arquivoAudioSituacaoAtualRegras.Play();
        arquivoDeAudioQueEstaTocandoAgoraEhmelodyOUregras = "regras";

        //mudar figura central
        SpriteRenderer spriteRendererFiguraCentral =
            GameObject.Find("figuraCentralSituacaoAtualHougaii").GetComponent<SpriteRenderer>(); // we are accessing the SpriteRenderer that is attached to the Gameobject
        spriteRendererFiguraCentral.sprite = this.sprite_para_figura_central_regras;

        //falta comecar a corotina que passa para a proxima tela assim que acaba esse contador
        StartCoroutine(esperarSituacaoRegrasTerminarParaPassarParaProximaTela());
    }

    public void recomecarAudioSituacaoAtual()
    {

        if (this.arquivoAudioSituacaoAtualRegras.isPlaying == true)
        {
            //significa que o usuario apertou o botao de recomecar o audio durante o audio de regras. entao devemos 
            //alem de botar Play() no audio da melody, comecar a corotina do audio das regras
            //a corotina ja ativada que estava esperando o audio das regras terminar deve ser parada sem fazer nada!
            esperarSituacaoRegrasTerminarParaPassarParaProximaTelaDeveriaTerminarSemFazerNada = true;
            this.pararTodosOsAudios();
            comecarArquivoDeAudioDaMelodyEMudarFiguraCentral();
        }
        else
        {
            //significa que o usuario apertou o botao de recomecar audio durante o audio da melody. entao devemos
            //apenas reiniciar o arquivo de audio e parar o audio das regras. 
            //Nao precisa reiniciar a corotina que espera o audio acabar, nem
            //alterar booleano arquivoDeAudioQueEstaTocandoAgoraEhmelodyOUregras e nem mudar a figura central
            this.arquivoAudioSituacaoAtualMelody.Play();
        }
    }

    //assim que o arquivo de audio com a fala da melody parar, o da regras deve comecar
    IEnumerator esperarSituacaoRegrasTerminarParaPassarParaProximaTela()
    {
        esperarSituacaoRegrasTerminarParaPassarParaProximaTelaDeveriaTerminarSemFazerNada = false;
        while (this.usuarioEstaDentroDeLetsJam == true ||
                (this.usuarioEstaDentroDeLetsJam == false && (this.arquivoAudioSituacaoAtualRegras.isPlaying == true || this.arquivoAudioSituacaoAtualMelody.isPlaying == true)))
        {
            yield return new WaitForSeconds(.1f);
        }

        if (esperarSituacaoRegrasTerminarParaPassarParaProximaTelaDeveriaTerminarSemFazerNada == false)
        {
            //a funcao recomecarAudioSituacaoAtual() pode alterar esse booleano!
            passarParaProximaTela();
        }
        else
        {
            //a corotina termina e n faz nada
        }
        
    }

    private void passarParaProximaTela()
    {
        //desaparecer a tela atual...
        PopupWindowBehavior telaSituacaoHougaii = GameObject.Find("telaSituacaoHougaii").GetComponent<PopupWindowBehavior>();
        telaSituacaoHougaii.irParaPosicaoDeDesaparecer();

        //aparecer proxima tela
        PopupWindowBehavior telaEscolhaEtosanHougaii = GameObject.Find("telaEscolhaEtosanHougaii").GetComponent<PopupWindowBehavior>();
        telaEscolhaEtosanHougaii.voltarAPosicaoInicial();
        PopupWindowBehavior escolhaEtosanHougaiiTexto = GameObject.Find("escolhaEtosanHougaiiTexto").GetComponent<PopupWindowBehavior>();
        escolhaEtosanHougaiiTexto.voltarAPosicaoInicial();
        GUIBarScript barra_afeicao_melody = GameObject.Find("barra_afeicao_melody").GetComponent<GUIBarScript>();
        barra_afeicao_melody.voltarAPosicaoInicial();
        GUIBarScript barra_bondade = GameObject.Find("barra_bondade").GetComponent<GUIBarScript>();
        barra_bondade.voltarAPosicaoInicial();


        //e dizer ao ModoHougaii qual a tela que deveria ser mostrada apos o letsjam
        GameObject modoHougaii = GameObject.Find("Main Camera");
        ModoHougaii modoHougaiiTipoReal = modoHougaii.GetComponent<ModoHougaii>();
        modoHougaiiTipoReal.setaoFecharLetsJamMostrarQualTela("telaEscolhaEtosanHougaii");

        //passar para a proxima tela a situacao atual
        telaEscolhaEtosanHougaii telaEscolhaEtosanHougaiiComTipoReal = GameObject.Find("telaEscolhaEtosanHougaii").GetComponent<telaEscolhaEtosanHougaii>();
        telaEscolhaEtosanHougaiiComTipoReal.prepararNovaTelaEscolhaEtosan(this.situacaoAtual); //vamos inicializar a tela seguinte com tudo que ela tem direito

        this.arquivoAudioSituacaoAtualMelodyEstaPausado = false;
        this.arquivoAudioSituacaoAtualRegrasEstaPausado = false;
    }

    //funcao chamada por botaoRecomecarAudioSituacaoAtual na tela da decisao do etosan senao dava bug nos audios(eh como se o audio da melody ja comecasse playando)
    public void pararTodosOsAudios()
    {
        this.arquivoAudioSituacaoAtualMelody.Stop();
        this.arquivoAudioSituacaoAtualRegras.Stop();
        this.arquivoAudioSituacaoAtualMelodyEstaPausado = false;
        this.arquivoAudioSituacaoAtualRegrasEstaPausado = false;
    }
}
