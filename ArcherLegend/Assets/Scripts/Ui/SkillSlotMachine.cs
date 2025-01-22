using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSlotMachine : MonoBehaviour
{
    public GameObject[] SlotSkillObject;
    public Button[] Slot;

    public Sprite[] SkillSprite;

    [System.Serializable]
    public class DisplayItemSlot
    {
        public List<Image> SlotSprite = new List<Image>();
    }

    public DisplayItemSlot[] DisplayItemSlots;

    public Image DisplayResultImage;
    public List <int> StartList = new List<int>();

    public List <int> ResultIndexList = new List<int>();
    int ItemCnt = 3;

    void Start()
    {
        for (int i = 0; i < ItemCnt * Slot.Length; i++)
        {
            StartList.Add(i);

        }

        for ( int i = 0 ; i < Slot.Length ; i++)
        {
            for (int j = 0; j < ItemCnt; j++)
            {
                Slot[i].interactable = false;

                int randomIndex = Random.Range( 0, StartList.Count);
                if ( i == 0 && j == 1 || i ==  1 && j == 0 || i == 2 && j == 2 )
                {
                    ResultIndexList.Add (StartList[randomIndex]);

                }
                DisplayItemSlots[i].SlotSprite[j].sprite = SkillSprite[StartList[randomIndex]];

                if (j == 0)
                {
                    DisplayItemSlots[i].SlotSprite[j].sprite = SkillSprite[StartList[randomIndex]];
                }
                StartList.RemoveAt (randomIndex);
            }
        }
        // StartCoroutine( StartSlot1 ());
        // StartCoroutine( StartSlot2 ());
        // StartCoroutine( StartSlot3 ());

        for (int i = 0; i < Slot.Length; i++)
        {
            StartCoroutine( StartSlot (i));
        }
    }
    int [] answer = { 2,3,1};

    IEnumerator StartSlot (int SlotIndex)
    {
        for (int i = 0; i <  (ItemCnt * (6 + SlotIndex * 4) + answer[SlotIndex]) *2; i++)
        {
            SlotSkillObject[SlotIndex].transform.localPosition -= new Vector3(0, 50f, 0);
                        if (SlotSkillObject[SlotIndex].transform.localPosition.y < 50f)
                        {
                            SlotSkillObject[SlotIndex].transform.localPosition += new Vector3 (0, 300f, 0); 
                        }
                        yield return null;
        }
        for (int i = 0; i < ItemCnt ; i++)
        {
            Slot[i].interactable = true;
        }
    }
    IEnumerator StartSlot1()
    {
        for ( int i = 0; i < ItemCnt * 6 + 2; i++)
        {
            SlotSkillObject[0].transform.localPosition -= new Vector3(0, 50f, 0);
            if (SlotSkillObject[0].transform.localPosition.y < 50f)
            {
                SlotSkillObject[0].transform.localPosition += new Vector3 (0, 300f, 0); 
            }
            yield return null;
        }
        for ( int i = 0; i < ItemCnt; i++)
        {
            Slot[i].interactable = true;
        }

       
    }
        IEnumerator StartSlot2()
    {
        for ( int i = 0; i < (ItemCnt * 10 + 0) * 2 ; i++)
        {
            SlotSkillObject[1].transform.localPosition -= new Vector3(0, 50f, 0);
            if (SlotSkillObject[1].transform.localPosition.y < 50f)
            {
                SlotSkillObject[1].transform.localPosition += new Vector3 (0, 300f, 0); 
            }
            yield return null;
        }
        for ( int i = 0; i < ItemCnt; i++)
        {
            Slot[i].interactable = true;
        }

       
    }
        IEnumerator StartSlot3()
    {
        for ( int i = 0; i < (ItemCnt * 14 + 1) * 2; i++)
        {
            SlotSkillObject[2].transform.localPosition -= new Vector3(0, 50f, 0);
            if (SlotSkillObject[2].transform.localPosition.y < 50f)
            {
                SlotSkillObject[2].transform.localPosition += new Vector3 (0, 300f, 0); 
            }
            yield return null;
        }
        for ( int i = 0; i < ItemCnt; i++)
        {
            Slot[i].interactable = true;
        }
    }
    public void ClickBtn(int index)
    {
        DisplayResultImage.sprite = SkillSprite[ResultIndexList[index]];
    }
}
