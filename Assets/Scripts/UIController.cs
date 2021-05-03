using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;


public class UIController : MonoBehaviour
{
    PlayerControl player;
    Text distanceText;

    GameObject results;
    TextMeshProUGUI finalDes;
    

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        distanceText = GameObject.Find("DistanceText").GetComponent<Text>();

        finalDes = GameObject.Find("FinalDes").GetComponent<TextMeshProUGUI>();
        results = GameObject.Find("Results");
        results.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int distance = Mathf.FloorToInt(player.distance);
        distanceText.text = distance + " m";

        if(player.isDead)
        {
            results.SetActive(true);
            finalDes.text = distance + " m";
        }
    }

    public void Quit()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); 
    }

    public void Retry()
    {
        SceneManager.LoadScene("The Game");
    }
}
