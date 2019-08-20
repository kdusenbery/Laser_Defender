using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    TextMeshProUGUI healthTxt;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        healthTxt = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        healthTxt.text = player.GetHealth().ToString();
    }
}
