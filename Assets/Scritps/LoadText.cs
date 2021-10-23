using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadText
{
    public string[][] ReadText(EnemyEnum.Enemy enemy, string quest="")
    {
        if (quest == "")
        {
            string[] stringSeparators = new string[] { "---" };
            string[] stringSeparatorshp = new string[] { "~~~" };
            string[][] result;
            var block = Resources.Load(enemy.ToString() + "Text").ToString().Split(stringSeparatorshp, System.StringSplitOptions.None);
            result = new string[block.Length][];



            for (int i = 0; i < block.Length; i++)
            {
                result[i] = (block[i].Split(stringSeparators, System.StringSplitOptions.None));
            }
            return result;
        }
        else
        {
            string[] stringSeparators = new string[] { "---" };
            string[] stringSeparatorshp = new string[] { "~~~" };
            string[][] result;
            var block = Resources.Load("QuestText\\" + quest).ToString().Split(stringSeparatorshp, System.StringSplitOptions.None);
            result = new string[block.Length][];



            for (int i = 0; i < block.Length; i++)
            {
                result[i] = (block[i].Split(stringSeparators, System.StringSplitOptions.None));
            }
            return result;
        }
    }
}
