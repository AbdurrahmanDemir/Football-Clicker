using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MatchState
{
    attack,
    defence,
}
public class MatchEngine : MonoBehaviour
{
    MatchState matchState;

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
    [SerializeField] private AnnouncerPrefabs announcerPrefabs;
    [SerializeField] private Transform announcerParents;
    [SerializeField] private GameObject announcerPanel;
    [SerializeField] private GameObject attackButtonPanel;
    [SerializeField] private GameObject defenceButtonPanel;
    [SerializeField] private GameObject animPanel;
    [SerializeField] private TextMeshProUGUI announcerText;
    [SerializeField] private string[] announcerPressSuccessful;
    [SerializeField] private string[] announcerPressUnsuccessful;
    [SerializeField] private string[] announcerGrabSuccessful;
    [SerializeField] private string[] announcerGrabUnsuccessful;
    [SerializeField] private string[] announcerPassSuccessful;
    [SerializeField] private string[] announcerPassUnsuccessful;
    [SerializeField] private string[] announcerDribbleSuccessful;
    [SerializeField] private string[] announcerDribbleUnsuccessful;
    [SerializeField] private string[] announcerShootSuccessful;
    [SerializeField] private string[] announcerShootUnsuccessful;
    [SerializeField] private string[] announcerMove;
    [SerializeField] private Animator animator;


   


    private void Start()
    {
        AttackState();
        moveAttackSlider.maxValue = 3;
        moveDefenceSlider.maxValue = 3;
        myScoreText.text = myScore.ToString();
        opponentScoreText.text = opponentScore.ToString();
    }

    private void Update()
    {
        if (attackMove <= 0 && matchState != MatchState.defence)
        {
            DefenceState();
            AnnouncerText(announcerMove[0]);

        }
        else if (defenceMove <= 0 && matchState != MatchState.attack)
        {
            AttackState();
            AnnouncerText(announcerMove[1]);
        }

        if (myScore >= 3 || opponentScore >= 3)
        {
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
        pass = (myTeamTotalGen / (myTeamTotalGen + opponentTotalGen))*100f;
        dribble = ((myTeamMidGen + myTeamForGen) / (myTeamMidGen + myTeamForGen + opponentMidGen + opponentDefGen))*100f;
        shoot = (myTeamForGen / (myTeamForGen + opponentDefGen)) * 100f;
        press = (myTeamTotalGen / (myTeamTotalGen + opponentTotalGen))*100f ;
        grab = (myTeamForGen / (myTeamForGen + opponentDefGen)) * 100f;

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
        int announcerText= UnityEngine.Random.Range(0, announcerPressSuccessful.Length);
        int announcerUnText= UnityEngine.Random.Range(0, announcerPressUnsuccessful.Length);
        Debug.Log(RandomRate);
        if (RandomRate < press)
        {
            DefencePanelActive();
            yield return new WaitForSeconds(1f);
            DefencePanelActive();
            DefenceRate(5f, 2f);
            defenceMove--;          
            MoveTextUpdate();
            AnnouncerText(announcerPressSuccessful[announcerText]);
        }
        else
        {
            DefencePanelActive();
            yield return new WaitForSeconds(1f);
            DefenceRate(-5f, -2f);
            defenceMove--;

            if (defenceMove < 1)
            {   opponentScore += 1;
                opponentScoreText.text = opponentScore.ToString();
                press = firstPressRate;
                grab = firstGrabRate;
                AttackState();
                AnnouncerText(announcerGrabUnsuccessful[announcerUnText]);
            }
            else
            {
                MoveTextUpdate();
                AnnouncerText(announcerPressUnsuccessful[announcerUnText]);
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
        int announcerText = UnityEngine.Random.Range(0, announcerGrabSuccessful.Length);
        int announcerUnText = UnityEngine.Random.Range(0, announcerGrabUnsuccessful.Length);
        Debug.Log(RandomRate);

        if (RandomRate < grab)
        {
            DefencePanelActive();
            yield return new WaitForSeconds(1f);
            AttackState();
            MoveTextUpdate();
            AnnouncerText(announcerGrabSuccessful[announcerText]);
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
            AnnouncerText(announcerGrabUnsuccessful[announcerUnText]);
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
        int announcerText = UnityEngine.Random.Range(0, announcerPassSuccessful.Length);
        int announcerUnText = UnityEngine.Random.Range(0, announcerPassUnsuccessful.Length);
        Debug.Log(RandomRate);

        if (RandomRate < pass)
        {
            AttackPanelActive();
            yield return new WaitForSeconds(1f);
            AttackPanelActive();
            AttackRate(10f, 5f, 3f);
            attackMove--;
            AnnouncerText(announcerPassSuccessful[announcerText]);
            MoveTextUpdate();

        }
        else
        {
            AttackPanelActive();
            yield return new WaitForSeconds(1f);
            AttackRate(-5f, -5f, -3f);
            DefenceState();
            AnnouncerText(announcerPassUnsuccessful[announcerUnText]);
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
        int announcerText = UnityEngine.Random.Range(0, announcerDribbleSuccessful.Length);
        int announcerUnText = UnityEngine.Random.Range(0, announcerDribbleUnsuccessful.Length);
        Debug.Log(RandomRate);

        if (RandomRate < dribble)
        {
            AttackPanelActive();
            yield return new WaitForSeconds(1f);
            AttackPanelActive();
            AttackRate(0f, 5f, 5f);
            attackMove--;
            AnnouncerText(announcerDribbleSuccessful[announcerText]);
            MoveTextUpdate();
        }
        else
        {
            AttackPanelActive();
            yield return new WaitForSeconds(1f);
            AttackRate(-8f, -5f, -5f);
            DefenceState();
            AnnouncerText(announcerDribbleUnsuccessful[announcerUnText]);
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
        int announcerText = UnityEngine.Random.Range(0, announcerShootSuccessful.Length);
        int announcerUnText = UnityEngine.Random.Range(0, announcerShootUnsuccessful.Length);
        Debug.Log(RandomRate);

        if (RandomRate < shoot)
        {
            AttackPanelActive();
            yield return new WaitForSeconds(1f);
            AttackPanelActive();
            myScore += 1;
            myScoreText.text = myScore.ToString();
            AnnouncerText(announcerShootSuccessful[announcerText]);
            pass = firstPassRate;
            dribble = firstDribbleRate;
            shoot = firstShootRate;
            DefenceState();
            MoveTextUpdate();
        }
        else
        {
            AttackPanelActive();
            yield return new WaitForSeconds(1f);
            AttackRate(-10f, -8f, -5f);
            DefenceState();
            AnnouncerText(announcerShootUnsuccessful[announcerUnText]);
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
    //public void AnnouncerPanelActive()
    //{
    //    if(announcerPanel.activeSelf)
    //        announcerPanel.SetActive(false);
    //    else
    //        announcerPanel.SetActive(true);
    //}

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


}
