using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoresUI : MonoBehaviour
{
    [SerializeField] private Text _scoresText;

    private string SCORES_MESSAGE = "Enemies defeated\n";

    private void OnEnable()
    {
        _scoresText = GetComponentInChildren<Text>();
        _scoresText.text = $"{SCORES_MESSAGE}0";

        Game.OnUpdateScores += UpdateScoresText;
    }

    public void UpdateScoresText(int scores)
    {
        _scoresText.text = $"{SCORES_MESSAGE}{scores}";
    }
}
