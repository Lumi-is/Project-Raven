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
    public string levelName;

    //Private

    readonly private float BPM;
    private float lastBeat;
    private float timeBetweenBeats;
    private float lastFrameMusicTime;
    private float currentFrameMusicTime;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        lastBeat = 0;
        timeBetweenBeats = 60/BPM;

        musicSource.Play();

        StartCoroutine(BeatEnumerator(BPM, beatTimerOffsetLimit));
    }

    private float beatTimerOffsetLimit;

    void Update()
    {
        lastFrameMusicTime = currentFrameMusicTime;
        currentFrameMusicTime = musicSource.time;

        //DEBUGGING
        //Debug.Log(musicSource.time);

        if(currentFrameMusicTime > lastBeat + timeBetweenBeats && currentFrameMusicTime > lastFrameMusicTime)
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
            //spawner.currentBeat = 0; // WIP
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

    private IEnumerator BeatEnumerator(float LocalBPM, float offsetTime)
    {
        //spawner.SpawnEnemies(); // WIP
        OnBeat = false;

        yield return new WaitForSeconds(60/LocalBPM-offsetTime);
        OnBeat = true;
    }
}
