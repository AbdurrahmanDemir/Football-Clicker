using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Quest[] quests;
    private Dictionary<int, Quest> uncompletedQuestDictionnary = new Dictionary<int, Quest>();

    [Header("Elements")]
    [SerializeField] private QuestContainer QuestContainerPrefab;
    [SerializeField] private Transform questContainerParent;



    private void Awake()
    {
        QuestContainer.onRewardClaimed += QuestRewardClaimedCallback;
    }

    private void OnDestroy()
    {
        QuestContainer.onRewardClaimed -= QuestRewardClaimedCallback;
    }

    private void Start()
    {
        CreatQuestContainers();
    }
    private void QuestRewardClaimedCallback(int questIndex)
    {


        // görevi kaydediyoruz
        SetQuestComplete(questIndex);

        // oyuncuya ödülünü veriyoruz
        int reward = quests[questIndex].reward;
        DataManager.instance.AddGem(reward);

        UpdateQuest();


    }

    private void UpdateQuest()
    {

        // Önceki görevleri yok ediyoruz
        foreach (Transform child in questContainerParent)
        {
            Destroy(child.gameObject);
        }

        CreatQuestContainers(); //yeni görev oluþturuyoruz
    }
    public void CreatQuestContainers() //Görev barlarýný oluþturuyoruz
    {
        StoreUncompletedMissions();

        foreach (KeyValuePair<int, Quest> questData in uncompletedQuestDictionnary)
        {
            CreatQuestContainer(questData);
        }
    }

    private void StoreUncompletedMissions() //Gösterilecek görevleri buluyoruz
    {
        uncompletedQuestDictionnary.Clear(); //Tamamlanmamýþ görevler listesini temizle


        //Görevler arasýnda gez, tamamlanmamýþ olanlardan 3 tanesini listeye ekle
        for (int i = 0; i < quests.Length; i++)
        {
            if (IsQuestComplete(i))
                continue;

            Quest quest = quests[i];
            quest.progress = GetQuestProgress(new KeyValuePair<int, Quest>(i, quest));

            uncompletedQuestDictionnary.Add(i, quest);

            if (uncompletedQuestDictionnary.Count >= 5)
                break;
        }
    }

    public void CreatQuestContainer(KeyValuePair<int, Quest> questData) //Görev barýný oluþturuyoruz ve UI'da görev barlarýný dolduruyoruz
    {
        QuestContainer QuestContainerInstance = Instantiate(QuestContainerPrefab, questContainerParent);


        string title = GetQuestTitle(questData.Value);
        string rewardString = questData.Value.reward.ToString();
        float progress = GetQuestProgress(questData);

        QuestContainerInstance.Configure(title, rewardString, progress, questData.Key);


        Debug.Log("KEY" + QuestContainerInstance.GetKey());
    }


    private string GetQuestTitle(Quest quest) //Aktif görevin ismini alýyoruz
    {
        switch (quest.Type)
        {
            case QuestType.Click:
                return "Click " + quest.target.ToString() + " Times";

            case QuestType.BuyPlayers:
                return "Buy " + quest.target.ToString() + " Players";

            case QuestType.AdsWatch:
                return "Watch " + quest.target.ToString() + " ads";

            case QuestType.PlayMatch:
                return "Play " + quest.target.ToString() + " Matches";
            case QuestType.ClubHoseUpgrade:
                return "Upgrade " + quest.target.ToString() + " Club House";
            case QuestType.PlayerUpgrade:
                return "Upgrade " + quest.target.ToString() + " Players";


            default:
                return "Blank";
        }
    }

    public void UpdateQuestProgress(int questIndex, float newProgress)
    {
        Debug.Log("New Progress : " + newProgress);

        // görev ilerlemesini kaydet
        SaveQuestProgress(questIndex, newProgress);

        Quest quest = quests[questIndex];
        quest.progress = newProgress;
        quests[questIndex] = quest;

        uncompletedQuestDictionnary[questIndex] = quest;

        if (questContainerParent != null)
        {
            for (int i = 0; i < questContainerParent.childCount; i++)
            {
                QuestContainer questContainer = questContainerParent.GetChild(i).GetComponent<QuestContainer>();

                if (questContainer.GetKey() != questIndex)
                    continue;


                questContainer.UpdateProgress(newProgress);
            }
        }

    }

    public Dictionary<int, Quest> GetCurrentQuest() //Aktif görevi diðer scriptlerde kullanmak için alýyoruz
    {
        return uncompletedQuestDictionnary;
    }
    private float GetQuestProgress(KeyValuePair<int, Quest> questData) //Görev ilerlemesini alýyoruz
    {
        return PlayerPrefs.GetFloat("QuestProgress" + questData.Key);
    }

    private void SaveQuestProgress(int key, float progress) //Görev ilerlemesini kaydediyoruz
    {
        PlayerPrefs.SetFloat("QuestProgress" + key, progress);

    }

    private void SetQuestComplete(int questIndex) //Görev tamamlanýnca kaydediyoruz
    {
        PlayerPrefs.SetInt("Quest" + questIndex, 1);
    }

    private bool IsQuestComplete(int questIndex) //Görev tamamlandý mý sorguluyoruz
    {
        return PlayerPrefs.GetInt("Quest" + questIndex) == 1;

    }
}
public enum QuestType { Click, BuyPlayers, PlayMatch, AdsWatch, ClubHoseUpgrade, PlayerUpgrade } //Görev türlerini belirliyoruz

[System.Serializable]
public struct Quest
{
    public QuestType Type;
    public int target;
    public int reward;
    public float progress;
}
