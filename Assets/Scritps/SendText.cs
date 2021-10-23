using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendText : MonoBehaviour
{
    static public bool isFight = false;
    static GameObject FightLog;
    static GameObject LastLogPlayer;
    static string[][] EnemyText;
    void Awake()
    {
        FightLog = GameObject.Find("FightLog");
    }
    private void Start()
    {
        if (isFight)
        {
            LoadText lt = new LoadText();
            EnemyText = lt.ReadText(EnemyEnum.Enemy.Wolf);
        }
    }
    public static void SendLog(string text ="", Color color=default)
    {
       // GUIStyle style = new GUIStyle();
      //  style.richText = true;
      //  GUILayout.Label("<size=30>Some  text</size>", style);
        if (color == default)
            color = Color.white;
        Text Text = new GameObject().AddComponent<Text>();
        if (text == "")
        {
            Text.text = $"<color=green>{PlayerInfo.PlayerName}</color>: " + CurrInputField.inputField.text + '\n';
            CurrInputField.inputField.text = null;
        }
        else
            Text.text = text;
        Text.fontSize = 75;
        Text.gameObject.name = $"Player Log {FightingSystem.turn}";
        Text.raycastTarget = false;
        Text.color = color;
        Text.gameObject.transform.SetParent(FightLog.transform, false);
        Text.font = Font.CreateDynamicFontFromOSFont("Arial", 75);
        Text.verticalOverflow = VerticalWrapMode.Overflow;
    }

    static int previosRandom=-1;
    static int previosRandom1=-1;
    public static void EnemyLog(float hpPercent, string s="")
    {
        Text Text = new GameObject().AddComponent<Text>();
        if (s == "")
        {
            int stage;
            switch (hpPercent)
            {
                case float x when x > 0.69f:
                    stage = 0;
                    break;
                case float x when x > 0.3f:
                    stage = 1;
                    break;
                case float x when x > 0f:
                    stage = 2;
                    break;
                case float x when x <= 0f:
                    stage = 3;
                    break;
                default:
                    stage = 0;
                    break;
            }

            
            int u = Random.Range(0, EnemyText[stage].Length);
            while (previosRandom == u || previosRandom1 == u)
            {
                u = Random.Range(0, EnemyText[stage].Length);
            }

            previosRandom1 = previosRandom;
            previosRandom = u;

            Text.text = "<color=red>Wolf</color>: " + EnemyText[stage][u];
        }
        else
        {
            Text.text = s;
        }
        Text.fontSize = 75;
        Text.verticalOverflow = VerticalWrapMode.Overflow;
        Text.gameObject.name = $"Enemy Log {FightingSystem.turn}";
        Text.raycastTarget = false;
        Text.color = Color.white;
        Text.gameObject.transform.SetParent(FightLog.transform, false);
        Text.font = Font.CreateDynamicFontFromOSFont("Arial", 75);
    }
}
