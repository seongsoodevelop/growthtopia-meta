using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NetworkPacketReceiver;

public class RoomElementManager : MonoBehaviour
{
    [Serializable]
    public class ElementList
    {
        public List<Element> list;
    }

    public static RoomElementManager inst { get; private set; }

    [SerializeField]
    public List<ElementList> elementPrefabs;

    private void Awake()
    {
        inst = this;
    }

    public void Init(List<PropertyElement> elements)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var element in elements)
        {
            try
            {
                Element inst = Instantiate(elementPrefabs[element.type].list[element.code].gameObject, transform).GetComponent<Element>();
                inst.transform.name = $"{element.id} {elementPrefabs[element.type].list[element.code].name}";
                inst.Init(element.id, new Vector3(element.position_x, element.position_y, element.position_z));
            }
            catch { }
        }
    }

    public void OnRoomQuit()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
