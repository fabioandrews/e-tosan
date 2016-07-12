using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine.UI;

public class MultiplasLinguagens : MonoBehaviour
{
    //coisas necessarias para dar suporte a multiplas linguagens
    private static Lang LMan;
    private string currentLang = "English"; //linguagem padrao do aplicativo

    public void OnEnable()
    {
        /*
    Initialize the Lang class by providing a path to the desired language XML file, a default language
    and a boolean to indicate if we are operating on an XML file located from a downloaded resource or local.
    True if XML resource is on the web, false if local
 
    If initializing from a web based XML resource you'll need to supply the text of the downloaded resource in placed
    of the path.
 
    web example:
    var wwwXML : WWW = new WWW("http://www.exampleURL.com/lang.xml");
    yield wwwXML;
     
    LMan = new Lang(wwwXML.text, currentLang, true);
    */
        LMan = new Lang(Path.Combine(Application.dataPath, "MultiplasLinguagens/lang.xml"), currentLang, false);
    }

    void Start()
    {
        if (UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == "modohougaii")
        {
            this.mudarTextoCenamodohougaii();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static string pegarTextoDaTag(string tagParaBuscarEmLangxml)
    {
        string texto = LMan.getString(tagParaBuscarEmLangxml);
        return texto;
    }

    private void mudarTextoCenamodohougaii()
    {
        string continuar = LMan.getString("continuar"); //pegar do xml o texto com a tag "continuar"  
        GameObject gameObjectContinuar = GameObject.Find("continuar");
        Text textoContinuar = gameObjectContinuar.GetComponent<Text>();
        textoContinuar.text = continuar;

        string menu_principal = LMan.getString("menu_principal");
        GameObject gameObjectmenuprincipal = GameObject.Find("menu principal");
        Text textomenu_principal = gameObjectmenuprincipal.GetComponent<Text>();
        textomenu_principal.text = menu_principal;

        string tem_certeza_que_quer_ir_ao_menu_principal = LMan.getString("tem_certeza_que_quer_ir_ao_menu_principal");
        GameObject gameObjecttem_certeza_que_quer_ir_ao_menu_principal = GameObject.Find("tem_certeza_que_quer_ir_ao_menu_principal");
        Text textotem_certeza_que_quer_ir_ao_menu_principal = gameObjecttem_certeza_que_quer_ir_ao_menu_principal.GetComponent<Text>();
        textotem_certeza_que_quer_ir_ao_menu_principal.text = tem_certeza_que_quer_ir_ao_menu_principal;

        string sim = LMan.getString("sim");
        GameObject gameObjectsim = GameObject.Find("sim");
        Text textosim = gameObjectsim.GetComponent<Text>();
        textosim.text = sim;

        string nao = LMan.getString("nao");
        GameObject gameObjectnao = GameObject.Find("nao");
        Text textonao = gameObjectnao.GetComponent<Text>();
        textonao.text = nao;

        string pause = LMan.getString("pause");
        GameObject pause_text = GameObject.Find("pause_text");
        Text textopause = pause_text.GetComponent<Text>();
        textopause.text = pause;

        string verbos = LMan.getString("verbos");
        GameObject objetoverbos = GameObject.Find("verbos");
        Text textoverbos = objetoverbos.GetComponent<Text>();
        textoverbos.text = verbos;

        string formas_verbais = LMan.getString("formas_verbais");
        GameObject objetoformas_verbais = GameObject.Find("formas_verbais");
        Text textformas_verbais = objetoformas_verbais.GetComponent<Text>();
        textformas_verbais.text = formas_verbais;

        string lugares = LMan.getString("lugares");
        GameObject objetolugares = GameObject.Find("lugares");
        Text textlugares = objetolugares.GetComponent<Text>();
        textlugares.text = lugares;

        string subtitulo_letsjam = LMan.getString("subtitulo_letsjam");
        GameObject objetosubtitulo_letsjam = GameObject.Find("subtitulo_letsjam");
        Text textsubtitulo_letsjam = objetosubtitulo_letsjam.GetComponent<Text>();
        textsubtitulo_letsjam.text = subtitulo_letsjam;


    }
}

