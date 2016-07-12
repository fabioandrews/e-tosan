using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SituacaoModoHougaii
{
    private string id; //eh a partir dele que iremos formar nomeArquivoWavDepoimentoMelody e nomeArquivoWavDepoimentoRegras
    private LinkedList<string> temposVerbaisEstudados;
    private string verbo;
    private string lugar;
    private LinkedList<string> duasRespostasDoEtosan; // e-tosan soh pode dizer duas respostas
    private string respostaCorretaDoEtosan; //qual das duas respostas e aquela que eh a mais boazinha q o eto-san deveria dizer?
    private bool respostaCorretaAgradaMelody;
    private string nomeArquivoWavMelody; //usaremos para mandar ao RadioLetsJam depois. Tem de ter a extensão tb
    private string nomeArquivoWavRegras; //nome do arquivo wav depoimento da mocinha que diz as regras
    //obs: para formar as duas strings acima, basta ter o id, pois o nome deles eh "id_melody" ou "id_regras" 

    public SituacaoModoHougaii(string id, LinkedList<string> temposVerbaisEstudados, string verbo, string lugar,
                    LinkedList<string> duasRespostasDoEtosan, string respostaCorretaDoEtosan, bool respostaCorretaAgradaMelody)
    {
        this.id = id;
        this.temposVerbaisEstudados = temposVerbaisEstudados;
        this.verbo = verbo;
        this.lugar = lugar;
        this.nomeArquivoWavMelody = id + "_melody.wav";
        this.nomeArquivoWavRegras = id + "_regras.wav";
        this.duasRespostasDoEtosan = duasRespostasDoEtosan;
        this.respostaCorretaAgradaMelody = respostaCorretaAgradaMelody;
        this.respostaCorretaDoEtosan = respostaCorretaDoEtosan;
    }

    public string getid()
    {
        return id;
    }

    public LinkedList<string> gettemposVerbaisEstudados()
    {
        return this.temposVerbaisEstudados;
    }

    public string getverbo()
    {
        return this.verbo;
    }

    public string getlugar()
    {
        return this.lugar;
    }

    public LinkedList<string> getduasRespostasDoEtosan()
    {
        return this.duasRespostasDoEtosan;
    }

    public string getrespostaCorretaDoEtosan()
    {
        return respostaCorretaDoEtosan;
    }

    public bool getrespostaAgradaMelody()
    {
        return respostaCorretaAgradaMelody;
    }

    public string getnomeArquivoWavMelody()
    {
        return nomeArquivoWavMelody;
    }

    public string getnomeArquivoWavRegras()
    {
        return nomeArquivoWavRegras;
    }
}
