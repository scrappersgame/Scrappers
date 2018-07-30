using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using Ink.Runtime;
using TMPro;


public class StoryMaster : MonoBehaviour {

    public static StoryMaster sm;
    public Story story;

    [SerializeField]
    private RectTransform textBox;
    [SerializeField]
    private TextMeshProUGUI dialog;
    [SerializeField]
    private TextMeshProUGUI speakerName;
    [SerializeField]
    private Button buttonPrefab;
    [SerializeField]
    private Canvas canvas;

    private void Awake()
    {
        if (sm == null)
            sm = GameObject.FindGameObjectWithTag("GM").GetComponent<StoryMaster>();
    }
    public void StartStory(TextAsset _inkJSONAsset)
    {
        sm.story = new Story(_inkJSONAsset.text);
        sm.RefreshView();
    }

    public void EnterName (string _name){
        story.variablesState["name"] = _name;
        story.ChoosePathString("name_given");
        RefreshView();
    }

    public void RefreshView()
    {
        RemoveButtons();
        if (story.canContinue)
        {
            GameMaster.gm.paused = true;
            textBox.gameObject.SetActive(true);
            string[] _storyText = story.Continue().Trim().Split(new string[] { ":" }, StringSplitOptions.None);
            while (story.canContinue) {
                _storyText = story.Continue().Trim().Split(new string[] { ":" }, StringSplitOptions.None);
            }
            if (_storyText.Length == 2)
            {
                speakerName.text = _storyText[0].Trim();
                dialog.text = _storyText[1].Trim();
            }else{
                RefreshView();
            }

            if (story.currentChoices.Count > 0)
            {
                float _canvasWidth = canvas.gameObject.GetComponent<RectTransform>().rect.width;
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
        }
        else
        {
            GameMaster.gm.paused = false;
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
        choice.transform.SetParent(canvas.transform, false);

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
        int childCount = canvas.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            Destroy(canvas.transform.GetChild(i).gameObject);
        }
    }

}