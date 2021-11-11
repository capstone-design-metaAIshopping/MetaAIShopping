using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpace : MonoBehaviour
{
    public Dictionary<string, string[]> spaceDict = new Dictionary<string, string[]>();
    public static ChangeSpace _Instance;
    // Start is called before the first frame update
    void Awake()
    {
        _Instance = this;
      
        spaceDict.Add("집", new string[] { "소파","책상","의자" });
       // spaceDict.Add("롤러장", new string[] { "인라인스케이트","롤러스케이트" });
        spaceDict.Add("도로", new string[] { "킥보드","자전거" });
        spaceDict.Add("농구장", new string[] { "농구공","축구공","농구골대" });
        spaceDict.Add("겨울", new string[] { "티셔츠" });

    }

}
