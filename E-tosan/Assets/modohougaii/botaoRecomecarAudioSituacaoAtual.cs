using UnityEngine;
using System.Collections;

public class botaoRecomecarAudioSituacaoAtual : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseDown()
    {

        TelaSituacaoHougaii telaSituacaoHougaii = GameObject.Find("telaSituacaoHougaii").GetComponent<TelaSituacaoHougaii>();
        telaSituacaoHougaii.recomecarAudioSituacaoAtual();
    }
}