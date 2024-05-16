using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTracker : MonoBehaviour
{
    private QuestManager questManager;
    private void Awake()
    {
        questManager = GetComponent<QuestManager>();

        InputManager.onPitchClickedQuest += ClickedCallback;
        MatchManager.onPlayMatch+=PlayMatchCallback;
        ShopManager.onPlayerPurchased += BuyPlayersCallback;
        DataManager.onUpgradeClubHouse += ClubHouseUpgradeCallback;
    }
    
    private void OnDestroy()
    {
        InputManager.onPitchClickedQuest -= ClickedCallback;
        MatchManager.onPlayMatch -= PlayMatchCallback;
        ShopManager.onPlayerPurchased -= BuyPlayersCallback;
        DataManager.onUpgradeClubHouse -= ClubHouseUpgradeCallback;
    }

    private void ClickedCallback()
    {
        Dictionary<int, Quest> quests = new Dictionary<int, Quest>(questManager.GetCurrentQuest()); //aktif görevi alýyoruz

        foreach (KeyValuePair<int, Quest> questData in quests)
        {
            Quest quest = questData.Value;

            if (quest.Type == QuestType.Click)
            {
                int currentEnemiesKilled = (int)(quest.progress * quest.target);
                currentEnemiesKilled++;
                float newProgress = (float)currentEnemiesKilled / quest.target;

                questManager.UpdateQuestProgress(questData.Key, newProgress);
            }
        }
    }

    

    private void BuyPlayersCallback()
    {
        Dictionary<int, Quest> quests = new Dictionary<int, Quest>(questManager.GetCurrentQuest());

        foreach (KeyValuePair<int, Quest> questData in quests)
        {
            Quest quest = questData.Value;

            if (quest.Type == QuestType.BuyPlayers)
            {
                int currentTowerLevel = (int)(quest.progress * quest.target);
                currentTowerLevel++;

                float newProgress = (float)currentTowerLevel / quest.target;

                questManager.UpdateQuestProgress(questData.Key, newProgress);
            }
        }
    }


    private void PlayMatchCallback()
    {
        Dictionary<int, Quest> quests = new Dictionary<int, Quest>(questManager.GetCurrentQuest());

        foreach (KeyValuePair<int, Quest> questData in quests)
        {
            Quest quest = questData.Value;

            if (quest.Type == QuestType.PlayMatch)
            {
                int currentArenaLevel = (int)(quest.progress * quest.target);
                currentArenaLevel++;
                float newProgress = (float)currentArenaLevel / quest.target;

                questManager.UpdateQuestProgress(questData.Key, newProgress);
            }
        }
    }
    private void ClubHouseUpgradeCallback()
    {
        Dictionary<int, Quest> quests = new Dictionary<int, Quest>(questManager.GetCurrentQuest());

        foreach (KeyValuePair<int, Quest> questData in quests)
        {
            Quest quest = questData.Value;

            if (quest.Type == QuestType.ClubHoseUpgrade)
            {
                int currentCard = (int)(quest.progress * quest.target);
                currentCard++;

                float newProgress = (float)currentCard / quest.target;

                questManager.UpdateQuestProgress(questData.Key, newProgress);
            }
        }
    }
    private void AdsWatchCallback()
    {
        Dictionary<int, Quest> quests = new Dictionary<int, Quest>(questManager.GetCurrentQuest());

        foreach (KeyValuePair<int, Quest> questData in quests)
        {
            Quest quest = questData.Value;

            if (quest.Type == QuestType.AdsWatch)
            {
                int currentAdsWatch = (int)(quest.progress * quest.target);
                currentAdsWatch++;

                float newProgress = (float)currentAdsWatch / quest.target;

                questManager.UpdateQuestProgress(questData.Key, newProgress);
            }
        }
    }
}
