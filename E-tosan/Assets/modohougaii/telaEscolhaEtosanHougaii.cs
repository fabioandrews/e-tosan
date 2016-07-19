using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Text.RegularExpressions;

public class telaEscolhaEtosanHougaii : MonoBehaviour
{
    private SituacaoModoHougaii situacaoAtual;
    private bool barraAfeicaoMelodyTerminouDeSubirOuDescer;
    private bool barraBondadeTerminouDeSubirOuDescer;
    //so depois destes bool atualizarem eh que o jogo continua e uma nova situacao eh selecionada

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
	}

    //funcao chamada por TelaSituacaoHougaii apos terminar o audio das duas situacoes
    //faz tudo que a tela deveria fazer no inicio dela: setar score, mudar texto dos botoes etc
    public void prepararNovaTelaEscolhaEtosan(SituacaoModoHougaii situacaoAtual)
    {
        this.situacaoAtual = situacaoAtual;

        //vamos mudar o texto nos botoes de acordo com a situacao atual
        this.mudarTextosBotoesTelaEscolhaEtosan();
        this.mudarscoreTexto();

        barraAfeicaoMelodyTerminouDeSubirOuDescer = false;
        barraBondadeTerminouDeSubirOuDescer = false;

    //para testar o funcionamento das barras de afeicao e bondade diminuindo seus valores, basta descomentar a linha seguinte
    /*GUIBarScript barra_afeicao_melody = GameObject.Find("barra_afeicao_melody").GetComponent<GUIBarScript>();
    barra_afeicao_melody.Value = (float)1;
    StartCoroutine(fazerBarraAumentarOuDiminuirGradativamente(barra_afeicao_melody, (float)0.01));

    GUIBarScript barra_bondade = GameObject.Find("barra_bondade").GetComponent<GUIBarScript>();
    barra_bondade.Value = (float)1;
    StartCoroutine(fazerBarraAumentarOuDiminuirGradativamente(barra_bondade, (float)0.51));*/
}


    private void mudarTextosBotoesTelaEscolhaEtosan()
    {
        string textoBotao1 = this.situacaoAtual.getduasRespostasDoEtosan().First.Value;
        string textoBotao2 = this.situacaoAtual.getduasRespostasDoEtosan().Last.Value;
        GameObject.Find("botaoTelaEscolhaEtosan1").GetComponent<botaoTelaEscolhaEtosan>().mudarTextoRelacionado(textoBotao1, "botaoTelaEscolhaEtosan1");
        GameObject.Find("botaoTelaEscolhaEtosan2").GetComponent<botaoTelaEscolhaEtosan>().mudarTextoRelacionado(textoBotao2, "botaoTelaEscolhaEtosan2");
    }

    private void mudarscoreTexto()
    {
        ModoHougaii modoHougaii = GameObject.Find("Main Camera").GetComponent<ModoHougaii>();
        int score = modoHougaii.getscoreDaPartida();

        Text objetoScoreTexto = GameObject.Find("scoreTexto").GetComponent<Text>();
        string textoScoreTexto = objetoScoreTexto.text;

        //primeiro vou remover tudo que tiver de numero no texto de score
        textoScoreTexto  = Regex.Replace(textoScoreTexto, @"[0-9\-]", string.Empty);

        //agora adicionar o novo score na string
        if (score < 10)
        {
            textoScoreTexto = textoScoreTexto + "0000";
        }
        else if (score < 100)
        {
            textoScoreTexto = textoScoreTexto + "000";
        }
        else if (score < 1000)
        {
            textoScoreTexto = textoScoreTexto + "00";
        }
        else if (score < 10000)
        {
            textoScoreTexto = textoScoreTexto + "0";
        }

        textoScoreTexto = textoScoreTexto + score.ToString();

        //agora mudar o objeto
        objetoScoreTexto.text = textoScoreTexto;
    }

    //funcao chamada externamente pelo botaoTelaEscolhaEtosan
    public void usuarioEscolheuUmaResposta(string respostaUsuario)
    {
        //vamos ver se eh a resposta correta e se sim, marcar pontos se nao diminuir
        String respostaCorreta = this.situacaoAtual.getrespostaCorretaDoEtosan();

        int scoreAntigo = GameObject.Find("Main Camera").GetComponent<ModoHougaii>().getscoreDaPartida();
        int scoreAtual = 0;

        if (respostaCorreta.CompareTo(respostaUsuario) == 0)
        {
            //o usuario escolheu a resposta correta!
            scoreAtual = scoreAntigo + 100;
        }
        else
        {
            //resposta errada!
            if (scoreAntigo >= 50)
            {
                scoreAtual = scoreAntigo - 50;
            }
            else
            {
                scoreAtual = 0;
            }
        }

        ModoHougaii modoHougaii = GameObject.Find("Main Camera").GetComponent<ModoHougaii>();
        modoHougaii.setscoreDaPartida(scoreAtual);
        this.executarAnimacaoScoreAumentaOuDiminui(scoreAntigo, scoreAtual);
        this.mudarscoreTexto();

        //agora vamos fazer as animacoes das barrinhas e carinhas dos medidores da parte de baixo da tela
        
        bool usuarioErrouDePropositoParaFazerMelodyFeliz = this.usuarioErrouDePropositoParaFazerAMelodyFeliz(respostaUsuario);

        this.encherOuDiminuirBarraAfeicaoMelody(respostaUsuario);
        this.encherOuDiminuirBarraBondade(respostaUsuario);

        int percentualCarinhaAfeicaoMelodyAntigo = modoHougaii.getpercentualCarinhaAfeicaoMelody();
        int percentualCarinhaBondadeAntigo = modoHougaii.getpercentualCarinhaBondade();
        int quantasCarinhasAfeicaoMelodyEstaoCheiasAntigo = modoHougaii.getquantasCarinhasAfeicaoMelodyEstaoCheias();
        int quantasCarinhasBondadeEstaoCheiasAntigo = modoHougaii.getquantasCarinhasBondadeEstaoCheias();


        modoHougaii.encherPercentualCarinhaAfeicaoMelodyComBaseNoPercentualDaBarrinha();
        if (usuarioErrouDePropositoParaFazerMelodyFeliz == true)
        {
            //O USUARIO ERROU DE PROPOSITO PARA FAZER A MELODY FELIZ! VAMOS REDUZIR UMA CARINHA DE BONDADE DELE
            //LA NO ModoHougaii
            modoHougaii.removerUmaCarinhaBondadeEEsvaziarCarinhaAtual();
        }
        else
        {
            modoHougaii.encherPercentualCarinhaBondadeComBaseNoPercentualDaBarrinha();
        }

        int percentualCarinhaAfeicaoMelodyNovo = modoHougaii.getpercentualCarinhaAfeicaoMelody();
        int percentualCarinhaBondadeNovo = modoHougaii.getpercentualCarinhaBondade();
        this.executarAnimacaoAumentarPercentualCarinhaAfeicaoMelody(percentualCarinhaAfeicaoMelodyAntigo, percentualCarinhaAfeicaoMelodyNovo, quantasCarinhasAfeicaoMelodyEstaoCheiasAntigo, modoHougaii);
        this.executarAnimacaoAumentarPercentualCarinhaBondade(percentualCarinhaBondadeAntigo, percentualCarinhaBondadeNovo, usuarioErrouDePropositoParaFazerMelodyFeliz, quantasCarinhasBondadeEstaoCheiasAntigo, modoHougaii);

        //deixar a quantidade de carinhas verde ou vermelho por um tempo dependendo se ela mudou ou nao
        //e fazer um sfx de boo ou yeah
        this.executarAnimacaoAlterarTextoQuantasCarinhasAfeicaoMelodyEstaoCheias(modoHougaii, quantasCarinhasAfeicaoMelodyEstaoCheiasAntigo);
        this.executarAnimacaoAlterarTextoQuantasCarinhasBondadeEstaoCheias(modoHougaii, usuarioErrouDePropositoParaFazerMelodyFeliz,quantasCarinhasBondadeEstaoCheiasAntigo);

        this.alterarTextoQuantasCarinhasAfeicaoMelodyEstaoCheias(modoHougaii);
        this.alterarTextoQuantasCarinhasBondadeEstaoCheias(modoHougaii);

        //agora, vamos escolher outra situacao ou escolher mais 4 situacoes. Isso eh trabalho do ModoHougaii
        //primeiro vamos fechar essa janela, mas soh apenas as barras de afeicao e bondade ficarem atualizadas

        StartCoroutine(esperarBarrasEncheremEDepoisExecutarProcedimentoPassarParaNovaSituacao(modoHougaii));
    }

    private IEnumerator esperarBarrasEncheremEDepoisExecutarProcedimentoPassarParaNovaSituacao(ModoHougaii modoHougaii)
    {
        while (this.barraAfeicaoMelodyTerminouDeSubirOuDescer == false && this.barraBondadeTerminouDeSubirOuDescer == false)
        {
            yield return new WaitForSeconds((float)0.5);
        }

        //vamos esperar soh mais um tempinho
        yield return new WaitForSeconds((float)1);
        //agora sim vamos mudar a situacao atual
        this.fazerEssaTelaEscolhaEtosanHougaiiDesaparecer();
        modoHougaii.definirNovaSituacaoAtualTelaHougaii();
    }

    private void encherOuDiminuirBarraAfeicaoMelody(string respostaUsuario)
    {
        string diminuirOuAumentarBarraAfeicaoMelody = "diminuir"; //pode ser diminuir ou aumentar
        if (respostaUsuario.CompareTo(this.situacaoAtual.getrespostaCorretaDoEtosan()) == 0)
        {
            //usuario respondeu corretamente, mas serah que essa resposta agrada a melody?
            bool respostaCorretaAgradaAMelody = this.situacaoAtual.getrespostaAgradaMelody();
            if (respostaCorretaAgradaAMelody == true)
            {
                diminuirOuAumentarBarraAfeicaoMelody = "aumentar";
            }
            else
            {
                diminuirOuAumentarBarraAfeicaoMelody = "diminuir";
            }
        }
        else
        {
            //usuario respondeu incorretamente, mas serah que essa resposta agrada a melody?
            bool respostaCorretaAgradaAMelody = this.situacaoAtual.getrespostaAgradaMelody();
            if (respostaCorretaAgradaAMelody == true)
            {
                diminuirOuAumentarBarraAfeicaoMelody = "diminuir"; //o usuario respodeu incorretamente, mas a resposta correta iria agradar a melody... entao ele vai desagradar a melody!
            }
            else
            {
                diminuirOuAumentarBarraAfeicaoMelody = "aumentar";
            }
        }

        ModoHougaii modoHougaii = GameObject.Find("Main Camera").GetComponent<ModoHougaii>();
        int percentualDaBarrinhaAfeicaoMelody = modoHougaii.getpercentualDaBarrinhaAfeicaoMelody();
        int novoPercentualDaBarrinhaAfeicaoMelody;
        if (diminuirOuAumentarBarraAfeicaoMelody.CompareTo("diminuir") == 0)
        {
            novoPercentualDaBarrinhaAfeicaoMelody = percentualDaBarrinhaAfeicaoMelody - 25;
            if (novoPercentualDaBarrinhaAfeicaoMelody < 0)
            {
                novoPercentualDaBarrinhaAfeicaoMelody = 0;
            }
        }
        else
        {
            novoPercentualDaBarrinhaAfeicaoMelody = percentualDaBarrinhaAfeicaoMelody + 25;
            if(novoPercentualDaBarrinhaAfeicaoMelody > 100)
            {
                novoPercentualDaBarrinhaAfeicaoMelody = 100;
            }
        }

        if (novoPercentualDaBarrinhaAfeicaoMelody != percentualDaBarrinhaAfeicaoMelody)
        {
            this.executarAnimacaoAumentarOuDiminuirBarrinhaAfeicaoMelody(novoPercentualDaBarrinhaAfeicaoMelody);
        }
        modoHougaii.setpercentualDaBarrinhaAfeicaoMelody(novoPercentualDaBarrinhaAfeicaoMelody);
    }

    private void encherOuDiminuirBarraBondade(string respostaUsuario)
    {
        string diminuirOuAumentarBarraBondade = "diminuir"; //pode ser diminuir ou aumentar
        if (respostaUsuario.CompareTo(this.situacaoAtual.getrespostaCorretaDoEtosan()) == 0)
        {
            //usuario respondeu corretamente
            diminuirOuAumentarBarraBondade = "aumentar";
        }
        else
        {
            diminuirOuAumentarBarraBondade = "diminuir";
        }

        ModoHougaii modoHougaii = GameObject.Find("Main Camera").GetComponent<ModoHougaii>();
        int percentualDaBarrinhaBondade = modoHougaii.getpercentualDaBarrinhaBondade();
        int novoPercentualDaBarrinhaBondade;
        if (diminuirOuAumentarBarraBondade.CompareTo("diminuir") == 0)
        {
            novoPercentualDaBarrinhaBondade = percentualDaBarrinhaBondade - 25;
            if (novoPercentualDaBarrinhaBondade < 0)
            {
                novoPercentualDaBarrinhaBondade = 0;
            }
        }
        else
        {
            novoPercentualDaBarrinhaBondade = percentualDaBarrinhaBondade + 25;
            if (novoPercentualDaBarrinhaBondade > 100)
            {
                novoPercentualDaBarrinhaBondade = 100;
            }
        }

        if (novoPercentualDaBarrinhaBondade != percentualDaBarrinhaBondade)
        {
            this.executarAnimacaoAumentarOuDiminuirBarrinhaBondade(novoPercentualDaBarrinhaBondade);
        }
        modoHougaii.setpercentualDaBarrinhaBondade(novoPercentualDaBarrinhaBondade);
    }


    private bool usuarioErrouDePropositoParaFazerAMelodyFeliz(String respostaUsuario)
    {
        if (respostaUsuario.CompareTo(this.situacaoAtual.getrespostaCorretaDoEtosan()) == 0)
        {
            return false;
        }
        else
        {
            //usuario respondeu incorretamente. Serah que essa resposta agrada a melody?
            bool respostaCorretaAgradaAMelody = this.situacaoAtual.getrespostaAgradaMelody();
            if (respostaCorretaAgradaAMelody == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void executarAnimacaoScoreAumentaOuDiminui(int scoreAntigo, int scoreAtual)
    {

    }
    private void executarAnimacaoAumentarOuDiminuirBarrinhaAfeicaoMelody(int percentualBarrinhaNovo)
    {
        GUIBarScript barra_afeicao_melody = GameObject.Find("barra_afeicao_melody").GetComponent<GUIBarScript>();
        float valorNaQualABarraTemDeParar = (float)percentualBarrinhaNovo / (float)100;
        //fazendo cruz-credo: 100 equivale a 1 entao percentualBarrinhaNovo equivale a x, tenho o x acima
        string nome_barra = "barra_afeicao_melody";
        StartCoroutine(fazerBarraAumentarOuDiminuirGradativamente(barra_afeicao_melody, valorNaQualABarraTemDeParar, nome_barra));
    }
    private void executarAnimacaoAumentarOuDiminuirBarrinhaBondade(int percentualBarrinhaNovo)
    {
        GUIBarScript barra_bondade = GameObject.Find("barra_bondade").GetComponent<GUIBarScript>();
        float valorNaQualABarraTemDeParar = (float)percentualBarrinhaNovo / (float)100;
        //fazendo cruz-credo: 100 equivale a 1 entao percentualBarrinhaNovo equivale a x, tenho o x acima
        string nome_barra = "barra_bondade";
        StartCoroutine(fazerBarraAumentarOuDiminuirGradativamente(barra_bondade, valorNaQualABarraTemDeParar, nome_barra));
    }
    //esse valor passado como parâmetro tem de estar entre 1 e 0.0. É bom se for multiplo de 10, colocar um digito 1 a mais no final.
    //Por exemplo: (float)0.31 darah 30% da barra
    IEnumerator fazerBarraAumentarOuDiminuirGradativamente(GUIBarScript barra, float valorNaQualABarraTemDeParar, string nomeBarra)
    {
        if (barra.Value > valorNaQualABarraTemDeParar)
        {
            //vamos diminuir a barra gradativamente
            while (barra.Value > valorNaQualABarraTemDeParar && barra.Value > 0.01)
            {
                //por algum motivo, o 0.01 eh o 0% nestas barrinhas. O 0 as vezes da bug no gradiente da barrinha
                barra.Value = barra.Value - (float)0.01;
                yield return new WaitForSeconds(.100f);
            }

            if (nomeBarra.CompareTo("barra_afeicao_melody") == 0)
            {
                this.barraAfeicaoMelodyTerminouDeSubirOuDescer = true;
            }
            else
            {
                this.barraBondadeTerminouDeSubirOuDescer = true;
            }
        }
        else
        {
            //vamos aumentar a barra gradativamente
            while (barra.Value < valorNaQualABarraTemDeParar)
            {
                barra.Value = barra.Value + (float)0.01;
                yield return new WaitForSeconds(.100f);
            }

            if (nomeBarra.CompareTo("barra_afeicao_melody") == 0)
            {
                this.barraAfeicaoMelodyTerminouDeSubirOuDescer = true;
            }
            else
            {
                this.barraBondadeTerminouDeSubirOuDescer = true;
            }
        }
    }


    private void executarAnimacaoAumentarPercentualCarinhaAfeicaoMelody(int percentualAntigo, int novoPercentual, int quantasCarinhasAfeicaoMelodyEstaoCheiasAntigo, ModoHougaii modoHougaii)
    {
        int quantasCarinhasAfeicaoMelodyEstaoCheiasNovo = modoHougaii.getquantasCarinhasAfeicaoMelodyEstaoCheias();
        if (percentualAntigo == novoPercentual)
        {
            //existe essa possibilidade: a barrinha ja estah no 100%, o que significa que a cada vez que o usuario
            //continuar sendo bom, ele vai nem precisar encher a carinha, ja vai direto aumentando o num de barrinhas cheias,
            //mas de qualquer forma seria bom mostrar uma animacaozinha
            if (quantasCarinhasAfeicaoMelodyEstaoCheiasAntigo != quantasCarinhasAfeicaoMelodyEstaoCheiasNovo)
            {
                //entao o numero de carinhas cheias aumentou! Vamos fazer uma pequena animacao onde 
                //uma carinha vai ficar cheia por um tempo e depois vai ficar vazia
                carinha_para_encher carinha_para_encher_melody = GameObject.Find("carinha_para_encher_melody").GetComponent<carinha_para_encher>();
                StartCoroutine(manterPercentualCarinhaEm100EDepoisVoltarA0(carinha_para_encher_melody));
            }
            else
            {
                //nao faz nada! Afinal, o percentual continua o mesmo...
            }
        }
        else if (percentualAntigo < novoPercentual)
        {
            //ouve um aumento normal(abaixo de 100%) da carinha
            carinha_para_encher carinha_para_encher_melody = GameObject.Find("carinha_para_encher_melody").GetComponent<carinha_para_encher>();
            carinha_para_encher_melody.mudarSpriteAtual(novoPercentual);
        }
        else
        {
            //como neste jogo os percentuais nao diminuem, entao isso soh pode significar uma coisa: uma nova carinha cheia
            //foi criada! Temos de fazer uma pequena animacao onde o percentual fica 100% e depois de um tempo vira 0%
            carinha_para_encher carinha_para_encher_melody = GameObject.Find("carinha_para_encher_melody").GetComponent<carinha_para_encher>();
            StartCoroutine(manterPercentualCarinhaEm100EDepoisVoltarA0(carinha_para_encher_melody));
        }
    }

    private void executarAnimacaoAumentarPercentualCarinhaBondade(int percentualAntigo, int novoPercentual, bool usuarioErrouDePropositoParaFazerMelodyFeliz, int quantasCarinhasBondadeEstaoCheiasAntigo, ModoHougaii modoHougaii)
    {
        int quantasCarinhasBondadeEstaoCheiasNovo = modoHougaii.getquantasCarinhasBondadeEstaoCheias();
        if (usuarioErrouDePropositoParaFazerMelodyFeliz == true)
        {
            //INICIAR PUNICAO NA CARINHA TB!JA FOI FEITO ISSO NA PARTE DO CODIGO. FALTA NA PARTE DE ANIMACAO
            //mas serah que o percentualde carinhas preenchidas de bondade nao ja eram 0? Se sim, nem preciso desta animacao
            if (novoPercentual != percentualAntigo)
            {
                StartCoroutine(piscarCarinhaBondadeEDepoisVoltarAo0());
            }
        }
        else if (percentualAntigo == novoPercentual)
        {
            //existe essa possibilidade: a barrinha ja estah no 100%, o que significa que a cada vez que o usuario
            //continuar sendo bom, ele vai nem precisar encher a carinha, ja vai direto aumentando o num de barrinhas cheias,
            //mas de qualquer forma seria bom mostrar uma animacaozinha
            if (quantasCarinhasBondadeEstaoCheiasAntigo != quantasCarinhasBondadeEstaoCheiasNovo)
            {
                //entao o numero de carinhas cheias aumentou! Vamos fazer uma pequena animacao onde 
                //uma carinha vai ficar cheia por um tempo e depois vai ficar vazia
                carinha_para_encher carinha_para_encher_bondade = GameObject.Find("carinha_para_encher_bondade").GetComponent<carinha_para_encher>();
                StartCoroutine(manterPercentualCarinhaEm100EDepoisVoltarA0(carinha_para_encher_bondade));
            }
            else
            {
                //nao faz nada! Afinal, o percentual continua o mesmo 
            }
        }
        else if (percentualAntigo < novoPercentual)
        {
            //ouve um aumento normal(abaixo de 100%) da carinha
            carinha_para_encher carinha_para_encher_bondade = GameObject.Find("carinha_para_encher_bondade").GetComponent<carinha_para_encher>();
            carinha_para_encher_bondade.mudarSpriteAtual(novoPercentual);
        }
        else
        {
            //como neste jogo os percentuais nao diminuem, entao isso soh pode significar uma coisa: uma nova carinha cheia
            //foi criada! Temos de fazer uma pequena animacao onde o percentual fica 100% e depois de um tempo vira 0%
            carinha_para_encher carinha_para_encher_bondade = GameObject.Find("carinha_para_encher_bondade").GetComponent<carinha_para_encher>();
            StartCoroutine(manterPercentualCarinhaEm100EDepoisVoltarA0(carinha_para_encher_bondade));
        }
    }

    private IEnumerator piscarCarinhaBondadeEDepoisVoltarAo0()
    {
        carinha_para_encher carinha_para_encher_bondade = GameObject.Find("carinha_para_encher_bondade").GetComponent<carinha_para_encher>();
        carinha_para_encher_bondade.GetComponent<Renderer>().enabled = true;
        double tempoPassado = 0;
        while (tempoPassado < 3)
        {
            yield return new WaitForSeconds((float)0.5);
            carinha_para_encher_bondade.GetComponent<Renderer>().enabled = !carinha_para_encher_bondade.GetComponent<Renderer>().enabled;
            tempoPassado = tempoPassado + 0.5;
        }

        carinha_para_encher_bondade.mudarSpriteAtual(0);
        carinha_para_encher_bondade.GetComponent<Renderer>().enabled = true;
    }

    private IEnumerator manterPercentualCarinhaEm100EDepoisVoltarA0(carinha_para_encher qualCarinhaDeveSofrerEssaAnimacao)
    {
        //encher carinha ao 100%
        qualCarinhaDeveSofrerEssaAnimacao.mudarSpriteAtual(100);
        qualCarinhaDeveSofrerEssaAnimacao.GetComponent<Renderer>().enabled = true;
        double tempoPassado = 0;
        while (tempoPassado < 3)
        {
            yield return new WaitForSeconds((float)0.5);
            qualCarinhaDeveSofrerEssaAnimacao.GetComponent<Renderer>().enabled = !qualCarinhaDeveSofrerEssaAnimacao.GetComponent<Renderer>().enabled;
            tempoPassado = tempoPassado + 0.5;
        }

        qualCarinhaDeveSofrerEssaAnimacao.mudarSpriteAtual(0);
        qualCarinhaDeveSofrerEssaAnimacao.GetComponent<Renderer>().enabled = true;
    }

    private void executarAnimacaoAlterarTextoQuantasCarinhasAfeicaoMelodyEstaoCheias(ModoHougaii modoHougaii, int quantasCarinhasAfeicaoMelodyEstaoCheiasAntigo)
    {
        int quantasCarinhasAfeicaoMelodyEstaoCheiasNovo = modoHougaii.getquantasCarinhasAfeicaoMelodyEstaoCheias();
        Text quantasCarinhasAfeicaoMelodyCheiasText = GameObject.Find("quantasCarinhasAfeicaoMelodyCheias").GetComponent<Text>();
        if (quantasCarinhasAfeicaoMelodyEstaoCheiasAntigo < quantasCarinhasAfeicaoMelodyEstaoCheiasNovo)
        {
            //fazer texto ficar verde por um tempo
            StartCoroutine(alterarTextoPorAlgumTempo("verde", quantasCarinhasAfeicaoMelodyCheiasText));
        }
    }
    private void executarAnimacaoAlterarTextoQuantasCarinhasBondadeEstaoCheias(ModoHougaii modoHougaii, bool usuarioErrouDePropositoParaFazerMelodyFeliz, int quantasCarinhasBondadeEstaoCheiasAntigo)
    {
        int quantasCarinhasBondadeEstaoCheiasNovo = modoHougaii.getquantasCarinhasBondadeEstaoCheias();
        Text quantasCarinhasBondadeCheiasText = GameObject.Find("quantasCarinhasBondadeCheias").GetComponent<Text>();
        if (usuarioErrouDePropositoParaFazerMelodyFeliz == true)
        {
            //fazer um boo!!!!
            //fazer texto ficar vermelho por um tempo
            StartCoroutine(alterarTextoPorAlgumTempo("vermelho", quantasCarinhasBondadeCheiasText));
        }
        else if (quantasCarinhasBondadeEstaoCheiasAntigo < quantasCarinhasBondadeEstaoCheiasNovo)
        {
            //fazer texto ficar verde por um tempo
            StartCoroutine(alterarTextoPorAlgumTempo("verde", quantasCarinhasBondadeCheiasText));
        }
    }
    IEnumerator alterarTextoPorAlgumTempo(string corTexto, Text texto)
    {
        Color corAntigaDoTexto = texto.color;
        if (corTexto.CompareTo("verde") == 0)
        {
            texto.color = Color.green;
        }
        else if(corTexto.CompareTo("vermelho") == 0)
        {
            texto.color = Color.red;
        }

        int quantosSegundosSePassaram = 0;
        while (quantosSegundosSePassaram < 3)
        {
            yield return new WaitForSeconds(1);
            quantosSegundosSePassaram = quantosSegundosSePassaram + 1;
        }
        texto.color = corAntigaDoTexto;
    }


    private void alterarTextoQuantasCarinhasAfeicaoMelodyEstaoCheias(ModoHougaii modoHougaii)
    {
        int quantasCarinhasAfeicaoMelodyEstaoCheias = modoHougaii.getquantasCarinhasAfeicaoMelodyEstaoCheias();
        Text quantasCarinhasAfeicaoMelodyCheiasText = GameObject.Find("quantasCarinhasAfeicaoMelodyCheias").GetComponent<Text>();

        if (quantasCarinhasAfeicaoMelodyEstaoCheias < 10)
        {
            quantasCarinhasAfeicaoMelodyCheiasText.text = "0" + quantasCarinhasAfeicaoMelodyEstaoCheias.ToString();
        }
        else
        {
            quantasCarinhasAfeicaoMelodyCheiasText.text = quantasCarinhasAfeicaoMelodyEstaoCheias.ToString();
        }
    }

    private void alterarTextoQuantasCarinhasBondadeEstaoCheias(ModoHougaii modoHougaii)
    {
        int quantasCarinhasBondadeEstaoCheias = modoHougaii.getquantasCarinhasBondadeEstaoCheias();
        Text quantasCarinhasBondadeCheiasText = GameObject.Find("quantasCarinhasBondadeCheias").GetComponent<Text>();

        if (quantasCarinhasBondadeEstaoCheias < 10)
        {
            quantasCarinhasBondadeCheiasText.text = "0" + quantasCarinhasBondadeEstaoCheias.ToString();
        }
        else
        {
            quantasCarinhasBondadeCheiasText.text = quantasCarinhasBondadeEstaoCheias.ToString();
        }
    }

    private void fazerEssaTelaEscolhaEtosanHougaiiDesaparecer()
    {
        PopupWindowBehavior telaEscolhaEtosanHougaii = GameObject.Find("telaEscolhaEtosanHougaii").GetComponent<PopupWindowBehavior>();
        telaEscolhaEtosanHougaii.irParaPosicaoDeDesaparecer();
        PopupWindowBehavior escolhaEtosanHougaiiTexto = GameObject.Find("escolhaEtosanHougaiiTexto").GetComponent<PopupWindowBehavior>();
        escolhaEtosanHougaiiTexto.irParaPosicaoDeDesaparecer();

        GUIBarScript barra_afeicao_melody = GameObject.Find("barra_afeicao_melody").GetComponent<GUIBarScript>();
        barra_afeicao_melody.irParaPosicaoDeDesaparecer();
        GUIBarScript barra_bondade = GameObject.Find("barra_bondade").GetComponent<GUIBarScript>();
        barra_bondade.irParaPosicaoDeDesaparecer();
    }

}
