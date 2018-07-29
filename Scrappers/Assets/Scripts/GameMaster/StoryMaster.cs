using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Ink.Runtime;

public class StoryMaster : MonoBehaviour {

    public static StoryMaster sm;
    public Story story;

    [SerializeField]
    private RectTransform textBox;
    [SerializeField]
    private Text text;
    public bool displayTop;
    public bool reading
    {
        get
        {
            return textBox.gameObject.activeSelf;
        }
    }
    private void Awake()
    {
        if (sm == null)
            sm = GameObject.FindGameObjectWithTag("GM").GetComponent<StoryMaster>();
    }
    public static void StartStory(TextAsset _inkJSONAsset)
    {
        sm.story = new Story(_inkJSONAsset.text);
        sm.RefreshView();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RefreshView();
        }

        float anchor = displayTop ? 1 : 0;
        textBox.pivot = new Vector2(textBox.pivot.x, anchor);
        textBox.anchorMin = new Vector2(textBox.anchorMin.x, anchor);
        textBox.anchorMax = new Vector2(textBox.anchorMin.x, anchor);
    }

    public void RefreshView()
    {
        if (story.canContinue)
        {
            textBox.gameObject.SetActive(true);
            text.text = story.Continue().Trim();
        }
        else
        {
            HideView();
        }
    }

    void HideView()
    {
        textBox.gameObject.SetActive(false);
    }
}