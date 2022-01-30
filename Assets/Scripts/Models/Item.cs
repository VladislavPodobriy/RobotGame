using System;
using Enums;
using UnityEngine;

namespace Models
{
    [Serializable]
    public class Item
    {
        public ItemType Type;
        public Sprite Sprite;
    }
}