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
    private string arquivoDeAudioQueEstaTocandoAgoraEhmelodyOUregras; //pode ser "melody" ou "regras"
    private LinkedList<FileInfo> soundFiles; //necessario para quando for carregar arquivos de audio do diretorio

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
            }
            else if(arquivoDeAudioQueEstaTocandoAgoraEhmelodyOUregras != null)
            {
                this.arquivoAudioSituacaoAtualRegras.Pause();
            }
        }
        else
        {
            //hora de continuar o arquivo de audio da melody/regras ou comecar

            if (this.arquivoAudioSituacaoAtualMelody.isPlaying == false && this.arquivoAudioSituacaoAtualRegras.isPlaying == false)
            {
                this.comecarArquivoDeAudioDaMelodyEMudarFiguraCentral();
            }
            else
            {
                //hora de despausar(continuar) algum arquivo de audio... qual o que estava passando?
                if (arquivoDeAudioQueEstaTocandoAgoraEhmelodyOUregras.CompareTo("melody") == 0)
                {
                    this.arquivoAudioSituacaoAtualMelody.UnPause();
                }
                else
                {
                    this.arquivoAudioSituacaoAtualRegras.UnPause();
                }
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

        soundFiles = new LinkedList<FileInfo>();
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
            this.arquivoAudioSituacaoAtualRegras.Stop();
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
        while (this.usuarioEstaDentroDeLetsJam == true ||
                (this.usuarioEstaDentroDeLetsJam == false && (this.arquivoAudioSituacaoAtualRegras.isPlaying == true || this.arquivoAudioSituacaoAtualMelody.isPlaying == true)))
        {
            yield return new WaitForSeconds(.1f);
        }

        this.passarParaProximaTela();
    }

    private void passarParaProximaTela()
    {
        //desaparecer a tela atual...
        PopupWindowBehavior telaSituacaoHougaii = GameObject.Find("telaSituacaoHougaii").GetComponent<PopupWindowBehavior>();
        telaSituacaoHougaii.irParaPosicaoDeDesaparecer();

        //aparecer proxima tela
        PopupWindowBehavior telaEscolhaEtosanHougaii = GameObject.Find("telaEscolhaEtosanHougaii").GetComponent<PopupWindowBehavior>();
        telaEscolhaEtosanHougaii.voltarAPosicaoInicial();
    } 
}
