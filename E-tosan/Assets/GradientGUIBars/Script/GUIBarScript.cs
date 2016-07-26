using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUIBarScript : MonoBehaviour
{

	//Declare variables

	//for the CurrentValue, and the Value it will be after the update
	private float CurrentValue;
	public float Value;

	//FadeValue is current amount the bar is faded 
	private float FadeValue;
	//FadeFactor is a little complex, Open the ReadMe file to know more
	public float FadeFactor = 6f;

    //position and scale of the GUIBar on the screen
	public float ScaleSize;

	//Font Variables
	public bool DisplayText = true;
	public string TextString;
	public Color TextColor;
	public bool OverRideTextColorWithGradient = false;
	public Font TextFont;
	public float TextSize;
	public Vector2 TextOffset;

    public Vector2 Position;


	//Carries the .png images to display the GUIBar
	public Texture2D Background;
	public Texture2D Mask;
	public Texture2D ValueBar; //Each Bar needs it's own ValueBar Texture
	public Texture2D Foreground;
    
	//Carries the colors that the GUIbar will be
	public List<Color> GradientColors = new List<Color>();

	//These are used for redrawing the GUIBar
	private Gradient g = new Gradient();
	private GradientColorKey[] gck;
	private GradientAlphaKey[] gak;
	private Color[] MaskPixels;
    public Vector2 PosicaoInicial;
    public Vector2 posicaoRetangulo;

    void Start()
    {
    }

    public Vector2 WorldToGuiPoint(Vector3 position)
    {
        var guiPosition = Camera.main.WorldToScreenPoint(position);
        // Y axis coordinate in screen is reversed relative to world Y coordinate
        guiPosition.y = Screen.height - guiPosition.y;

        return guiPosition;
    }

    public void obterPosicaoInicial()
    {
        this.PosicaoInicial = transform.localPosition;
        if (this.gameObject.name.CompareTo("barra_bondade") == 0)
        {
            //this.PosicaoInicial = GameObject.Find("cara_diabinho").transform.localPosition + (Vector3.up * 5);
            this.PosicaoInicial = GameObject.Find("cara_diabinho").transform.position;
            //this.posicaoRetangulo = Camera.main.WorldToViewportPoint(this.PosicaoInicial); //converter posicao do gameobject na tela para posicao que retangulo precisa
            Vector3 cara_diabinho_posicao = GameObject.Find("cara_diabinho").transform.position;
            Vector3 posicao_retangulo_relativa_ao_diabinho = cara_diabinho_posicao;
            //posicao_retangulo_relativa_ao_diabinho.x += (float)0.5;
            //posicao_retangulo_relativa_ao_diabinho.x += (float)(GameObject.Find("cara_diabinho").GetComponent<Renderer>().bounds.size.x / 2);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(cara_diabinho_posicao);
            Vector2 guiPosition = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
            this.posicaoRetangulo = guiPosition;
            //this.posicaoRetangulo = WorldToGuiPoint(GameObject.Find("cara_diabinho").transform.position);
        }
    }

    public void irParaPosicaoDeDesaparecer()
    {
        this.transform.localPosition = new Vector2(10000, 10000);
        this.posicaoRetangulo = Camera.main.WorldToScreenPoint(transform.position); //converter posicao do gameobject na tela para posicao que retangulo precisa

    }

    public void voltarAPosicaoInicial()
    {
        this.transform.localPosition = PosicaoInicial;
        //this.posicaoRetangulo = Camera.main.WorldToViewportPoint(PosicaoInicial);
        if (this.gameObject.name.CompareTo("barra_bondade") == 0)
        {
            Vector3 cara_diabinho_posicao = GameObject.Find("cara_diabinho").transform.position;
            print("cara_diabinho position:" + GameObject.Find("cara_diabinho").transform.position.x + ";" + GameObject.Find("cara_diabinho").transform.localPosition.y);
            Vector3 posicao_retangulo_relativa_ao_diabinho = cara_diabinho_posicao;
            //posicao_retangulo_relativa_ao_diabinho.x += (float)0.5;
            posicao_retangulo_relativa_ao_diabinho.x += (float)(GameObject.Find("cara_diabinho").GetComponent<Renderer>().bounds.size.x / 2);
            //this.posicaoRetangulo = Camera.main.WorldToScreenPoint(posicao_retangulo_relativa_ao_diabinho);
            //this.posicaoRetangulo.y += Screen.width / 1200;//(float)70.5;
            //this.posicaoRetangulo = WorldToGuiPoint(GameObject.Find("cara_diabinho").transform.position);

            Vector3 screenPos = Camera.main.WorldToScreenPoint(cara_diabinho_posicao);
            Vector2 guiPosition = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
            this.posicaoRetangulo = guiPosition;

        }
    }


    //Stanard OnGUI Method
    void OnGUI()
    {

        //UpdateBar is a very large function so i'm only excuting it when i have to.
        if (
            Mathf.Round(CurrentValue * 100f) != Mathf.Round(Value * 100f)
            )
        {
            UpdateBar();
        }



        //if you don't have a background texture i won't draw it
        if (Background != null)
        {

            //GUI.DrawTexture(new Rect(posicaoRetangulo.x, posicaoRetangulo.y,Background.width * ScaleSize,Background.height * ScaleSize),Background);
            //GUI.DrawTexture(new Rect(posicaoRetangulo.x, Screen.height - posicaoRetangulo.y, Background.width * ScaleSize, Background.height * ScaleSize), Background);
            Vector2 v2 = new Vector2(Background.width * ScaleSize, Background.height* ScaleSize);

            GUI.DrawTexture(new Rect(posicaoRetangulo, v2), Background);

        }

        //GUI.DrawTexture(new Rect(posicaoRetangulo.x, posicaoRetangulo.y,ValueBar.width * ScaleSize,ValueBar.height * ScaleSize),ValueBar);
        //GUI.DrawTexture(new Rect(posicaoRetangulo.x, Screen.height - posicaoRetangulo.y, ValueBar.width * ScaleSize, ValueBar.height * ScaleSize), ValueBar);
        Vector2 v3 = new Vector2(Background.width * ScaleSize, Background.height * ScaleSize);
        GUI.DrawTexture(new Rect(this.posicaoRetangulo, v3), ValueBar);


        //if you don't have a foreground texture i won't draw it
        if (Foreground != null)
        {
            //GUI.DrawTexture(new Rect(posicaoRetangulo.x, posicaoRetangulo.y,Foreground.width * ScaleSize,Foreground.height * ScaleSize),Foreground);
            //GUI.DrawTexture(new Rect(posicaoRetangulo.x, Screen.height - posicaoRetangulo.y, Foreground.width * ScaleSize, Foreground.height * ScaleSize), Foreground);
            Vector2 v2 = new Vector2(Background.width * ScaleSize, Background.height * ScaleSize);

            GUI.DrawTexture(new Rect(this.posicaoRetangulo,v2), Foreground);


        }

        //if display text is enabled the display text will be drawn
        if (DisplayText)
        {

            GUIStyle LabelStyle = new GUIStyle();

            if (OverRideTextColorWithGradient)
            {
                Color MaxGradientColor = new Color(g.Evaluate(Value * 0.99f).r, g.Evaluate(Value * 0.99f).g, g.Evaluate(Value * 0.99f).b, 1.0f);
                LabelStyle.normal.textColor = MaxGradientColor;
            }
            else
            {
                LabelStyle.normal.textColor = TextColor;
            }
            LabelStyle.fontSize = (int)TextSize;
            LabelStyle.font = TextFont;

            TextString = ((int)(Value * 100)).ToString() + "%";

            GUI.Label(new Rect(posicaoRetangulo.x + TextOffset.x, posicaoRetangulo.y + TextOffset.y, ValueBar.width * ScaleSize, ValueBar.height * ScaleSize), TextString, LabelStyle);

        }
    }

    //this method will redraw the bar
    private void UpdateBar()
    {
        //update the gradient
        UpdateGradient();

        //error handling
        if (g == null)
        {
            return;
        }

        //for each pixle in the ValueBar, we will change the color to w/e it is in the gradient
        int y = 0;
        while (y < ValueBar.height)
        {
            int x = 0;
            float xf = 0f;
            while (x < ValueBar.width)
            {
                Color gC = g.Evaluate(xf / Mask.width);

                if (Mask.GetPixel(x, y).a > 0.1f)
                {
                    ValueBar.SetPixel(x, y, gC);
                }
                x = x + 1;
                xf = xf + 1;
            }
            y = y + 1;
        }

        //set the new colors on the ValueBar
        ValueBar.Apply();
    }

    //this method will update the gradient
    private void UpdateGradient()
    {
        //error handling
        if (g == null)
        {
            return;
        }

        //set the new value
        CurrentValue = Value;

        //the FadeFactor is used to set the FadeValue, see ReadMe document for more Info
        FadeValue = ((Mathf.Sin((Value) * 3.14f)) / FadeFactor);

        if (FadeFactor == 0)
        {
            print("FadeFactor = 0 does not produce a good gradient all the way through the bar");
        }

        //clamping values of variables
        FadeFactor = Mathf.Clamp(FadeFactor, -1f, 20f);
        CurrentValue = Mathf.Clamp(CurrentValue, 0f, 1f);
        Value = Mathf.Clamp(Value, 0f, 1f);
        FadeValue = Mathf.Clamp(FadeValue, 0.0001f, 1f);

        //create variable to store the colors for the gradient
        gck = new GradientColorKey[GradientColors.Count];

        //add colors to gradient
        int i = 0;
        float f = 0f;
        while (i < GradientColors.Count)
        {
            gck[i].color = GradientColors[i];
            gck[i].time = f / (GradientColors.Count - 1);
            i++;
            f++;
        }

        //if you do not want to use these colors you can hardcode them like so 
        /*
		 * gck[0].color = [any color];
		 * gck[0].time =  [float number between 0, and 1];
		 * 
		 * gck[1].color = [any color];
		 * gck[1].time =  [float number between 0, and 1];
		 * 
		 * ...etc etc etc
		 * 
		 */



        //set the alpha keys for the gradient
        gak = new GradientAlphaKey[3];
        gak[0].alpha = 1.0f;
        gak[0].time = 0.0f;

        gak[1].alpha = 1.0f;
        gak[1].time = CurrentValue - (FadeValue / 2);

        gak[2].alpha = 0.00f;
        gak[2].time = CurrentValue + (FadeValue / 2);

        //add keys to gradient
        g.SetKeys(gck, gak);

    }




    //The following methods can be used within other code do change how the GUIBar Looks
    public void AddNewColor(Color color, int Key)
    {
        GradientColors.Insert(Key, color);
    }

    public void ChangeColor(Color color, int Key)
    {
        GradientColors[Key] = color;
    }

    public void RemoveColor(int Key)
    {
        GradientColors.RemoveAt(Key);
    }

    public void SetNewValue(float V)
    {
        Value = V;
    }

    public void SetNewValue(double V)
    {
        Value = (float)V;
    }

    public void SetNewValue(float V, float MV)
    {
        Value = V / MV;
    }

    public void SetNewValue(double V, double MV)
    {
        Value = (float)V / (float)MV;
    }

    public void ForceUpdate()
    {
        UpdateBar();
    }

}
