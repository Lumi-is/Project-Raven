using System.Collections;
using UnityEditor.EditorTools;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFX;
    
    [Header("Public")]
    [Tooltip("Whether or not player has done any action on the current beat.")]
    public int cooldownUntilNextAction;

    [Header("Music attributes")]
    public float BPM;

    [Tooltip("Offset for how much time the player is given on each beat for still hitting them correctly.")]
    public float MaxClickTimerOffset;

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

        StartCoroutine(CheckForBeat(BPM));
    }

    void Update()
    {
        lastFrameMusicTime = currentFrameMusicTime;
        currentFrameMusicTime = musicSource.time;

        //DEBUGGING
        //Debug.Log(musicSource.time);

        if(currentFrameMusicTime > lastBeat + timeBetweenBeats && currentFrameMusicTime > lastFrameMusicTime)
        {
            lastBeat += timeBetweenBeats;

            if(cooldownUntilNextAction != 0)
            {
                SFX.Play();
                cooldownUntilNextAction = cooldownUntilNextAction - 1;
            }
            
            StartCoroutine(CheckForBeat(BPM));
        }

        if(currentFrameMusicTime < lastFrameMusicTime)
        {
            lastBeat = 0;
            if(cooldownUntilNextAction != 0)
            {
                SFX.Play();
                cooldownUntilNextAction = cooldownUntilNextAction - 1;
            }
            StartCoroutine(CheckForBeat(BPM));
        }
    }

    private IEnumerator CheckForBeat(float LocalBPM)
    {       
        OnBeat = false;

        yield return new WaitForSeconds(60/LocalBPM-MaxClickTimerOffset);
        OnBeat = true;
    }
}
