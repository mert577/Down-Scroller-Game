using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteManager : MonoBehaviour
{
    // Start is called before the first frame update

    public  Color[] palette1;
    public Color[] palette2;

    public static Color[] currentPalette;

  
    void Start()
    {
        if (!PlayerPrefs.HasKey("PaletteIndex"))
        {
            PlayerPrefs.SetInt("PaletteIndex", 0);
        }
  
    }

    // Update is called once per frame
    void Update()
    {

        int currentIndex;
        currentIndex = PlayerPrefs.GetInt("PaletteIndex");
        if (currentIndex == 0) currentPalette = palette1;
        else if (currentIndex == 1) currentPalette = palette2;
    }
}
