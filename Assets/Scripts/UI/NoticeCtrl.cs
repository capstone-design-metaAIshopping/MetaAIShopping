using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeCtrl : MonoBehaviour
{
    public static NoticeCtrl _Instance;
    public GameObject noticePanel;
    
    private void Awake()
    {
        _Instance = this;
    }
    public void CreateNotice(string text, bool isOK)
    {
        noticePanel.SetActive(true);
        noticePanel.transform.Find("Text").GetComponent<Text>().text = text;
        noticePanel.transform.Find("ButtonOK").gameObject.SetActive(isOK);
    }

    public void ClickNoticeOKBtn()
    {
        noticePanel.SetActive(false);
    }
}
