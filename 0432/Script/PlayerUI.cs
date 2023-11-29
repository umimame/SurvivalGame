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
    private void Start()
    {
        scoreText.SetText(body.score.ToString());
        AnchorSet();
    }

    private void Update()
    {
        scoreText.SetText("Score:" + body.score.ToString("d6"));
        hpGuageVariable.OnChangeValue(body.hp.entity / body.hp.max);
        stGuageVariable.OnChangeValue(body.stamina.entity / body.stamina.max);
    }

    private void AnchorSet()
    {
        if(body.tag == Tags.Player01)
        {
            AddFunction.SetAnchor(scoreText.rectTransform, AddFunction.AnchorPresets.TopLeft);
            AddFunction.SetPivot(scoreText.rectTransform, AddFunction.PivotPresets.TopLeft);

        }
        else if(body.tag == Tags.Player02) 
        {
            AddFunction.SetAnchor(scoreText.rectTransform, AddFunction.AnchorPresets.BottomLeft);
            AddFunction.SetPivot(scoreText.rectTransform, AddFunction.PivotPresets.BottomLeft);
        }
    }
}
