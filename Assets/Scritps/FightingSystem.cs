using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FightingSystem : MonoBehaviour
{
    Text CountSymbol;
    public GameObject SendMessageButton;
    public static int turn=0;
    public GameObject[] SkillsButton;
    public GameObject BlockCanvas;
    [SerializeField] int FirstSkillDamage = 100;
    public static int usedSkill=-1;
 
    public int CountOfHit { get; set; } = 0;
    int HitSum=0;
    int thirdSkillDamage = 1000;
    public int CountOfHitSecondSkill { get; set; } = 0;
    public int CountOfHitThirdkill { get; set; } = 0;
    public int MaximumHitSecondSkill { get; set; } = 0;
    public int MaximumHitThirdSkill { get; set; } =5;
    GameObject[] cooldownTexts ;
    public static int[] cooldown { get; set; } = { 0, 0, 0, 0 };
    [Header("Player")]
    int health=15000;
    int energy=4000;
    int Maxenergy=4000;
    public int lessEnergy { get; set; } = 0;
    int Maxhealth=15000;
    Text HealthBar;
    Slider HealthBarSlider;
    Slider EnergyBarSlider;
    [Header("SecondSkill")]
    public Sprite[] SecondSkillSpritesArray;
    public GameObject[] SecondSkillSpritesGm;
    public Sprite DefaultIconSecondSkill;
    public Image SecondSkillImage;
    public GameObject[] ButtonHitCountSecondSkill;
    //Думаю стоит создать под него ScriptableObject 
    [Header("Enemy")]
    private int healthEnemy;
    Enemy CurrentEnemy;
    public Text HPText;
    int minSymbol = 1;
    public Slider HPSlider;
    [SerializeField] GameObject EnemyImage;
    [Header("Direction")]
    public GameObject DirectionPanel;
    public Image DirectionImage;
    public Sprite[] DirectionSpritesArray;
    private void Awake()
    {
        SendText.isFight = true;
    }
    private void Start()
    {
        Load();
        InitEnemy();
        CountSymbol = GameObject.Find("CountSymbol").GetComponent<Text>();
        cooldownTexts = new GameObject[SecondSkillSpritesGm.Length];
        HealthBar = GameObject.Find("HealthbarText").GetComponent<Text>();
        HealthBarSlider = GameObject.Find("Healthbar").GetComponent<Slider>();
        EnergyBarSlider = GameObject.Find("Energybar").GetComponent<Slider>();
        EnergyBarSlider.maxValue = Maxenergy;
        EnergyBarSlider.value = energy;
        HealthBar.text = $"{PlayerInfo.PlayerName}[" + health + $"/{Maxhealth}]";
        if (QuestSystem.isQuestStarted && DemonsBond.currentStep<DemonsBond.QuestText[0].Length-1 && healthEnemy==CurrentEnemy.hp)
        {
            SendText.SendLog("<color=yellow>Система</color>: " + DemonsBond.QuestText[0][DemonsBond.currentStep]);
            DemonsBond.currentStep++;
        }
        HealthBarSlider.maxValue = Maxhealth;
       
        HealthBarSlider.value = health;
        for (int i = 0; i < SecondSkillSpritesGm.Length; i++)
        {
            cooldownTexts[i] = SecondSkillSpritesGm[i].transform.GetChild(0).gameObject;
        }
      
        StartCoroutine(BlockScreen());
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey("LastHitDate"))
        {
            TimeSpan date = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("LastHitDate"));
            if (date.Days > 0 && healthEnemy>0)
            {
               
                for (int i = 0; i < date.Days; i++)
                {
                    SendText.SendLog("Система: вы пропустили ход.\n", new Color32(0xEE,0xB1,0x14,0xFF));
                }
            }
        }
    }
    void InitEnemy()
    {
        CurrentEnemy = (Enemy)Resources.Load($"Enemy/Wolf");
        healthEnemy = CurrentEnemy.hp;
        EnemyImage.GetComponent<Image>().sprite = CurrentEnemy.EnemySprite;
        EnemyImage.GetComponent<Image>().color = new Color(EnemyImage.GetComponent<Image>().color.r, EnemyImage.GetComponent<Image>().color.g, EnemyImage.GetComponent<Image>().color.b, 1f);
        HPText.text = $"{CurrentEnemy.name}[" + healthEnemy + $"/{CurrentEnemy.hp}]";
        healthEnemy = CurrentEnemy.hp;
        HPSlider.maxValue = CurrentEnemy.hp;
        HPSlider.value = healthEnemy;
    }
    public void OnMessage()
    {
        CountSymbol.text = $"[{CurrInputField.inputField.text.Length}\\2000]";
        if (CurrInputField.inputField.text.Length >= minSymbol && CountOfHit + CountOfHitSecondSkill+CountOfHitThirdkill > 0 && energy-lessEnergy>0)
        {
            
            SendMessageButton.GetComponent<Button>().enabled = true;
            SendMessageButton.GetComponent<Image>().color = new Color(SendMessageButton.GetComponent<Image>().color.r, SendMessageButton.GetComponent<Image>().color.g, SendMessageButton.GetComponent<Image>().color.b, 1f);

        }
        else
        {

            SendMessageButton.GetComponent<Button>().enabled = false;
            SendMessageButton.GetComponent<Image>().color = new Color(SendMessageButton.GetComponent<Image>().color.r, SendMessageButton.GetComponent<Image>().color.g, SendMessageButton.GetComponent<Image>().color.b, 0.5f);

        }
    }

    public void EndTurn()
    {
        if (healthEnemy > 0)
        {
            turn++;
            HitSum += CountOfHit + CountOfHitSecondSkill;
            if (HitSum >= 100)
            {
                SkillsButton[2].GetComponent<Button>().enabled = true;
                SkillsButton[2].GetComponent<Image>().color = new Color(SkillsButton[2].GetComponent<Image>().color.r, SkillsButton[2].GetComponent<Image>().color.g, SkillsButton[2].GetComponent<Image>().color.b, 1f);
            }
            IncreaseHealth();
            UseSecondSkill();
            UseThirdSkill();
            BreakSecondSkill();
            StartCoroutine(BlockScreen());
            if (healthEnemy < 0)
            {
                OnEnemyDie();
                healthEnemy = 0;
            }
            HPText.text = $"{CurrentEnemy.name}[" + healthEnemy + $"/{CurrentEnemy.hp}]";
            HPSlider.value = healthEnemy;
            SendText.SendLog();
            OpenButton();
            EnergyBarSlider.value = energy;
        }
        else if(DemonsBond.currentStep == 22)
        {
            SceneManager.LoadScene("Portal_Location");
        }
        else if (DemonsBond.currentStep<DemonsBond.QuestText[0].Length-1)
        {
            
            SendText.SendLog();
            StartCoroutine(BlockScreen("<color=yellow>Система</color>: " + DemonsBond.QuestText[0][DemonsBond.currentStep]));
            DemonsBond.currentStep++;

        }
    }

    IEnumerator BlockScreen(string s="")
    {
        BlockCanvas.SetActive(true);
        yield return new WaitForSeconds(3f);
        if (healthEnemy > 0)
        {
            PlayerPrefs.SetString("LastHitDate", DateTime.Now.ToString());
            health -= CurrentEnemy.damage;
            HealthBarSlider.value = health;
            HealthBar.text = "Player[" + health + $"/{Maxhealth}]";
        }
        SendText.EnemyLog((float)healthEnemy/CurrentEnemy.hp, s);
        BlockCanvas.SetActive(false);
       
    }
    private void UseSecondSkill()
    {
        
        int damage=0;
        switch (usedSkill)
        {
            case 0:
                cooldown[0] = 2;
                damage = 50;
                break;
            case 1:
                cooldown[1] = 10;
                damage = 200;
                break;
            case 2:
                cooldown[2] = 3;
                damage = 200;
                break;
            case 3:
                cooldown[3] = 6;
                damage = 300;
                break;
        }
        MaximumHitSecondSkill -= CountOfHitSecondSkill;
        healthEnemy -= damage* CountOfHitSecondSkill;
        energy -= lessEnergy;
        lessEnergy = 0;
        CountOfHitSecondSkill = 0;
        usedSkill = -1;
        for (int i = 0; i < cooldown.Length; i++)
        {
            if (cooldown[i] > 0)
            {
                cooldown[i]--;
                cooldownTexts[i].GetComponent<Text>().text = $"{cooldown[i]}";
                if (cooldown[i] > 0)
                {
                    SecondSkillSpritesGm[i].GetComponent<Button>().enabled = false;
                    SecondSkillSpritesGm[i].GetComponent<Image>().color = new Color(SecondSkillSpritesGm[i].GetComponent<Image>().color.r, SecondSkillSpritesGm[i].GetComponent<Image>().color.g, SecondSkillSpritesGm[i].GetComponent<Image>().color.b, 0.5f);
                }
                else
                {
                    cooldownTexts[i].GetComponent<Text>().text = "";
                    SecondSkillSpritesGm[i].GetComponent<Button>().enabled = true;
                    SecondSkillSpritesGm[i].GetComponent<Image>().color = new Color(SecondSkillSpritesGm[i].GetComponent<Image>().color.r, SkillsButton[1].GetComponent<Image>().color.g, SecondSkillSpritesGm[i].GetComponent<Image>().color.b, 1f);

                }
            }
        }
    }

    private void IncreaseHealth()
    {
        healthEnemy -= FirstSkillDamage * CountOfHit;
        CountOfHit = 0;
    }
    private void BreakSecondSkill()
    {
        SecondSkillImage.sprite = DefaultIconSecondSkill;
 
        usedSkill = -1;
    }
    private void UseThirdSkill()
    {
        if (HitSum >= 100)
        {
            healthEnemy -= CountOfHitThirdkill * thirdSkillDamage;
            if (CountOfHitThirdkill > 0)
            {
                HitSum = 0;
                CountOfHitThirdkill = 0;
                SkillsButton[2].GetComponent<Button>().enabled = false;
                SkillsButton[2].GetComponent<Image>().color = new Color(SkillsButton[2].GetComponent<Image>().color.r, SkillsButton[2].GetComponent<Image>().color.g, SkillsButton[2].GetComponent<Image>().color.b, 0.5f);
            }
        }
    }
    public void ChangeDirection(string dir)
    {
        if (dir == "up")
        {
            Debug.Log("Up");
            OpenerDirectionPanel(false);
            DirectionImage.sprite = DirectionSpritesArray[0];
        }

        if (dir == "down")
        {
            Debug.Log("down");
            OpenerDirectionPanel(false);
            DirectionImage.sprite = DirectionSpritesArray[1];
        }

        if (dir == "right")
        {
            Debug.Log("right");
            OpenerDirectionPanel(false);
            DirectionImage.sprite = DirectionSpritesArray[2];
        }

        if (dir == "left")
        {
            Debug.Log("left");
            OpenerDirectionPanel(false);
            DirectionImage.sprite = DirectionSpritesArray[3];
        }
    }

    public void ChangeSecondSkill(int i)
    {
        SecondSkillImage.sprite = SecondSkillSpritesArray[i];
        usedSkill = i;
        switch (i)
        {
            case 0:
                
                MaximumHitSecondSkill = 16;
                lessEnergy = 100;
                break;
            case 1:
                
                MaximumHitSecondSkill = 21;
                lessEnergy = 400;
                break;
            case 2:
               
                MaximumHitSecondSkill = 13;
                lessEnergy = 200;
                break;
            case 3:
                
                MaximumHitSecondSkill = 13;
                lessEnergy = 300;
                break;
        }
        for (int j= 0; j < 21; j++)
        {
            if (!ButtonHitCountSecondSkill[j].activeSelf && j<MaximumHitSecondSkill)
            ButtonHitCountSecondSkill[j].SetActive(true);
            else if(j>= MaximumHitSecondSkill)
                ButtonHitCountSecondSkill[j].SetActive(false);
        }
    }

    public void OpenerDirectionPanel(bool state)
    {
        DirectionPanel.SetActive(state);
    }

    public void BlockSkill(int i)
    {
        if (healthEnemy > 0) {
            for (int j = 0; j < SkillsButton.Length; j++)
            {
                if (j != i)
                {
                    SkillsButton[j].GetComponent<Button>().enabled = false;
                    SkillsButton[j].GetComponent<Image>().color = new Color(SkillsButton[j].GetComponent<Image>().color.r, SkillsButton[j].GetComponent<Image>().color.g, SkillsButton[j].GetComponent<Image>().color.b, 0.5f);
                }
            }    
        }
    }


    void OnEnemyDie()
    {
        switch (CurrentEnemy.TypeEnemy)
        {
            case EnemyEnum.Enemy.Wolf:
                OnWolfDie();
                break;
            default:
                break;
        }
    }
    public void OpenButton()
    {
        
        for (int j = 0; j < SkillsButton.Length; j++)
        {
            if ((j!=2 || HitSum>=100))
            {
                SkillsButton[j].GetComponent<Button>().enabled = true;
                SkillsButton[j].GetComponent<Image>().color = new Color(SkillsButton[j].GetComponent<Image>().color.r, SkillsButton[j].GetComponent<Image>().color.g, SkillsButton[j].GetComponent<Image>().color.b, 1f);
            }
        }
    }


    void OnWolfDie()
    {
        if(QuestSystem.isQuestStarted && DemonsBond.currentStep < DemonsBond.QuestText[0].Length - 1)
        {
           StartCoroutine(BlockScreen("<color=yellow>Система</color>: " + DemonsBond.QuestText[0][DemonsBond.currentStep]));
            DemonsBond.currentStep++;
        }
    }
}