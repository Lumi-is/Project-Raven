using System.Collections;
using System.IO;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFX;
    
    [Header("Attributes")]
    [Tooltip("Whether or not player has done any action on the current beat.")]
    public int cooldownUntilNextAction;

    [Header("Music Attributes")]
    [Tooltip("Offset for how much time the player is given on each beat for still hitting them correctly.")]
    public float MaxOffset;

    [Header("Data")]
    [Tooltip("Boolean read by other scripts for checking whether the music is currently on beat and an action can be done.")]
    public bool OnBeat;

    [Header("Spawner")]
    [SerializeField] SpawnManager spawner;

    //Private
    private float BPM;
    private float lastBeat;
    private float timeBetweenBeats;
    private float lastFrameMusicTime;
    private float currentFrameMusicTime;
    private float beatTimerOffsetLimit;
    private bool LevelOn;

    public void StartLevel()
    {
        BPM = spawner.currentPhaseData.BPM;
        LevelOn = true;
        lastBeat = 0;
        timeBetweenBeats = 60f/BPM;

        musicSource.Play();

        StartCoroutine(BeatEnumerator(BPM, beatTimerOffsetLimit));
    }

    

    void FixedUpdate()
    {
        if(LevelOn)
        {
            // Sets the lastFrameMusicTime to be that of currentFrameMusicTime
            // This works because currentFrameMusicTime is in this case set after the lastFrameMusicTime
            // This means the lastFrameMusicTime always gets updated one frame late
            lastFrameMusicTime = currentFrameMusicTime;
            currentFrameMusicTime = musicSource.time;

            // Checks if enough time has gone from last beat for the beat change to occur
            // Additionally, makes sure the music has not looped
            if(currentFrameMusicTime >= lastBeat + timeBetweenBeats && currentFrameMusicTime > lastFrameMusicTime)
            {
                beatTimerOffsetLimit = MaxOffset/2;
                lastBeat += timeBetweenBeats;

                if(cooldownUntilNextAction != 0)
                {
                    SFX.Play();
                    cooldownUntilNextAction--;
                }
                
                StartCoroutine(BeatEnumerator(BPM, beatTimerOffsetLimit));
            }

            if(currentFrameMusicTime < lastFrameMusicTime)
            {
                spawner.OnBeatReset();
                beatTimerOffsetLimit = MaxOffset/2;
                lastBeat = 0;
                if(cooldownUntilNextAction != 0)
                {
                    SFX.Play();
                    cooldownUntilNextAction--;
                }
                StartCoroutine(BeatEnumerator(BPM, beatTimerOffsetLimit));
            }
        }
    }

    private IEnumerator BeatEnumerator(float LocalBPM, float offsetTime)
    {
        spawner.OnBeatChange();
        OnBeat = false;

        yield return new WaitForSeconds(60/LocalBPM-offsetTime);
        OnBeat = true;
    }
}

