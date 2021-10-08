using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DegradableItem : MonoBehaviour
{


    public static DegradableItem Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ChangeTree")
        {
            if (GameController.Instance.ProfColor.saturation.value < -25 && !GameController.Instance.IsGrayScale)
            {
                SwitchTree(true);
            }
            else if (GameController.Instance.ProfColor.saturation.value == 0 && GameController.Instance.IsGrayScale)
            {
                SwitchTree(false);
                Destroy(other.gameObject);
            }
        }
    }

    public void SwitchTree(bool isDegradable)
    {
        if(isDegradable)
        {
            this.transform.GetChild(0).gameObject.SetActive(false);

            this.transform.GetChild(1).gameObject.SetActive(true);

        }
        else
        {
            this.transform.GetChild(0).gameObject.SetActive(true);

            this.transform.GetChild(1).gameObject.SetActive(false);

        }

    }
}
