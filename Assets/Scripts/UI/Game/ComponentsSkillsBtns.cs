using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;

public class ComponentsSkillsBtns : MonoBehaviour
{
    public List<BtnSkills> listBtnSkills;
    public CowntdownFilled cdFilled;
    public void SetInfo(SerializedDictionary<int, Sprite> timeAndSprite, Color[] colors, Animator anim)
    {
        int index = 0;
        foreach (var (time, sprite) in timeAndSprite)
        {
            listBtnSkills[index].SetColor(colors[0], colors[1]);
            index++;
        }
    }


    public List<BtnSkills> GetListBtnSkills()
    {
        return listBtnSkills;
    }
}
