using UnityEngine;
using System.Collections;

//essa classe vai se comunicar bom o BD e vai fornecer situacoes para a classe ModoHougaii
public class FornecedorDeSituacoes
{
    System.Collections.Generic.LinkedList<Situacao> possiveisSituacoes;
    public FornecedorDeSituacoes()
    {
        possiveisSituacoes = new System.Collections.Generic.LinkedList<Situacao>();
        this.obterPossiveisSituacoes();
    }

    public void obterPossiveisSituacoes() //essa funcao futuramente vai se comunicar com o BD e vai obter todas as possiveis situacoes que o sistema pode oferecer
    {

    }
}
