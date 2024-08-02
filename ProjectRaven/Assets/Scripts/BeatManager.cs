using System.Collections;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;

    [Header("Music attributes")]
    public float BPM;
    public float MaxClickTimerOffset;
    public float startDelay;

    [Header("Read-only")]
    public bool OnBeat;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicSource.Play();


        StartCoroutine(CheckForBeat(BPM,startDelay));
    }

    private IEnumerator CheckForBeat(float LocalBPM, float startDelay)
    {
        if(startDelay != 0)
        {
            yield return new WaitForSeconds(startDelay);
        }
        yield return new WaitForSeconds(60/LocalBPM-MaxClickTimerOffset);
        OnBeat = true;

        yield return new WaitForSeconds(MaxClickTimerOffset);
        OnBeat = false;

        StartCoroutine(CheckForBeat(BPM,0));
    }
}
