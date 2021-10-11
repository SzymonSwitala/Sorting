using UnityEngine;
using TMPro;
public class BoardController : MonoBehaviour
{
    public bool startTimer;
    private int comparisons;
    private float timeElapsed;
    [SerializeField] private TextMeshPro timerText;
    [SerializeField] private TextMeshPro ComparisonsText;
    [SerializeField] private TextMeshPro typeText;

    public void ShowTexts()
    {
        timerText.enabled = true;
        ComparisonsText.enabled = true;
        typeText.enabled = true;
    }
    public void SetTexts(string type)
    {

        typeText.text = type;
    }
    public void AddComparisons(int value)
    {
        comparisons += value;
        ComparisonsText.text = "Comparisons: " + comparisons;
    }
    private void FixedUpdate()
    {
        if (startTimer)
        {
            timeElapsed += Time.deltaTime;

            timerText.text = "Time: " + (int)timeElapsed;


        }


    }
}
