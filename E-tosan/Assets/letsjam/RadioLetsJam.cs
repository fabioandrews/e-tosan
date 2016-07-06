using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class RadioLetsJam : MonoBehaviour
{

    public enum SeekDirection {Forward, Backward};

    public AudioSource source;
    public List<AudioClip> clips = new List<AudioClip>();

    [SerializeField]
    [HideInInspector]
    private int currentIndex = 0;

    private LinkedList<FileInfo> soundFiles;
    private List<string> validExtensions = new List<string> { ".ogg", ".wav" }; // Don't forget the "." i.e. "ogg" won't work - cause Path.GetExtension(filePath) will return .ext, not just ext.
    private string absolutePath; // relative path to where the app is running - change this to "./music" in your case

    private string[] nomesArquivosDoRadio; //quais sao os nomes dos arquivos que deveriam fazer parte do radio atualmente? Ex: formas_verbais_hougaii
    Vector3 posicaoInicial;

    private bool terminouDeCarregarClips; //variavel que fica false quando sistema comeca a preencher a variavel clips e fica true quando clips ja esta prontinho

    // Use this for initialization
    void Start()
    {
        posicaoInicial = transform.localPosition;
        transform.localPosition = new Vector3(1000, 1000);
    }

    public void voltarAPosicaoInicial()
    {
        transform.localPosition = posicaoInicial;
    }

    public void irParaPosicaoDeDesaparecer()
    {
        transform.localPosition = new Vector3(1000, 1000);
    }

    public void setarArquivosDoRadio(string[] nomesArquivosDevemEstarNoRadio, string caminhoAteArquivos)
    {
        absolutePath = caminhoAteArquivos;
        nomesArquivosDoRadio = nomesArquivosDevemEstarNoRadio;

        if (source == null) source = gameObject.AddComponent<AudioSource>();

        ReloadSounds();
    }

    public bool getTerminouDeCarregarClips()
    {
        return terminouDeCarregarClips;
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
        source.clip = clips[currentIndex];
        source.Play();
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
        terminouDeCarregarClips = false; //vamos passar para a fase de carregando clips
        clips.Clear();
        soundFiles = new LinkedList<FileInfo>();
        // get all valid files
        var info = new DirectoryInfo(absolutePath);
        FileInfo[] fileInfos = info.GetFiles();
        for (int i = 0; i < fileInfos.Length; i++)
        {
            FileInfo umFileInfo = fileInfos[i];
            Debug.Log(umFileInfo.Name);
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

        terminouDeCarregarClips = true;
        PlayCurrent();
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
    }
}
