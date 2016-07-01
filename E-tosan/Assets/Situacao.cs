using UnityEngine;
using System.Collections;

public class Situacao 
{
	private ArrayList temposVerbaisEstudados;
	private string verbo;
	private string lugar;
	private string depoimentoMelody;
	private string depoimentoRegras; //depoimento da mocinha que diz as regras
	private ArrayList duasRespostasDoEtosan; // e-tosan soh pode dizer duas respostas
	private bool respostaAgradaMelody;

	public Situacao(ArrayList temposVerbaisEstudados, string verbo, string lugar, string depoimentoMelody, string depoimentoRegras, ArrayList duasRespostasDoEtosan, bool respostaAgradaMelody)
	{
		this.temposVerbaisEstudados = temposVerbaisEstudados;
		this.verbo = verbo;
		this.lugar = lugar;
		this.depoimentoMelody = depoimentoMelody;
		this.depoimentoRegras = depoimentoRegras;
		this.duasRespostasDoEtosan = duasRespostasDoEtosan;
		this.respostaAgradaMelody = respostaAgradaMelody;
	}
}
