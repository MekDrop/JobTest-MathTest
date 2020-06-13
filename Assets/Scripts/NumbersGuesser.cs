using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Main game logic
/// </summary>
public class NumbersGuesser : MonoBehaviour
{

    /// <summary>
    /// Minimal number to be guessed
    /// </summary>
    public int minNumber = 1;

    /// <summary>
    /// Maximal number to be guessed
    /// </summary>
    public int maxNumber = 1000;

    /// <summary>
    /// How many lifes can be at game start?
    /// </summary>
    public int maxLifes = 5;
    
    /// <summary>
    /// Scrollview content that is linked for adding new data lines
    /// </summary>
    public GameObject linkedScrollViewContent;

    /// <summary>
    /// Linked input field from where to get value
    /// </summary>
    public InputField linkedInputField;

    /// <summary>
    /// Font to be used for console messages
    /// </summary>
    public Font consoleFont;

    /// <summary>
    /// Padding from left or ride side depending on text message aligment
    /// </summary>
    public float textHorizontalPaddingMin = 50;

    /// <summary>
    /// Inverted horizontal padding depending on text message aligment
    /// </summary>
    public float textHorizontalPaddingMax = 200;

    /// <summary>
    /// Vertical padding from top of the screen for first message
    /// </summary>
    public float textVerticalPaddingMin = 20;

    /// <summary>
    /// Line height for console message
    /// </summary>
    public float consoleLineHeight = 20.0f;
    
    /// <summary>
    /// Sprite that is used for indicating lifes
    /// </summary>
    public Sprite heart;

    /// <summary>
    /// How many lifes player has now?
    /// </summary>
    protected int currentLifes;

    /// <summary>
    /// Defines player image element
    /// </summary>
    public Image linkedPlayerImage;

    /// <summary>
    /// Enum to define how text will be aligned when writing console messages
    /// </summary>
    protected enum TextAlign: ushort
    {
        Left = 0,
        Right = 1
    }

    // Start is called before the first frame update
    void Start()
    {
        this.currentNumber = this.GenerateNumber();
        this.currentLifes = this.maxLifes;
        this.linkedPlayerImage.sprite = SharedData.playerSprite;

        string maskedNumber = "".PadLeft(this.currentNumber.ToString().Length, '*');
        
        this.DrawLifes();
        this.WriteMessage("<color=yellow>" + maskedNumber + "</color>" );
        this.WriteMessage("Koks tai skaičius?");

        this.linkedInputField.ActivateInputField();
        this.linkedInputField.Select();
    }

    /// <summary>
    /// Event executed when number is entered in input field
    /// </summary>
    public void OnNumberEntered()
    {
        if (this.linkedInputField.text.Trim().Length == 0)
        {
            return;
        }

        int number = int.Parse(this.linkedInputField.text);

        this.WriteMessage(number.ToString(), TextAlign.Right);
        this.linkedInputField.text = "";
        this.linkedInputField.ActivateInputField();
        this.linkedInputField.Select();

        if (number == this.currentNumber)
        {
            this.WriteMessage("Yey! Laimėjote!");
            SharedData.hasWon = true;
            this.gameObject.GetComponent<SceneSwitcher>().SwitchToNextScene();
            return;
        }

        if (--this.currentLifes == 0)
        {
            this.WriteMessage("Oh... deja, nepavyko atspėti!");
            SharedData.hasWon = false;
            this.gameObject.GetComponent<SceneSwitcher>().SwitchToNextScene();
            return;
        }
        
        this.DrawLifes();
        if (number > this.currentNumber)
        {
            this.WriteMessage("Sugeneruotas skaičius yra mažesnis");
        } else if (number < this.currentNumber)
        {
            this.WriteMessage("Sugeneruotas skaičius yra didesnis");
        }
    }

    /// <summary>
    /// Current number that is guessing
    /// </summary>
    protected int currentNumber
    {
        get
        {
            return SharedData.lastNumber;
        }
        set
        {
            SharedData.lastNumber = value;
        }
    }

    /// <summary>
    /// Generates number for guessing
    /// </summary>
    /// <returns>Generated number</returns>
    protected int GenerateNumber()
    {
        System.Random rnd = new System.Random();
        return rnd.Next(this.minNumber, this.maxNumber);
    }

    /// <summary>
    /// Writes message to console
    /// </summary>
    /// <param name="msg">Message to write to console</param>
    /// <param name="align">How message should be aligned?</param>
    protected void WriteMessage(string msg, TextAlign align = TextAlign.Left)
    {
        float left = (align == TextAlign.Left) ? this.textHorizontalPaddingMin : this.textHorizontalPaddingMax;
        float right = (align == TextAlign.Left) ? -this.textHorizontalPaddingMax : -this.textHorizontalPaddingMin;
        GameObject txtUI = CreateContentGameObject(left, right);

        Text txt = txtUI.AddComponent<Text>();
        txt.text = msg;
        txt.font = this.consoleFont;        
        txt.alignment = align == TextAlign.Left ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight;

        RectTransform trans = txtUI.GetComponent<RectTransform>();
        trans.sizeDelta = new Vector2(trans.sizeDelta.x, this.consoleLineHeight);
    }

    /// <summary>
    /// Draws how many hearts as how many lifes players has
    /// </summary>
    protected void DrawLifes()
    {
        GameObject lifesUI = CreateContentGameObject(this.textHorizontalPaddingMin, -this.textHorizontalPaddingMax, false);

        Image image = lifesUI.AddComponent<Image>();
        image.sprite = this.heart;
        image.type = Image.Type.Tiled;
        image.pixelsPerUnitMultiplier = this.heart.rect.height / this.consoleLineHeight; 

        RectTransform trans = lifesUI.GetComponent<RectTransform>();
        trans.sizeDelta = new Vector2(
            this.heart.rect.width * this.currentLifes / image.pixelsPerUnitMultiplier,
            Math.Min(this.heart.rect.height, this.consoleLineHeight)
        );
    }

    /// <summary>
    /// Creates console game object
    /// </summary>
    /// <param name="left">Left position</param>
    /// <param name="right">Right position</param>
    /// <param name="fillWidth">if tru automatically tries to use max possible width of line</param>
    /// <returns></returns>
    protected GameObject CreateContentGameObject(float left, float right, bool fillWidth = true)
    {
        int no = linkedScrollViewContent.transform.childCount + 1;
        float top = -this.consoleLineHeight * no - textVerticalPaddingMin;

        GameObject newObj = new GameObject("DataRow" + no.ToString());
        newObj.transform.SetParent(linkedScrollViewContent.transform);

        RectTransform trans = newObj.AddComponent<RectTransform>();
        trans.anchorMin = new Vector2(0, 1);
        trans.anchorMax = new Vector2(fillWidth ? 1 : 0, 1);
        trans.pivot = new Vector2(0, 0);

        trans.offsetMin = new Vector2(left, top);
        trans.offsetMax = new Vector2(right, 0);

        return newObj;
    }

}
