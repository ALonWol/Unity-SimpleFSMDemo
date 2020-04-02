using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopText : MonoBehaviour
{
    public Transform textTransform;

    public TMP_Text text;

    public Camera uiCamera;

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    void Update()
    {
        var point = uiCamera.WorldToViewportPoint(transform.position);
        textTransform.position = new Vector3(Screen.width / 2 * point.x + Screen.width * uiCamera.rect.x, Screen.height * point.y, 0);
    }

    public void SetText(string text) {
        this.text.text = text;
    }
}
