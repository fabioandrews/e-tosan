using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class telaFimDeJogoModoHougaii : MonoBehaviour
{
    private int quantasMoedasObtidasCarinhasAfeicaoMelody;
    private int quantasMoedasObtidasCarinhasBondade;
    private int quantasMoedasObtidas; //moedas da partida + moedas da afeicao + moedas da bondade

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void mudarTextosDATelaFimDeJogoModoHougaii()
    {
        //primeiro reiniciar atributos
        quantasMoedasObtidasCarinhasAfeicaoMelody = 0;
        quantasMoedasObtidasCarinhasBondade = 0;
        quantasMoedasObtidas = 0;

        //as carinhas de afeicao e bondade
        this.alterarTextosQuantidadeCarinhasAfeicaoBondade();

        //as moedas obtidas pelas carinhas
        this.alterarMoedasObtidasPelasCarinhasAfeicaoEBondadeEOsTextos();

        //as moedas obtidas durante o jogo serao juntadas as moedas obtidas com as carinhas
        this.alterarMoedasObtidasEOTexto();

        //o score
        this.alterarTextoPontuacaoFinal();
    }

    private void alterarTextosQuantidadeCarinhasAfeicaoBondade()
    {
        ModoHougaii modoHougaii = GameObject.Find("Main Camera").GetComponent<ModoHougaii>();
        int quantasCarinhasAfeicaoMelodyEstaoCheias = modoHougaii.getquantasCarinhasAfeicaoMelodyEstaoCheias();
        int quantasCarinhasBondadeEstaoCheias = modoHougaii.getquantasCarinhasBondadeEstaoCheias();

        //carinhas afeicao
        Text quantasCarinhasAfeicaoMelodyCheiasFimDeJogo = GameObject.Find("quantasCarinhasAfeicaoMelodyCheiasFimDeJogo").GetComponent<Text>();
        string textoquantasCarinhasAfeicaoMelodyCheiasFimDeJogo = quantasCarinhasAfeicaoMelodyCheiasFimDeJogo.text;
        //primeiro vou remover tudo que tiver de numero no texto
        textoquantasCarinhasAfeicaoMelodyCheiasFimDeJogo = Regex.Replace(textoquantasCarinhasAfeicaoMelodyCheiasFimDeJogo, @"[0-9\-]", string.Empty);
        if (quantasCarinhasAfeicaoMelodyEstaoCheias < 10)
        {
            textoquantasCarinhasAfeicaoMelodyCheiasFimDeJogo = textoquantasCarinhasAfeicaoMelodyCheiasFimDeJogo + "0" + quantasCarinhasAfeicaoMelodyEstaoCheias.ToString();
        }
        else
        {
            textoquantasCarinhasAfeicaoMelodyCheiasFimDeJogo = textoquantasCarinhasAfeicaoMelodyCheiasFimDeJogo + quantasCarinhasAfeicaoMelodyEstaoCheias.ToString();
        }
        quantasCarinhasAfeicaoMelodyCheiasFimDeJogo.text = textoquantasCarinhasAfeicaoMelodyCheiasFimDeJogo;

        //carinhas bondade
        Text quantasCarinhasBondadeCheiasFimDeJogo = GameObject.Find("quantasCarinhasBondadeCheiasFimDeJogo").GetComponent<Text>();
        string textoquantasCarinhasBondadeCheiasFimDeJogo = quantasCarinhasBondadeCheiasFimDeJogo.text;
        //primeiro vou remover tudo que tiver de numero no texto
        textoquantasCarinhasBondadeCheiasFimDeJogo = Regex.Replace(textoquantasCarinhasBondadeCheiasFimDeJogo, @"[0-9\-]", string.Empty);
        if (quantasCarinhasBondadeEstaoCheias < 10)
        {
            textoquantasCarinhasBondadeCheiasFimDeJogo = textoquantasCarinhasBondadeCheiasFimDeJogo + "0" + quantasCarinhasBondadeEstaoCheias.ToString();
        }
        else
        {
            textoquantasCarinhasBondadeCheiasFimDeJogo = textoquantasCarinhasBondadeCheiasFimDeJogo + quantasCarinhasBondadeEstaoCheias.ToString();
        }
        quantasCarinhasBondadeCheiasFimDeJogo.text = textoquantasCarinhasBondadeCheiasFimDeJogo;
    }

    private void alterarMoedasObtidasPelasCarinhasAfeicaoEBondadeEOsTextos()
    {
        //ate agora esta assim: cada carinha bondade dah 5 moedas e cara carinha melody dah 1 moeda
        ModoHougaii modoHougaii = GameObject.Find("Main Camera").GetComponent<ModoHougaii>();
        int quantasCarinhasAfeicaoMelodyEstaoCheias = modoHougaii.getquantasCarinhasAfeicaoMelodyEstaoCheias();
        int quantasCarinhasBondadeEstaoCheias = modoHougaii.getquantasCarinhasBondadeEstaoCheias();

        this.quantasMoedasObtidasCarinhasBondade = 5 * quantasCarinhasBondadeEstaoCheias;
        this.quantasMoedasObtidasCarinhasAfeicaoMelody = 1 * quantasCarinhasAfeicaoMelodyEstaoCheias;

        //moedas afeicao melody
        if (quantasMoedasObtidasCarinhasAfeicaoMelody > 0)
        {
            //vamos tornar estes componentes das moedas visiveis, ja que eles deveriam aparecer. VEJA O ELSE abaixo e vc entenderah
            //deveriamos fazer desaparecer o texto das moedas e o icone e o mais
            GameObject.Find("iconeMoedaMelodyAfeicaoMelody").GetComponent<Renderer>().enabled = true;
            GameObject.Find("sinalMaisMoedasDasCarinhasAfeicaoMelody").GetComponent<Renderer>().enabled = true;
            GameObject.Find("moedasObtidasDasCarinhasAfeicaoMelody").GetComponent<Text>().enabled = true;

            Text moedasObtidasDasCarinhasAfeicaoMelody = GameObject.Find("moedasObtidasDasCarinhasAfeicaoMelody").GetComponent<Text>();
            string textomoedasObtidasDasCarinhasAfeicaoMelody = moedasObtidasDasCarinhasAfeicaoMelody.text;
            //primeiro vou remover tudo que tiver de numero no texto
            textomoedasObtidasDasCarinhasAfeicaoMelody = Regex.Replace(textomoedasObtidasDasCarinhasAfeicaoMelody, @"[0-9\-]", string.Empty);
            if (this.quantasMoedasObtidasCarinhasAfeicaoMelody < 10)
            {
                textomoedasObtidasDasCarinhasAfeicaoMelody = textomoedasObtidasDasCarinhasAfeicaoMelody + "0" + quantasMoedasObtidasCarinhasAfeicaoMelody.ToString();
            }
            else
            {
                textomoedasObtidasDasCarinhasAfeicaoMelody = textomoedasObtidasDasCarinhasAfeicaoMelody + quantasMoedasObtidasCarinhasAfeicaoMelody.ToString();
            }
            moedasObtidasDasCarinhasAfeicaoMelody.text = textomoedasObtidasDasCarinhasAfeicaoMelody;
        }
        else
        {
            //deveriamos fazer desaparecer o texto das moedas e o icone e o mais
            GameObject.Find("iconeMoedaMelodyAfeicaoMelody").GetComponent<Renderer>().enabled = false;
            GameObject.Find("sinalMaisMoedasDasCarinhasAfeicaoMelody").GetComponent<Renderer>().enabled = false;
            GameObject.Find("moedasObtidasDasCarinhasAfeicaoMelody").GetComponent<Text>().enabled = false;
        }


        //moedas bondade
        if (quantasMoedasObtidasCarinhasBondade > 0)
        {
            //vamos tornar estes componentes das moedas visiveis, ja que eles deveriam aparecer. VEJA O ELSE abaixo e vc entenderah
            //deveriamos fazer desaparecer o texto das moedas e o icone e o mais
            GameObject.Find("iconeMoedaMelodyBondade").GetComponent<Renderer>().enabled = true;
            GameObject.Find("sinalMaisMoedasDasCarinhasBondade").GetComponent<Renderer>().enabled = true;
            GameObject.Find("moedasObtidasDasCarinhasBondade").GetComponent<Text>().enabled = true;

            Text moedasObtidasDasCarinhasBondade = GameObject.Find("moedasObtidasDasCarinhasBondade").GetComponent<Text>();
            string textomoedasObtidasDasCarinhasBondade = moedasObtidasDasCarinhasBondade.text;
            //primeiro vou remover tudo que tiver de numero no texto
            textomoedasObtidasDasCarinhasBondade = Regex.Replace(textomoedasObtidasDasCarinhasBondade, @"[0-9\-]", string.Empty);
            if (this.quantasMoedasObtidasCarinhasBondade < 10)
            {
                textomoedasObtidasDasCarinhasBondade = textomoedasObtidasDasCarinhasBondade + "0" + quantasMoedasObtidasCarinhasBondade.ToString();
            }
            else
            {
                textomoedasObtidasDasCarinhasBondade = textomoedasObtidasDasCarinhasBondade + quantasMoedasObtidasCarinhasBondade.ToString();
            }
            moedasObtidasDasCarinhasBondade.text = textomoedasObtidasDasCarinhasBondade;
        }
        else
        {
            //deveriamos fazer desaparecer o texto das moedas e o icone e o mais
            GameObject.Find("iconeMoedaMelodyBondade").GetComponent<Renderer>().enabled = false;
            GameObject.Find("sinalMaisMoedasDasCarinhasBondade").GetComponent<Renderer>().enabled = false;
            GameObject.Find("moedasObtidasDasCarinhasBondade").GetComponent<Text>().enabled = false;
        } 
    }

    private void alterarMoedasObtidasEOTexto()
    {
        ModoHougaii modoHougaii = GameObject.Find("Main Camera").GetComponent<ModoHougaii>();
        int moedasObtidasDuranteOJogo = modoHougaii.getquantasMoedasObtidasDuranteOJogo();
        this.quantasMoedasObtidas = moedasObtidasDuranteOJogo + this.quantasMoedasObtidasCarinhasAfeicaoMelody + this.quantasMoedasObtidasCarinhasBondade;

        Text moedas_obtidas = GameObject.Find("moedas_obtidas").GetComponent<Text>();
        string textomoedas_obtidas = moedas_obtidas.text;
        //primeiro vou remover tudo que tiver de numero no texto
        textomoedas_obtidas = Regex.Replace(textomoedas_obtidas, @"[0-9\-]", string.Empty);
        if (this.quantasMoedasObtidas < 10)
        {
            textomoedas_obtidas = textomoedas_obtidas + "00" + this.quantasMoedasObtidas.ToString();
        }
        else if(this.quantasMoedasObtidas < 100)
        {
            textomoedas_obtidas = textomoedas_obtidas + "0" + this.quantasMoedasObtidas.ToString();
        }
        else
        {
            textomoedas_obtidas = textomoedas_obtidas + this.quantasMoedasObtidas.ToString();
        }
        moedas_obtidas.text = textomoedas_obtidas;
    }

    private void alterarTextoPontuacaoFinal()
    {
        ModoHougaii modoHougaii = GameObject.Find("Main Camera").GetComponent<ModoHougaii>();
        int pontuacaoFinal = modoHougaii.getscoreDaPartida();

        Text pontuacaoFinalModoHougaii = GameObject.Find("pontuacaoFinalModoHougaii").GetComponent<Text>();
        string textopontuacaoFinalModoHougaii = pontuacaoFinalModoHougaii.text;
        //primeiro vou remover tudo que tiver de numero no texto
        textopontuacaoFinalModoHougaii = Regex.Replace(textopontuacaoFinalModoHougaii, @"[0-9\-]", string.Empty);
        if (pontuacaoFinal < 10)
        {
            textopontuacaoFinalModoHougaii = textopontuacaoFinalModoHougaii + "0000" + pontuacaoFinal.ToString();
        }
        else if (this.quantasMoedasObtidas < 100)
        {
            textopontuacaoFinalModoHougaii = textopontuacaoFinalModoHougaii + "000" + pontuacaoFinal.ToString();
        }
        else if (this.quantasMoedasObtidas < 1000)
        {
            textopontuacaoFinalModoHougaii = textopontuacaoFinalModoHougaii + "00" + pontuacaoFinal.ToString();
        }
        else if (this.quantasMoedasObtidas < 10000)
        {
            textopontuacaoFinalModoHougaii = textopontuacaoFinalModoHougaii + "0" + pontuacaoFinal.ToString();
        }
        else
        {
            textopontuacaoFinalModoHougaii = textopontuacaoFinalModoHougaii + pontuacaoFinal.ToString();
        }
        pontuacaoFinalModoHougaii.text = textopontuacaoFinalModoHougaii;

    }
}
