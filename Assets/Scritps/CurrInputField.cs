using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrInputField : MonoBehaviour
{
    static public InputField inputField;
    // Start is called before the first frame update
    void Start()
    {
        inputField = gameObject.GetComponent<InputField>();
    }

}
