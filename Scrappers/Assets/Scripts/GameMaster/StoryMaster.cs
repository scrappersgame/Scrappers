using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using TMPro;


public class StoryMaster : MonoBehaviour {

    public static StoryMaster sm;               // so we can call StoryMaster.sm from anywhere
    public Story story;                         // the ink story being told

    [SerializeField]
    private RectTransform textBox;              // main box containing all of the dialog ui
    [SerializeField]
    private TextMeshProUGUI dialog;             // what they're are saying
    [SerializeField]
    private TextMeshProUGUI speakerName;        // who's saying something
    [SerializeField]
    private Button buttonPrefab;                // choice button
    [SerializeField]
    private Canvas buttonCanvas;                // where the buttons go
    private bool nameGiven = false;             // have we given our name yet?

    private void Awake()
    {
        // setting sm to self so we can call it from anywhere
        if (sm == null)
            sm = GameObject.FindGameObjectWithTag("GM").GetComponent<StoryMaster>();
    }

    public void StartStory(TextAsset _inkJSONAsset)
    {
        // starts a new ink story, needs json input
        sm.story = new Story(_inkJSONAsset.text);
        sm.RefreshView();
    }

    public void EnterName (string _name){
        // only called at the beginning of the game when we first give our name
        story.variablesState["name"] = _name;
        story.ChoosePathString("name_given");
        nameGiven = true;
        RefreshView();
    }

    public void RefreshView()
    {
        // remove all the buttons before we start
        RemoveButtons();
        if (story.canContinue) // keep on keeping on
        {
            
            GameMaster.gm.speaking = true;
            textBox.gameObject.SetActive(true);
            string[] _storyText = story.Continue().Trim().Split(new string[] { ":" }, StringSplitOptions.None);
            if (_storyText.Length == 2)
            {
                speakerName.text = _storyText[0].Trim();
                dialog.text = _storyText[1].Trim();
            }else{
                speakerName.text = story.variablesState["name"].ToString();
                dialog.text = _storyText[0].Trim();
            }

            float _canvasWidth = buttonCanvas.gameObject.GetComponent<RectTransform>().rect.width;
            if (story.currentChoices.Count > 0)
            {
                float _buttonWidth = (_canvasWidth / story.currentChoices.Count);
                for (int i = 0; i < story.currentChoices.Count; i++)
                {
                    Choice choice = story.currentChoices[i];
                    Button button = CreateChoiceView(choice.text.Trim());
                    button.GetComponent<Button>().onClick.AddListener(() => { OnClickChoiceButton(choice.index); });
                    float _buttonHeight = button.gameObject.GetComponent<RectTransform>().rect.height;
                    button.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(_buttonWidth,_buttonHeight);
                }
            }
            else if (nameGiven){
                Button button = CreateChoiceView("continue...");
                button.GetComponent<Button>().onClick.AddListener(() => { RefreshView(); });
                float _buttonHeight = button.gameObject.GetComponent<RectTransform>().rect.height;
                button.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(_canvasWidth, _buttonHeight);
            }
        }
        else
        {
            GameMaster.gm.speaking = false;
            GameMaster.gm.GetComponent<WaveSpawner>().enabled = true;
            HideView();
        }
    }

    void HideView()
    {
        textBox.gameObject.SetActive(false);
    }

    Button CreateChoiceView(string text)
    {
        Button choice = Instantiate(buttonPrefab) as Button;
        choice.transform.SetParent(buttonCanvas.transform, false);

        TextMeshProUGUI choiceText = choice.GetComponentInChildren<TextMeshProUGUI>();
        choiceText.text = text;

        return choice;
    }
    void OnClickChoiceButton(int choice)
    {
        Debug.Log(choice);
        story.ChooseChoiceIndex(choice);
        RefreshView();
    }

    void RemoveButtons()
    {
        int childCount = buttonCanvas.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            Destroy(buttonCanvas.transform.GetChild(i).gameObject);
        }
    }

}