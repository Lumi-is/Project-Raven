using System.Collections;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    [Header("Public")]
    public bool hasDoneActionOnCurrentBeat;

    [Header("Music attributes")]
    public float BPM;
    public float MaxClickTimerOffset;

    [Header("Read-only")]
    public bool OnBeat;

    float lastBeat;
    float timeBetweenBeats;

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
        
        Debug.Log(musicSource.time);

        if(currentFrameMusicTime > lastBeat + timeBetweenBeats && currentFrameMusicTime > lastFrameMusicTime)
        {
            lastBeat += timeBetweenBeats;
            hasDoneActionOnCurrentBeat = false;
            StartCoroutine(CheckForBeat(BPM));
        }

        if(currentFrameMusicTime < lastFrameMusicTime)
        {
            lastBeat = 0;
            hasDoneActionOnCurrentBeat = false;
            StartCoroutine(CheckForBeat(BPM));
        }
    }

    private IEnumerator CheckForBeat(float LocalBPM)
    {       
        yield return new WaitForSeconds(MaxClickTimerOffset/2);
        OnBeat = false;

        yield return new WaitForSeconds(60/LocalBPM-MaxClickTimerOffset);
        OnBeat = true;

        //yield return new WaitForSeconds(MaxClickTimerOffset/2);
    }
}
