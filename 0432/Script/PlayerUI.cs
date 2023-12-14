using AddClass;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Chara_Player body;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private FillAmoutGuage hpGuageVariable;
    [SerializeField] private FillAmoutGuage stGuageVariable;
    [SerializeField] private FillAmoutGuage nestGuageVariable;
    private void Start()
    {
        scoreText.SetText(body.score.ToString());
        AnchorSet();
        nestGuageVariable.transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {
        scoreText.SetText("Score:" + body.score.ToString("d6"));
        hpGuageVariable.OnChangeValue(body.hp.entity / body.hp.max);
        stGuageVariable.OnChangeValue(body.stamina.entity / body.stamina.max);

    }

    public void ControlNestGuage(float ratio)
    {
        if(ratio <= 0)
        {
            nestGuageVariable.transform.parent.gameObject.SetActive(false);
            nestGuageVariable.OnChangeValue((float)ratio);
        }
        else
        {
            nestGuageVariable.transform.parent.gameObject.SetActive(true);
            nestGuageVariable.OnChangeValue((float)ratio);

        }
    }

    private void AnchorSet()
    {
        if(body.tag == Tags.Player01)
        {
            Set(scoreText.rectTransform, AnchorPresets.TopLeft, PivotPresets.TopLeft);
            
            RectTransform rect = stGuageVariable.transform.parent.GetComponent<RectTransform>();

            Set(rect, AnchorPresets.BottomLeft, PivotPresets.BottomLeft);
            rect = nestGuageVariable.transform.parent.GetComponent<RectTransform>();
            Set(rect, AnchorPresets.TopCenter, PivotPresets.TopCenter);

        }
        else if(body.tag == Tags.Player02)
        {
            Set(scoreText.rectTransform, AnchorPresets.BottomLeft, PivotPresets.BottomLeft);

            RectTransform rect = stGuageVariable.transform.parent.GetComponent<RectTransform>();

            Set(rect, AnchorPresets.TopLeft, PivotPresets.TopLeft);
            rect = nestGuageVariable.transform.parent.GetComponent<RectTransform>();
            Set(rect, AnchorPresets.BottonCenter, PivotPresets.BottomCenter);
        }

        void Set(RectTransform rect,AnchorPresets anchor, PivotPresets pivot)
        {
            AddFunction.SetAnchor(rect, anchor);
            AddFunction.SetPivot(rect, pivot);
        }
    }
}
