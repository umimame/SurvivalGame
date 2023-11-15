using My;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Chara_Player body;
    [SerializeField] private TextMeshProUGUI scoreText;
    private void Start()
    {
        scoreText.SetText(body.score.ToString());
        AnchorSet();
    }

    private void Update()
    {

        scoreText.SetText("Score:" + body.score.ToString());
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
