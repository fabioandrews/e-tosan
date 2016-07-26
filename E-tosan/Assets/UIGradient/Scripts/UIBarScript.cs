using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIBarScript : MonoBehaviour
{
    //Declare variables

    //for the CurrentValue, and the Value it will be after the update
    private float currentValue;

    [Range(0, 1)]
    public float Value;

    //FadeValue is current amount the bar is faded 
    private float fadeValue;
    //FadeFactor is a little complex, Open the ReadMe file to know more
    public float fadeFactor = 6f;

    //Carries the .png images to display the GUIBar
    public Image background;
    public Image mask;
    public Image valueBar; //Each Bar needs it's own ValueBar Texture
    public Image foreground;
    public Color TextColor;

    //Text Variables
    public bool displayText = true;
    public Text textValue;
    public bool overRideTextColorWithGradient = false;

    //Carries the colors that the GUIbar will be
    public List<Color> gradientColors = new List<Color>();

    //These are used for redrawing the GUIBar
    private Gradient g = new Gradient();
    private GradientColorKey[] gck;
    private GradientAlphaKey[] gak;
    private Color[] maskPixels;

    private Camera camera;
    private Vector2 savePosition;

    private RectTransform rectTransform;
    public Vector2 PosicaoInicial;

    void Start()
    {
        //        camera = GameObject.Find( "Main Camera").GetComponent<Camera>();
        //        savePosition = Position;
        //        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void obterPosicaoInicial()
    {
        if (this.gameObject.name.CompareTo("barra_bondade") == 0)
        {
            this.PosicaoInicial = GameObject.Find("BackgroundBondade").GetComponent<RectTransform>().anchoredPosition;
        }
        //this.PosicaoInicial = transform.localPosition;
    }

    public void irParaPosicaoDeDesaparecer()
    {
        //this.transform.localPosition = new Vector2(10000, 10000);
        if (this.gameObject.name.CompareTo("barra_bondade") == 0)
        {
            this.GetComponent<CanvasGroup>().alpha = 0.0f;
            GameObject.Find("cara_anjinho").GetComponent<Renderer>().enabled = false;
            GameObject.Find("cara_diabinho").GetComponent<Renderer>().enabled = false;

        }

        if (this.gameObject.name.CompareTo("barra_afeicao_melody") == 0)
        {
            this.GetComponent<CanvasGroup>().alpha = 0.0f;
            GameObject.Find("melody_zangada").GetComponent<Renderer>().enabled = false;
            GameObject.Find("melody_sorrindo").GetComponent<Renderer>().enabled = false;

        }
    }

    public void voltarAPosicaoInicial()
    {
        //this.transform.localPosition = PosicaoInicial;
        if (this.gameObject.name.CompareTo("barra_bondade") == 0)
        {
            this.GetComponent<CanvasGroup>().alpha = 1.0f;
            GameObject.Find("cara_anjinho").GetComponent<Renderer>().enabled = true;
            GameObject.Find("cara_diabinho").GetComponent<Renderer>().enabled = true;
        }

        if (this.gameObject.name.CompareTo("barra_afeicao_melody") == 0)
        {
            this.GetComponent<CanvasGroup>().alpha = 1.0f;
            GameObject.Find("melody_zangada").GetComponent<Renderer>().enabled = true;
            GameObject.Find("melody_sorrindo").GetComponent<Renderer>().enabled = true;

        }

        textValue.color = TextColor;
        textValue.text = ((int)(Value * 100)).ToString() + "%";
    }

    //Stanard OnGUI Method
    void Update()
    {
        //UpdateBar is a very large function so i'm only excuting it when i have to.
        if (Mathf.Round(currentValue * 100f) != Mathf.Round(Value * 100f))
        {
            UpdateBar();
        }

        //if display text is enabled the display text will be drawn
        if (displayText && (textValue != null))
        {

            if (overRideTextColorWithGradient)
            {
                textValue.color = new Color(g.Evaluate(Value * 0.99f).r, g.Evaluate(Value * 0.99f).g, g.Evaluate(Value * 0.99f).b, 1.0f);
            }

            textValue.color = TextColor;
            textValue.text = ((int)(Value * 100)).ToString() + "%";
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

        //for each pixel in the ValueBar, we will change the color to w/e it is in the gradient
        RectTransform valueBarRect = valueBar.gameObject.GetComponent<RectTransform>();
        RectTransform maskRect = mask.gameObject.GetComponent<RectTransform>();
        int y = 0;
        while (y < valueBarRect.rect.height)
        {
            int x = 0;
            float xf = (float) x;
            while (x < valueBarRect.rect.width)
            {
                Color gC = g.Evaluate(xf / maskRect.rect.width);

                if (mask.sprite.texture.GetPixel(x, y).a > 0.1f)
                {
                    valueBar.sprite.texture.SetPixel(x, y, gC);
                }
                x = x + 1;
                xf = xf + 1;
            }
            y = y + 1;
        }

        //set the new colors on the valueBar
        valueBar.sprite.texture.Apply();
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
        currentValue = Value;

        //the fadeFactor is used to set the fadeValue, see ReadMe document for more Info
        fadeValue = ((Mathf.Sin((Value) * 3.14f)) / fadeFactor);

        if (fadeFactor == 0)
        {
            print("fadeFactor = 0 does not produce a good gradient all the way through the bar");
        }

        //clamping values of variables
        fadeFactor = Mathf.Clamp(fadeFactor, -1f, 20f);
        currentValue = Mathf.Clamp(currentValue, 0f, 1f);
        Value = Mathf.Clamp(Value, 0f, 1f);
        fadeValue = Mathf.Clamp(fadeValue, 0.0001f, 1f);

        //create variable to store the colors for the gradient
        gck = new GradientColorKey[gradientColors.Count];

        //add colors to gradient
        int i = 0;
        float f = 0f;
        while (i < gradientColors.Count)
        {
            gck[i].color = gradientColors[i];
            gck[i].time = f / (gradientColors.Count - 1);
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
        gak[1].time = currentValue - (fadeValue / 2);

        gak[2].alpha = 0.00f;
        gak[2].time = currentValue + (fadeValue / 2);

        //add keys to gradient
        g.SetKeys(gck, gak);

    }


    //The following methods can be used within other code do change how the GUIBar Looks
    public void AddNewColor(Color color, int Key)
    {
        gradientColors.Insert(Key, color);
    }

    public void ChangeColor(Color color, int Key)
    {
        gradientColors[Key] = color;
    }

    public void RemoveColor(int Key)
    {
        gradientColors.RemoveAt(Key);
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
