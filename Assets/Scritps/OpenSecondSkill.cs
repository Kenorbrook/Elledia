using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSecondSkill : MonoBehaviour
{
    [Header("Second Menu")]
    public GameObject ChoiceSkil;
    public GameObject ChoiceHitCount;
    public void OpenSecondSkillBut()
    {
       
            ChoiceSkil.SetActive(true);
    }
}
