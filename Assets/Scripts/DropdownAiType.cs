using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownAiType : MonoBehaviour {

    public Dropdown myDropdown;
    public List<PlayerController> players;

    // Use this for initialization
    void Start () {
        myDropdown.onValueChanged.AddListener(delegate {
            myDropdownValueChangedHandler(myDropdown);
        });
    }
    void Destroy()
    {
        myDropdown.onValueChanged.RemoveAllListeners();
    }
    public void myDropdownValueChangedHandler(Dropdown target)
    {
       if (target.value == 0)
        {
            foreach (var item in players)
            {
                item.aiType = PlayerController.AIType.V2;
            }
        }
        if (target.value == 1)
        {
            foreach (var item in players)
            {
                item.aiType = PlayerController.AIType.V1;
            }
        }
        if (target.value == 2)
        {
            foreach (var item in players)
            {
                item.aiType = PlayerController.AIType.V0;
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
