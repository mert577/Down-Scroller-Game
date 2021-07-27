using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteMember : MonoBehaviour
{
    // Start is called before the first frame update

    int colorIndex;
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetColor()
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().color = PaletteManager.currentPalette[colorIndex];
        }
        else if (GetComponent<Image>() != null)
        {
            GetComponent<Image>().color = PaletteManager.currentPalette[colorIndex];

        }
    }
}
