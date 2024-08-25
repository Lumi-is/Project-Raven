using System.Collections;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("Sound")]
    public AudioSource musicSource;
    [SerializeField] AudioSource SFX;
    [SerializeField] Spawner spawnerScript;
    
    [Header("Public")]
    [Tooltip("Whether or not player has done any action on the current beat.")]
    public int cooldownUntilNextAction;

    [Header("Music attributes")]
    public float BPM;

    [Tooltip("Offset for how much time the player is given on each beat for still hitting them correctly.")]
    public float MaxOffset;

    [Header("Read-only")]

    [Tooltip("Boolean read by other scripts for checking whether the music is currently on beat and an action can be done.")]
    public bool OnBeat;

    //Private
    private float lastBeat;
    private float timeBetweenBeats;

    private float lastFrameMusicTime;
    private float currentFrameMusicTime;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
            spawnerScript.beatCount = 0;
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
        spawnerScript.SpawnEnemies();
        OnBeat = false;

        yield return new WaitForSeconds(60/LocalBPM-offsetTime);
        OnBeat = true;
    }
}
