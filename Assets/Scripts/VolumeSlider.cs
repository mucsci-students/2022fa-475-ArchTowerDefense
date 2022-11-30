using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VolumeSlider : MonoBehaviour, IDropHandler, IBeginDragHandler
{ 
    public void OnDrop(PointerEventData data)
    {
        GetComponent<AudioSource>().Stop();
    }
 
    public void OnBeginDrag(PointerEventData data)
    {
        GetComponent<AudioSource>().Play();
    }
}
