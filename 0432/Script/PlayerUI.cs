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
    private Image nestImage;
    [SerializeField] private Color nestGuageColor1;
    [SerializeField] private Color nestGuageColor2;

    [SerializeField] private EnableAndFadeAlpha leaveText;
    [SerializeField] private EnableAndFadeAlpha stealText;

    private RectTransform rect;
    private void Start()
    {
        scoreText.SetText(body.score.ToString());
        AnchorSet();
        nestGuageVariable.transform.parent.gameObject.SetActive(false);
        nestImage = nestGuageVariable.GetComponent<Image>();
        leaveText.Initialize();
    }

    private void Update()
    {
        scoreText.SetText("Score:" + body.score.plan.ToString("d4"));
        hpGuageVariable.OnChangeValue(body.hp.entity / body.hp.max);
        stGuageVariable.OnChangeValue(body.stamina.entity / body.stamina.max);
        leaveText.Update();
    }

    public void ControlNestGuage(float ratio, bool leave)
    {
        nestGuageVariable.OnChangeValue((float)ratio);
        if (ratio <= 0)
        {
            nestGuageVariable.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            if (leave == true) { nestImage.color = nestGuageColor1; }
            else { nestImage.color = nestGuageColor2; }
            nestGuageVariable.transform.parent.gameObject.SetActive(true);
        }
    }

    public void NestTextLaunch(bool leave)
    {
        if(leave == true)
        {
            leaveText.Launch();
            Debug.Log(leaveText.img.Alpha);
        }
        else
        {
            stealText.Launch();
        }
    }

    private void AnchorSet()
    {
        if(body.tag == Tags.Player01)
        {
            rect = scoreText.rectTransform;
            Set(AnchorPresets.TopLeft, PivotPresets.TopLeft);
            
            rect = stGuageVariable.transform.parent.GetComponent<RectTransform>();
            Set(AnchorPresets.BottomLeft, PivotPresets.BottomLeft);

            rect = nestGuageVariable.transform.parent.GetComponent<RectTransform>();
            Set(AnchorPresets.TopCenter, PivotPresets.TopCenter);

            rect = leaveText.obj.GetComponent<TextMeshProUGUI>().rectTransform;
            Set(AnchorPresets.TopCenter, PivotPresets.TopCenter);

        }
        else if(body.tag == Tags.Player02)
        {
            rect = scoreText.rectTransform;
            Set(AnchorPresets.BottomLeft, PivotPresets.BottomLeft);

            rect = stGuageVariable.transform.parent.GetComponent<RectTransform>();
            Set(AnchorPresets.TopLeft, PivotPresets.TopLeft);

            rect = nestGuageVariable.transform.parent.GetComponent<RectTransform>();
            Set(AnchorPresets.BottonCenter, PivotPresets.BottomCenter);

            rect = leaveText.obj.GetComponent<TextMeshProUGUI>().rectTransform;
            Set(AnchorPresets.BottonCenter, PivotPresets.BottomCenter);
        }

        void Set(AnchorPresets anchor, PivotPresets pivot, bool reverse = false)
        {
            AddFunction.SetAnchor(rect, anchor);
            AddFunction.SetPivot(rect, pivot);
            if(reverse == true)
            {
                
            }
        }
    }
}
