using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine.UI;

/*
 * Como usar o radio: Basta ter um objeto com esse script(aconselho o objeto empty que engloba todos os componentes do radio)
 * e em alguma classe(tipo uma classe de um botão que deveria ser apertado para abrir o radio), faça:
 * RadioLetsJam radioLetsJam = GameObject.Find("radioLetsJam").GetComponent<RadioLetsJam>();
        radioLetsJam.voltarAPosicaoInicial();
        string[] arquivosDoRadio = new string[] { "hougaii.wav" }; //aqui devem estar os nomes de todos os arquivos que o raido deve passar

        radioLetsJam.setarArquivosDoRadio(arquivosDoRadio, "Assets/modohougaii/audiosModoHougaii/letsjam");
        radioLetsJam.PlayCurrent(); //para playar a musica 1 dessa lista de musicas
 */

public class RadioLetsJam : MonoBehaviour
{

    public enum SeekDirection {Forward, Backward};

    private AudioSource source;
    public List<AudioClip> clips = new List<AudioClip>();

    [SerializeField]
    [HideInInspector]
    private int currentIndex = 0;

    private LinkedList<FileInfo> soundFiles;
    private List<string> validExtensions = new List<string> { ".ogg", ".wav" }; // Don't forget the "." i.e. "ogg" won't work - cause Path.GetExtension(filePath) will return .ext, not just ext.
    private string absolutePath; // relative path to where the app is running - change this to "./music" in your case

    private string[] nomesArquivosDoRadio; //quais sao os nomes dos arquivos que deveriam fazer parte do radio atualmente? Ex: formas_verbais_hougaii

   private int quantosClipsForamLoaded; //variavel que vai sendo aumentada a medida que loadFile() vai terminando. Serve para eu esperar ate as corotinas acabarem
    private bool aindaCarregandoClipes; //boleano relacionado com o atributo acima que muda para false quando a funcao ReloadSounds comeca e termina quando o ultimo arquivo de som em loadfile eh carregado

    // Use this for initialization
    void Start()
    {
        
    }
    
    public void setarArquivosDoRadio(string[] nomesArquivosDevemEstarNoRadio, string caminhoAteArquivos)
    {
        this.currentIndex = 0;
        absolutePath = caminhoAteArquivos;
        nomesArquivosDoRadio = nomesArquivosDevemEstarNoRadio;

        if (source == null) source = gameObject.AddComponent<AudioSource>();

        ReloadSounds();
    }

    //funcao para checar se todos os arquivos de audio ja foram carregados(preciso dele pq carregar arquivos de audio eh feito em corotinas)
    public bool terminouDeCarregarClips()
    {
        if ((this.clips.Count == this.quantosClipsForamLoaded) && (this.aindaCarregandoClipes == false))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int tamanhoClips()
    {
        return clips.Count;
    }

    /*void OnGUI()
    {
        if (GUILayout.Button("Previous"))
        {
            Seek(SeekDirection.Backward);
            PlayCurrent();
        }
        if (GUILayout.Button("Play current"))
        {
            PlayCurrent();
        }
        if (GUILayout.Button("Next"))
        {
            Seek(SeekDirection.Forward);
            PlayCurrent();
        }
        if (GUILayout.Button("Reload"))
        {
            ReloadSounds();
        }
    }*/

    public void Seek(SeekDirection d)
    {
        if (d == SeekDirection.Forward)
            currentIndex = (currentIndex + 1) % clips.Count;
        else
        {
            currentIndex--;
            if (currentIndex < 0) currentIndex = clips.Count - 1;
        }
    }

    public void PlayCurrent()
    {
        /*while (this.terminouDeCarregarClips() == false || currentIndex >= clips.Count)
        {
            System.Threading.Thread.Sleep(100);
        }

        source.clip = clips[currentIndex];
        source.Play();
        this.mudarNomeMusicaAtualRadioLetsJam();
        this.mudarTraducaoMusicaAtualRadioLetsJam();*/
        StartCoroutine(esperarTerminouDeCarregarClips());
    }

    IEnumerator esperarTerminouDeCarregarClips()
    {
        GameObject previous = GameObject.Find("previous");
        previous.SetActive(false);
        GameObject skip = GameObject.Find("skip");
        skip.SetActive(false);

        while (this.terminouDeCarregarClips() == false || currentIndex >= clips.Count)
        {
            yield return new WaitForSeconds(0.1f);
        }

        //depois de esperar que os clipes estejam carregados, eh finalmente hora de PlayCurrent() playar o audio
        source.clip = clips[currentIndex];
        source.Play();

        //botoes devem estar ativos
        previous.SetActive(true);
        skip.SetActive(true);

        this.mudarNomeMusicaAtualRadioLetsJam();
        this.mudarTraducaoMusicaAtualRadioLetsJam();
    }

    private void mudarNomeMusicaAtualRadioLetsJam()
    {
        string nomeMusicaAtualComWav = source.clip.name;
        string oQueDesejoRemover = ".wav";
        string nomeMusicaAtual = nomeMusicaAtualComWav.Remove(nomeMusicaAtualComWav.IndexOf(oQueDesejoRemover), oQueDesejoRemover.Length);

        Text nomeMusicaAtualRadioLetsJam = GameObject.Find("nomeMusicaAtualRadioLetsJam").GetComponent<Text>();
        nomeMusicaAtualRadioLetsJam.text = nomeMusicaAtual;
    }

    private void mudarTraducaoMusicaAtualRadioLetsJam()
    {
        string nomeMusicaAtualComWav = source.clip.name;
        string oQueDesejoRemover = ".wav";
        string nomeMusicaAtual = nomeMusicaAtualComWav.Remove(nomeMusicaAtualComWav.IndexOf(oQueDesejoRemover), oQueDesejoRemover.Length);

        //basta usar o nome da musica atual + "_traducao"
        string tagParaBuscarEmLangXml = nomeMusicaAtual + "_traducao";
        string traducao = MultiplasLinguagens.pegarTextoDaTag(tagParaBuscarEmLangXml); //pegar do xml o texto com a tag "continuar"  

        Text traducaoMusicaAtualRadioLetsJam = GameObject.Find("traducaoMusicaAtualRadioLetsJam").GetComponent<Text>();
        traducaoMusicaAtualRadioLetsJam.text = traducao;
    }

    public void stopCurrent()
    {
        source.Stop();
    }

    public void pauseCurrent()
    {
        source.Pause();
    }

    public void unPauseCurrent()
    {
        source.UnPause();
    }


    void ReloadSounds()
    {
        this.aindaCarregandoClipes = true;
        this.quantosClipsForamLoaded = 0; //vamos passar para a fase de carregando clips
        clips.Clear();
        soundFiles = new LinkedList<FileInfo>();
        // get all valid files
        var info = new DirectoryInfo(absolutePath);
        FileInfo[] fileInfos = info.GetFiles();
        for (int i = 0; i < fileInfos.Length; i++)
        {
            FileInfo umFileInfo = fileInfos[i];
            //Debug.Log(umFileInfo.Name);
            if (nomeArquivoEstaPresenteEmArquivosDoRadio(umFileInfo.Name) == true)
            {
                soundFiles.AddLast(umFileInfo);
            }      
        }
        /*soundFiles = info.GetFiles()
            .Where(f => IsValidFileType(f.Name))
            .ToArray();*/
        // and load them && nomeArquivoEstaPresenteEmArquivosDoRadio(f.Name)

        foreach (var s in soundFiles)
        {
            StartCoroutine(LoadFile(s.FullName));
        }
    }

    private bool nomeArquivoEstaPresenteEmArquivosDoRadio(string nomeDeUmArquivo)
    {
        for (int i = 0; i < this.nomesArquivosDoRadio.Length; i++)
        {
            if (nomeDeUmArquivo.CompareTo(this.nomesArquivosDoRadio[i]) == 0)
            {
                return true;
            }
        }

        return false;
    }

    bool IsValidFileType(string fileName)
    {
        return validExtensions.Contains(Path.GetExtension(fileName));
        // Alternatively, you could go fileName.SubString(fileName.LastIndexOf('.') + 1); that way you don't need the '.' when you add your extensions
    }

    IEnumerator LoadFile(string path)
    {
        WWW www = new WWW("file://" + path);
        print("loading " + path);

        AudioClip clip = www.GetAudioClip(false);
        while (!clip.isReadyToPlay)
            yield return www;

        print("done loading");
        clip.name = Path.GetFileName(path);
        clips.Add(clip);
        Debug.Log("tamanho de clips:" + clips.Count);

        this.quantosClipsForamLoaded = this.quantosClipsForamLoaded + 1;

        if(this.clips.Count == this.quantosClipsForamLoaded)
        {
            this.aindaCarregandoClipes = false;
        }

    }

    /*private void LoadFile(string path)
    {
        WWW www = new WWW("file://" + path);
        AudioClip clip = www.GetAudioClip(false);
        clip.name = Path.GetFileName(path);
        clips.Add(clip);
        this.quantosClipsForamLoaded = this.quantosClipsForamLoaded + 1;

    }*/
}
