using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestSystem : MonoBehaviour
{
    public string StartedQuest { get; set; }
    static public bool isQuestStarted = false;
    [Header("Panels")]
    public GameObject mapPanel;
    public GameObject EventPanel;
    public GameObject DopPanel;
    public GameObject DescriptionPanel;
    public GameObject LoadingScreen;
    public GameObject EnterNamePanel;
    public GameObject BlockCanvas;
    [Header("Other")]
    public Text panelText;
    public Text inputfield;
    private bool isDescMenuOpened;
    private void Start()
    {
        BlockCanvas = GameObject.Find("BlockCanvas");
        BlockCanvas.SetActive(false);
        if (isQuestStarted && (DemonsBond.currentStep == 6|| DemonsBond.currentStep == 22))
        {
            StartCoroutine(WaitAnswer());
            DemonsBond.currentStep++;
        }
    }
    public void EnableMapPanel(bool state)
    {
        mapPanel.SetActive(state);
    }

    public void EnableDopPanel(bool state)
    {
        DopPanel.GetComponent<Animator>().SetBool("IsPanelOpened", state);
    }

    public void EnableDescriptionPanel()
    {
        isDescMenuOpened = !isDescMenuOpened;

        DescriptionPanel.GetComponent<Animator>().SetBool("DescOpened", isDescMenuOpened);
    }

    public void changeLoc(int i)
    {
        Location.CurrLocation = (Location.Locations)i;
    }
    public void EnableEventPanel(bool state)
    {
        EventPanel.SetActive(state);
    }
    public void StartQuest()
    {
        isQuestStarted = true;
        LoadText lt = new LoadText();
        DemonsBond.QuestText = lt.ReadText(EnemyEnum.Enemy.Wolf, StartedQuest);
        GameObject.Find("DemonsBond").GetComponent<Button>().enabled = false;
        DemonsBond.currentStep = 0;
        SendText.SendLog("<color=#910BB2>Квест</color>: " + DemonsBond.QuestText[0][DemonsBond.currentStep]);
        
    }
    public static void OpenMap()
    {
        GameObject.Find("MapButt").GetComponentsInChildren<Image>()[1].color = new Color(1, 1, 1, 1);
        GameObject.Find("MapButt").GetComponent<Button>().enabled = true;
    }
    public void SendMessage()
    {
        if (!EnterNamePanel.activeSelf && inputfield.text != "")
        {
            if (isQuestStarted && DemonsBond.currentStep != 6)
            {
                if (!(DemonsBond.currentStep > 6 && Location.CurrLocation != Location.Locations.Ragnar))
                {
                    SendText.SendLog();
                    StartCoroutine(WaitAnswer());
                    DemonsBond.currentStep++;

                }
                if (DemonsBond.currentStep == 6)
                {
                    OpenMap();
                }
                else if (DemonsBond.currentStep == 18)
                {
                    OpenScene("FightingScene");
                }
               
            }
            else if(!isQuestStarted)
            {

                SendText.SendLog("\n" + "Мечник: " + inputfield.transform.parent.GetComponent<InputField>().text);
                inputfield.transform.parent.GetComponent<InputField>().text = null;
                inputfield.text = null;
            }
        }
    }
    void EnterName()
    {
        EnterNamePanel.SetActive(true);
    }

    public void ConfirmName()
    {

        EnterNamePanel.SetActive(false);
        PlayerInfo.PlayerName = EnterNamePanel.GetComponentInChildren<InputField>().text;
        SendText.SendLog($"<color=green>{PlayerInfo.PlayerName}</color>: моё имя - {PlayerInfo.PlayerName}.\n");
        DemonsBond.currentStep++;
        StartCoroutine(WaitAnswer());
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void OpenScene(string name)
    {
        LoadingScreen.GetComponent<Animator>().SetBool("IsLoading", true);
        StartCoroutine(LoadScene(name));
    }
    IEnumerator LoadScene(string name)
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(name);
    }
    IEnumerator WaitAnswer()
    {

        BlockCanvas.SetActive(true);
        yield return new WaitForSeconds(3f);
        SendText.SendLog("<color=yellow>Система</color>: " + DemonsBond.QuestText[0][DemonsBond.currentStep]);
        BlockCanvas.SetActive(false);
        if (DemonsBond.currentStep == 2)
        {
            EnterName();
        }
        if(DemonsBond.currentStep==DemonsBond.QuestText[0].Length-1)
        {
            isQuestStarted = false;
            GameObject.Find("DemonsBond").SetActive(false);
        }
    }
}
