using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timerUILabel; // for inspector to drag textmeshpro objects
    private TextMeshProUGUI timerOffsetText; 

    private float t_offset = 0f; //Set to nothing, can be set to an offset if needed.
    private int t_minutes;
    private int t_seconds;
    private int t_milliseconds;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // this just handles 
        {
            Timer();
        }
        if (Input.GetMouseButtonDown(1))
        {
            OnClickCounter();
        }
    }

    private void Timer()
    {
        float t = Time.time - t_offset;

        t_minutes = ((int)t / 60); // t(secs) / 60 = total minutes
        t_seconds = ((int)t % 60); // t(secs) % 60 = remaining seconds
        t_milliseconds = ((int)(t * 1000)) % 1000; // (total seconds * 100) % 100 = remaining milliseconds

        timerUILabel.text = string.Format("{0:00}:{1:00}:{2:00}", t_minutes, t_seconds, t_milliseconds);
    }

    IEnumerator OnClickCounter()
    {
        WaitForSeconds wfs = new WaitForSeconds(1);

        while(true)
        {
            timerOffsetText.text = t_offset.ToString();
            yield return wfs;
            t_offset++;
        }
    }
}
