using UnityEngine;
using System.Collections;

public class PopupPauseLetsJam : MonoBehaviour
{
    Vector3 posicaoInicial;

    // Use this for initialization
    void Start ()
    {
        posicaoInicial = transform.localPosition;
        transform.localPosition = new Vector3(1000, 1000);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void voltarAPosicaoInicial()
    {
        transform.localPosition = posicaoInicial;
    }

    public void irParaPosicaoDeDesaparecer()
    {
        transform.localPosition = new Vector3(1000, 1000);
    }
}
