﻿using UnityEngine;
using System.Collections;

public class PopupWindowBehavior : MonoBehaviour {

    Vector3 posicaoInicial;

    
    // Use this for initialization
    void Start()
    {
        /*foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().enabled = false;
        }*/

        //posicaoInicial = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //essa funcao serah chamada pelo script que fica na sua Main Camera antes da primeira execucao de irParaPosicaoDeDesaparecer()
    //senao nao tem como obter posicaoInicial
    public void obterPosicaoInicial()
    {
        posicaoInicial = transform.localPosition;
        //nao podia fazer isso no Start()? Nao! Pq nao sabemos quando Start() sera executado para todas as janelas
    }

    public void voltarAPosicaoInicial()
    {
        transform.localPosition = posicaoInicial;
        /*foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().enabled = true;
        }*/
    }

    public void irParaPosicaoDeDesaparecer()
    {
        transform.localPosition = new Vector3(1000, 1000);
        /*foreach (Transform child in transform)
        {
            child.GetComponent<Renderer>().enabled = false;
        }*/
    }

}
