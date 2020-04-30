using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    // cache
    GameSession gameSession;
    TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Start()
    {
        // cache
        gameSession = FindObjectOfType<GameSession>();
        tmp = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = gameSession.GetScore().ToString();
    }
}
