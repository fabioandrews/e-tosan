using UnityEngine;
using System.Collections;

public class carinha_para_encher : MonoBehaviour {

    public Sprite sprite0;
    public Sprite sprite25;
    public Sprite sprite50;
    public Sprite sprite75;
    public Sprite sprite100;

    // Use this for initialization
    void Start()
    {
        this.GetComponent<SpriteRenderer>().sprite = sprite0;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void mudarSpriteAtual(int percentualCarinha)
    {
        if (percentualCarinha >= 0 && percentualCarinha < 25)
        {
            this.GetComponent<SpriteRenderer>().sprite = sprite0;
        }
        else if (percentualCarinha >= 25 && percentualCarinha < 50)
        {
            this.GetComponent<SpriteRenderer>().sprite = sprite25;
        }
        else if (percentualCarinha >= 50 && percentualCarinha < 75)
        {
            this.GetComponent<SpriteRenderer>().sprite = sprite50;
        }
        else if (percentualCarinha >= 75 && percentualCarinha < 100)
        {
            this.GetComponent<SpriteRenderer>().sprite = sprite75;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = sprite100;
        }
    }
}
