using System.Collections.Generic;
using UnityEngine;
using System.IO.DataHandling;

public class LevelEditor : MonoBehaviour
{
    [Header("Boss information")]
    [SerializeField] string bossName;
    [SerializeField] List<string> phaseNameList;

    [SerializeField] List<int> phaseVersionAmountList;

    [Header("AreaLevel information")]
    [SerializeField] string areaLevelName;
    [SerializeField] List<string> areaNameList;

    [SerializeField] List<string> roomNameList;

    [Header("Song information")]

    [SerializeField] List<AudioClip> phaseSongList;
    [SerializeField] float defaultBPM;



    //DEBUGGING ONLY: REMOVE IF CODE IS COMPLETE
    public void CreateDebugLevels()
    {
        BossDataHandler.DeleteBossLevel(bossName);
        BossDataHandler.CreateBossLevel(phaseSongList, phaseNameList, phaseVersionAmountList, bossName, defaultBPM);

        AreaLevelHandler.DeleteAreaLevel(areaLevelName);
        AreaLevelHandler.CreateAreaLevel(phaseSongList, areaNameList, areaLevelName, defaultBPM, roomNameList);
    }
}
