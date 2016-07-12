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

    // Use this for initialization
    void Start ()
    {
        arquivoAudioSituacaoAtualMelody = gameObject.AddComponent<AudioSource>();
        arquivoAudioSituacaoAtualRegras = gameObject.AddComponent<AudioSource>();
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

        string ondeEstaoOsArquivosDeAudioDeSituacoesDoLetsJam =
            modoHougaiiComTipoReal.getondeEstaoOsArquivosDeAudioDeSituacoesDoLetsJam();

        soundFiles = new LinkedList<FileInfo>();
        // get all valid files
        var info = new DirectoryInfo(ondeEstaoOsArquivosDeAudioDeSituacoesDoLetsJam);
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


        StartCoroutine(LoadFile(ondeEstaoOsArquivosDeAudioDeSituacoesDoLetsJam + "/" + this.situacaoAtual.getnomeArquivoWavMelody(),"melody"));
        StartCoroutine(LoadFile(ondeEstaoOsArquivosDeAudioDeSituacoesDoLetsJam + "/" + this.situacaoAtual.getnomeArquivoWavRegras(), "regras"));

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
            this.arquivoAudioSituacaoAtualMelody.clip = clip;
        }
        else
        {
            this.arquivoAudioSituacaoAtualRegras.clip = clip;
        }
    }
    

    private void comecarArquivoDeAudioDaMelodyEMudarFiguraCentral()
    {
        //hora de comecar algum arquivo de audio: o da melody
        this.arquivoAudioSituacaoAtualMelody.Play();
        arquivoDeAudioQueEstaTocandoAgoraEhmelodyOUregras = "melody";

        StartCoroutine(esperarSituacaoMelodyTerminarParaDarPlayNaSituacaoRegras());
        //inicio uma thread para assim que o arquivo de audio com a fala da melody parar, o da regras comecar
    }

    private void comecarArquivoDeAudioRegrasEMudarFiguraCentral()
    {
        //hora de comecar o outro audio!
        this.arquivoAudioSituacaoAtualRegras.Play();
        arquivoDeAudioQueEstaTocandoAgoraEhmelodyOUregras = "regras";
    }
}
