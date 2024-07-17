using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Assets.SimpleLocalization.Scripts;

public enum MatchState
{
    attack,
    defence,
}
public class MatchEngine : MonoBehaviour
{
    MatchState matchState;
    public EventMatch eventMatch;

    [Header("MATCH STATS")]
    [SerializeField] private int myScore=0;
    [SerializeField] private int opponentScore=0;
    [Header("MY TEAM ELEMENTS")]
     float myTeamDefGen;
     float myTeamMidGen;
     float myTeamForGen;
     float myTeamTotalGen;

    [Header("OPPONENT TEAM ELEMENTS")]
     float opponentDefGen;
     float opponentMidGen;
     float opponentForGen;
     float opponentTotalGen;

    [Header("ATTACK STATS")]
    float pass;
    float dribble;
    float shoot;
    float firstPassRate;
    float firstDribbleRate;
    float firstShootRate;
    public static Action onAttack;
    int attackMove=3;

    [Header("DEFENCE STATS")]
    float press;
    float grab;
    float firstPressRate;
    float firstGrabRate;
    public static Action onDefence;
    int defenceMove=3;

    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI defenceMoveText;
    [SerializeField] private TextMeshProUGUI attackMoveText;
    [SerializeField] private TextMeshProUGUI pressRateText;
    [SerializeField] private TextMeshProUGUI grabRateText;
    [SerializeField] private TextMeshProUGUI passRateText;
    [SerializeField] private TextMeshProUGUI dribbleRateText;
    [SerializeField] private TextMeshProUGUI shootRateText;
    [SerializeField] private TextMeshProUGUI stateText;
    [SerializeField] private TextMeshProUGUI myScoreText;
    [SerializeField] private TextMeshProUGUI opponentScoreText;
    [SerializeField] private Slider moveAttackSlider;
    [SerializeField] private Slider moveDefenceSlider;
    [SerializeField] private TextMeshProUGUI moveSliderText;
    [SerializeField] private GameObject attackPanel;
    [SerializeField] private GameObject defencePanel;
    [SerializeField] private GameObject[] infoPanels;
    [SerializeField] private AnnouncerPrefabs announcerPrefabs;
    [SerializeField] private Transform announcerParents;
    [SerializeField] private GameObject announcerPanel;
    [SerializeField] private GameObject AttackTimePanel;
    [SerializeField] private GameObject DefenceTimePanel;
    [SerializeField] private GameObject attackButtonPanel;
    [SerializeField] private GameObject defenceButtonPanel;
    [SerializeField] private GameObject animPanel;
    [SerializeField] private TextMeshProUGUI announcerText;
    [Header("ANNOUNCER TR")]
    [SerializeField] private string[] announcerPressSuccessfulTR;
    [SerializeField] private string[] announcerPressUnsuccessfulTR;
    [SerializeField] private string[] announcerGrabSuccessfulTR;
    [SerializeField] private string[] announcerGrabUnsuccessfulTR;
    [SerializeField] private string[] announcerPassSuccessfulTR;
    [SerializeField] private string[] announcerPassUnsuccessfulTR;
    [SerializeField] private string[] announcerDribbleSuccessfulTR;
    [SerializeField] private string[] announcerDribbleUnsuccessfulTR;
    [SerializeField] private string[] announcerShootSuccessfulTR;
    [SerializeField] private string[] announcerShootUnsuccessfulTR;
    [SerializeField] private string[] announcerMoveTR;
    [Header("ANNOUNCER EN")]
    [SerializeField] private string[] announcerPressSuccessfulEN;
    [SerializeField] private string[] announcerPressUnsuccessfulEN;
    [SerializeField] private string[] announcerGrabSuccessfulEN;
    [SerializeField] private string[] announcerGrabUnsuccessfulEN;
    [SerializeField] private string[] announcerPassSuccessfulEN;
    [SerializeField] private string[] announcerPassUnsuccessfulEN;
    [SerializeField] private string[] announcerDribbleSuccessfulEN;
    [SerializeField] private string[] announcerDribbleUnsuccessfulEN;
    [SerializeField] private string[] announcerShootSuccessfulEN;
    [SerializeField] private string[] announcerShootUnsuccessfulEN;
    [SerializeField] private string[] announcerMoveEN;
    [SerializeField] private Animator animator;


   


    private void Start()
    {
        AttackState();
        moveAttackSlider.maxValue = 3;
        moveDefenceSlider.maxValue = 3;
        myScoreText.text = myScore.ToString();
        opponentScoreText.text = opponentScore.ToString();

        attackButtonPanel.SetActive(true);
        defenceButtonPanel.SetActive(false);
    }

    private void Update()
    {
        BalanceButton();

        if (announcerParents.childCount > 6)
        {
            Destroy(announcerParents.GetChild(0).gameObject);
        }

        if (attackMove <= 0 && matchState != MatchState.defence)
        {
            DefenceState();
            StartCoroutine(PanelTime(DefenceTimePanel));
            if (LocalizationManager.Language == "Türkçe")
            {
                AnnouncerText(announcerMoveTR[0]);

            }
            else if (LocalizationManager.Language == "English")
            {
                AnnouncerText(announcerMoveEN[0]);
            }

        }
        else if (defenceMove <= 0 && matchState != MatchState.attack)
        {
            AttackState();
            StartCoroutine(PanelTime(AttackTimePanel));
            if (LocalizationManager.Language == "Türkçe")
            {
                AnnouncerText(announcerMoveTR[1]);

            }
            else if (LocalizationManager.Language == "English")
            {
                AnnouncerText(announcerMoveEN[1]);
            }
        }

        if (myScore >= 3 || opponentScore >= 3)
        {
            if (eventMatch.isEventMatch)
            {

                for (int i = 0; i < announcerParents.childCount; i++)
                {
                    Destroy(announcerParents.GetChild(i).gameObject);
                }
                eventMatch.EventMatchOver(myScore,opponentScore);
                return;
            }
            MatchManager.instance.matchScene.SetActive(false);
            MatchManager.instance.MatchEnd(myScore, opponentScore);
            myScore = 0;
            opponentScore = 0;
            myScoreText.text = myScore.ToString();
            opponentScoreText.text = opponentScore.ToString();


            for (int i = 0; i < announcerParents.childCount; i++)
            {
                Destroy(announcerParents.GetChild(i).gameObject);
            }

        }

        EnumState();
    }

    private void BalanceButton()
    {
        if (pass >= 100)
            pass = 100;
        if (dribble >= 100)
            dribble = 100;
        if (shoot >= 100)
            shoot = 100;
        if (grab >= 100)
            grab = 100;
        if (press >= 100)
            press = 100;

        RateTextUpdate();
    }

    public void OpponentTeamConfig(float DefGen, float MidGen, float ForGen)
    {
        opponentDefGen = DefGen;
        opponentMidGen= MidGen;
        opponentForGen = ForGen;
        opponentTotalGen = opponentDefGen + opponentForGen + opponentMidGen;
    }
    public void MyTeamConfig(float DefGen, float MidGen, float ForGen)
    {
        myTeamDefGen = DefGen;
        myTeamMidGen = MidGen;
        myTeamForGen = ForGen;
        myTeamTotalGen = myTeamDefGen + myTeamForGen + myTeamMidGen;
    }

    public void EnumState()
    {
        switch (matchState)
        {
            case MatchState.attack:
                stateText.text = matchState.ToString();
                
                onAttack?.Invoke();
                break;
            case MatchState.defence:
                stateText.text = matchState.ToString();
                
                onDefence?.Invoke();
                break;
            default:
                break;
        }
    }

    public void CalculateRate()
    {
        pass = (myTeamTotalGen / (myTeamTotalGen + opponentTotalGen))*100f + 10;
        dribble = ((myTeamMidGen + myTeamForGen) / (myTeamMidGen + myTeamForGen + opponentMidGen + opponentDefGen))*100f+10;
        shoot = (myTeamForGen / (myTeamForGen + opponentDefGen)) * 100f + 10;
        press = (myTeamTotalGen / (myTeamTotalGen + opponentTotalGen))*100f+10 ;
        grab = (myTeamForGen / (myTeamForGen + opponentDefGen)) * 100f + 10;

        firstPassRate = pass;
        firstDribbleRate = dribble;
        firstShootRate = shoot;
        firstPressRate = press;
        firstGrabRate = grab;

        RateTextUpdate();

        Debug.Log(pass);
        Debug.Log(dribble);
        Debug.Log(shoot);
        Debug.Log(press);
        Debug.Log(grab);
    }
    public void AttackState()
    {
        matchState = MatchState.attack; 

        stateText.text = matchState.ToString();
        attackPanel.SetActive(true);
        defencePanel.SetActive(false);
        attackMove = 3;
        attackMoveText.text = attackMove.ToString();
        moveAttackSlider.value = attackMove;
        Debug.Log("deneme1");

    }
    public void DefenceState()
    {
        
            matchState = MatchState.defence;
            stateText.text = matchState.ToString();
            defencePanel.SetActive(true);
            attackPanel.SetActive(false);
            defenceMove = 3;
            defenceMoveText.text = defenceMove.ToString();
            moveDefenceSlider.value = defenceMove;

        Debug.Log("deneme2");
        
            
    }
    public IEnumerator PressButton()
    {
        float RandomRate = UnityEngine.Random.Range(0, 100);
        int announcerText= UnityEngine.Random.Range(0, announcerPressSuccessfulTR.Length);
        int announcerTextEN= UnityEngine.Random.Range(0, announcerPressSuccessfulEN.Length);
        int announcerUnText= UnityEngine.Random.Range(0, announcerPressUnsuccessfulTR.Length);
        int announcerUnTextEN= UnityEngine.Random.Range(0, announcerPressUnsuccessfulEN.Length);
        Debug.Log(RandomRate);
        if (RandomRate < press)
        {
            DefencePanelActive();
            yield return new WaitForSeconds(1f);
            DefencePanelActive();
            DefenceRate(10f, 5f);
            defenceMove--;          
            MoveTextUpdate();
            if (LocalizationManager.Language == "Türkçe")
            {
            AnnouncerText(announcerPressSuccessfulTR[announcerText]);

            }else if (LocalizationManager.Language == "English")
            {
                AnnouncerText(announcerPressSuccessfulEN[announcerTextEN]);
            }
        }
        else
        {
            DefencePanelActive();
            yield return new WaitForSeconds(1f);
            DefencePanelActive();
            DefenceRate(-5f, -2f);
            defenceMove--;

            if (defenceMove < 1)
            {   opponentScore += 1;
                opponentScoreText.text = opponentScore.ToString();
                press = firstPressRate;
                grab = firstGrabRate;
                AttackState();

                if (LocalizationManager.Language == "Türkçe")
                {
                    AnnouncerText(announcerGrabUnsuccessfulTR[announcerUnText]);

                }
                else if (LocalizationManager.Language == "English")
                {
                    AnnouncerText(announcerGrabUnsuccessfulEN[announcerUnTextEN]);
                }
                StartCoroutine(PanelTime(AttackTimePanel));
            }
            else
            {
                MoveTextUpdate();
                if (LocalizationManager.Language == "Türkçe")
                {
                    AnnouncerText(announcerPressUnsuccessfulTR[announcerText]);

                }
                else if (LocalizationManager.Language == "English")
                {
                    AnnouncerText(announcerPressUnsuccessfulEN[announcerUnTextEN]);
                }
            }
            
        }
        RateTextUpdate();
    }
    public void PressButtonn()
    {
        StartCoroutine(PressButton());
    }

    public IEnumerator GrabButton()
    {
        float RandomRate = UnityEngine.Random.Range(0, 100);
        int announcerText = UnityEngine.Random.Range(0, announcerGrabSuccessfulTR.Length);
        int announcerTextEN = UnityEngine.Random.Range(0, announcerGrabSuccessfulEN.Length);
        int announcerUnText = UnityEngine.Random.Range(0, announcerGrabUnsuccessfulTR.Length);
        int announcerUnTextEN = UnityEngine.Random.Range(0, announcerGrabUnsuccessfulEN.Length);
        Debug.Log(RandomRate);

        if (RandomRate < grab)
        {
            DefencePanelActive();
            yield return new WaitForSeconds(1f);
            AttackState();
            if (LocalizationManager.Language == "Türkçe")
            {
                AnnouncerText(announcerGrabSuccessfulTR[announcerText]);

            }
            else if (LocalizationManager.Language == "English")
            {
                AnnouncerText(announcerGrabSuccessfulEN[announcerTextEN]);
            }
            AttackTimePanel.SetActive(true);
            yield return new WaitForSeconds(1.2f);
            AttackTimePanel.SetActive(false);
            MoveTextUpdate();
        }
        else
        {
            DefencePanelActive();
            yield return new WaitForSeconds(1f);
            opponentScore += 1;
            opponentScoreText.text = opponentScore.ToString();
            press = firstPressRate;
            grab = firstGrabRate;
            AttackState();
            if (LocalizationManager.Language == "Türkçe")
            {
                AnnouncerText(announcerGrabUnsuccessfulTR[announcerUnText]);

            }
            else if (LocalizationManager.Language == "English")
            {
                AnnouncerText(announcerGrabUnsuccessfulEN[announcerUnTextEN]);
            }
            StartCoroutine(PanelTime(AttackTimePanel));
        }
        RateTextUpdate();
    }
    public void GrabButtonn()
    {
        StartCoroutine(GrabButton());
    }
    public IEnumerator PassButton()
    {
        float RandomRate = UnityEngine.Random.Range(0, 100);
        int announcerText = UnityEngine.Random.Range(0, announcerPassSuccessfulTR.Length);
        int announcerTextEN = UnityEngine.Random.Range(0, announcerPassSuccessfulEN.Length);
        int announcerUnText = UnityEngine.Random.Range(0, announcerPassUnsuccessfulTR.Length);
        int announcerUnTextEN = UnityEngine.Random.Range(0, announcerPassUnsuccessfulEN.Length);
        Debug.Log(RandomRate);

        if (RandomRate < pass)
        {
            AttackPanelActive();
            yield return new WaitForSeconds(1f);
            AttackPanelActive();
            AttackRate(10f, 10f, 8f);
            attackMove--;
            if (LocalizationManager.Language == "Türkçe")
            {
                AnnouncerText(announcerPassSuccessfulTR[announcerText]);

            }
            else if (LocalizationManager.Language == "English")
            {
                AnnouncerText(announcerPassSuccessfulEN[announcerTextEN]);
            }
            MoveTextUpdate();

        }
        else
        {
            AttackPanelActive();
            yield return new WaitForSeconds(1f);
            AttackRate(-5f, -3f, -2f);
            DefenceState();
            if (LocalizationManager.Language == "Türkçe")
            {
                AnnouncerText(announcerPassUnsuccessfulTR[announcerUnText]);

            }
            else if (LocalizationManager.Language == "English")
            {
                AnnouncerText(announcerPassUnsuccessfulEN[announcerUnTextEN]);
            }
            StartCoroutine(PanelTime(DefenceTimePanel));
            MoveTextUpdate();
        }
        
        RateTextUpdate();
    }
    public void PassButtonn()
    {
        StartCoroutine(PassButton());
    }
    public IEnumerator DribbleButton()
    {
        float RandomRate = UnityEngine.Random.Range(0, 100);
        int announcerText = UnityEngine.Random.Range(0, announcerDribbleSuccessfulTR.Length);
        int announcerTextEN = UnityEngine.Random.Range(0, announcerDribbleSuccessfulEN.Length);
        int announcerUnText = UnityEngine.Random.Range(0, announcerDribbleUnsuccessfulTR.Length);
        int announcerUnTextEN = UnityEngine.Random.Range(0, announcerDribbleUnsuccessfulEN.Length);
        Debug.Log(RandomRate);

        if (RandomRate < dribble)
        {
            AttackPanelActive();
            yield return new WaitForSeconds(1f);
            AttackPanelActive();
            AttackRate(5f, 10f, 10f);
            attackMove--;

            if (LocalizationManager.Language == "Türkçe")
            {
                AnnouncerText(announcerDribbleSuccessfulTR[announcerText]);

            }
            else if (LocalizationManager.Language == "English")
            {
                AnnouncerText(announcerDribbleSuccessfulEN[announcerTextEN]);
            }
            MoveTextUpdate();
        }
        else
        {
            AttackPanelActive();
            yield return new WaitForSeconds(1f);
            AttackRate(-5f, -5f, -5f);
            DefenceState();

            if (LocalizationManager.Language == "Türkçe")
            {
                AnnouncerText(announcerDribbleUnsuccessfulTR[announcerUnText]);

            }
            else if (LocalizationManager.Language == "English")
            {
                AnnouncerText(announcerDribbleUnsuccessfulEN[announcerUnTextEN]);
            }
            StartCoroutine(PanelTime(DefenceTimePanel));
            MoveTextUpdate();
        }
        RateTextUpdate();
    }
    public void DribbleButtonn()
    {
        StartCoroutine(DribbleButton());
    }

    public IEnumerator ShootButton()
    {
        float RandomRate = UnityEngine.Random.Range(0, 100);
        int announcerText = UnityEngine.Random.Range(0, announcerShootSuccessfulTR.Length);
        int announcerTextEN = UnityEngine.Random.Range(0, announcerShootSuccessfulEN.Length);
        int announcerUnText = UnityEngine.Random.Range(0, announcerShootUnsuccessfulTR.Length);
        int announcerUnTextEN = UnityEngine.Random.Range(0, announcerShootUnsuccessfulEN.Length);
        Debug.Log(RandomRate);

        if (RandomRate < shoot)
        {
            AttackPanelActive();
            yield return new WaitForSeconds(1f);
            AttackPanelActive();
            myScore += 1;
            myScoreText.text = myScore.ToString();
            if (LocalizationManager.Language == "Türkçe")
            {
                AnnouncerText(announcerShootSuccessfulTR[announcerText]);

            }
            else if (LocalizationManager.Language == "English")
            {
                AnnouncerText(announcerShootSuccessfulEN[announcerTextEN]);
            }
            pass = firstPassRate;
            dribble = firstDribbleRate;
            shoot = firstShootRate;
            DefenceState();
            StartCoroutine(PanelTime(DefenceTimePanel));
            MoveTextUpdate();
        }
        else
        {
            AttackPanelActive();
            yield return new WaitForSeconds(1f);
            AttackRate(-2f, -2f, -2f);
            DefenceState();

            if (LocalizationManager.Language == "Türkçe")
            {
                AnnouncerText(announcerShootUnsuccessfulTR[announcerUnText]);

            }
            else if (LocalizationManager.Language == "English")
            {
                AnnouncerText(announcerShootUnsuccessfulEN[announcerUnTextEN]);
            }
            StartCoroutine(PanelTime(DefenceTimePanel));
            MoveTextUpdate();


        }
        RateTextUpdate();
    }
    public void ShootButtonn()
    {
        StartCoroutine(ShootButton());
    }

    void AttackRate(float passRate, float dribbleRate, float shootRate)
    {
        pass += passRate;
        dribble += dribbleRate;
        shoot += shootRate;
    }
    void DefenceRate(float pressRate, float grabRate)
    {
        press += pressRate;
        grab+= grabRate;
    }

    void RateTextUpdate()
    {
        passRateText.text = pass.ToString("F0");
        dribbleRateText.text = dribble.ToString("F0");
        shootRateText.text = shoot.ToString("F0");
        pressRateText.text = press.ToString("F0");
        grabRateText.text = grab.ToString("F0");
    }
    void MoveTextUpdate()
    {
        attackMoveText.text = attackMove.ToString();
        defenceMoveText.text = defenceMove.ToString();
        moveAttackSlider.value = attackMove;
        moveDefenceSlider.value = defenceMove;
    }

    void AnnouncerText(string text)
    {
        AnnouncerPrefabs announcer = Instantiate(announcerPrefabs, announcerParents);
        announcer.transform.Rotate(0, 0, 180);
        //Destroy(announcer.gameObject, 5f);
        announcer.Config(text);
    }
    public void infoPanelsOpen(int number)
    {
        if (!infoPanels[number].activeSelf)
        {
            infoPanels[number].SetActive(true);
        }
        else
            infoPanels[number].SetActive(false);        
    }
    public void AttackPanelActive()
    {
        if (attackButtonPanel.activeSelf)
            attackButtonPanel.SetActive(false);
        else
            attackButtonPanel.SetActive(true);
    }

    public void DefencePanelActive()
    {
        if (defenceButtonPanel.activeSelf)
            defenceButtonPanel.SetActive(false);
        else
            defenceButtonPanel.SetActive(true);
    }
    public IEnumerator PanelTime(GameObject panel)
    {
        panel.SetActive(true);
        yield return new WaitForSeconds(2f);
        panel.SetActive(false);
    }

    public void LeaveTheMatch()
    {       
            if (eventMatch.isEventMatch)
            {
            myScore = 0;
            opponentScore = 0;
            myScoreText.text = myScore.ToString();
            opponentScoreText.text = opponentScore.ToString();


            for (int i = 0; i < announcerParents.childCount; i++)
            {
                Destroy(announcerParents.GetChild(i).gameObject);
            }
            eventMatch.EventMatchOver(0, 3);
                return;
            }
            MatchManager.instance.matchScene.SetActive(false);
            MatchManager.instance.MatchEnd(0, 3);
            myScore = 0;
            opponentScore = 0;
            myScoreText.text = myScore.ToString();
            opponentScoreText.text = opponentScore.ToString();


            for (int i = 0; i < announcerParents.childCount; i++)
            {
                Destroy(announcerParents.GetChild(i).gameObject);
            }
        
    }
}
