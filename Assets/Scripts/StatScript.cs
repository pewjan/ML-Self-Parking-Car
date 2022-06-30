using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StatScript : MonoBehaviour
{
    public Text statText;


    public void setText(float point, int episode, float totalPoint)
    {
        statText.text = "Current point: " + point + "\n" + "Completed Episode: " + episode + "\n" + "Total Points: " + totalPoint;
    }


}
