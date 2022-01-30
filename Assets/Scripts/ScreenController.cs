using System.Collections.Generic;
using Enums;
using Pixelplacement;

namespace DefaultNamespace
{
    public class ScreenController: Singleton<ScreenController>
    {
        public List<ItemType> ActivatedItems;
    }
}