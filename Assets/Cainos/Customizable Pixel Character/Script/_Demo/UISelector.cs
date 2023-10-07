using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace Cainos.CustomizablePixelCharacter
{
    public class UISelector : MonoBehaviour
    {
        public TextMeshProUGUI text;
        [Space]
        public List<string> items;
        [Space]
        public UISelectorValueChanged onValueChange;

        public int Index
        {
            get { return index; }
            set
            {
                index = value;

                if (items == null) return;

                while (index < 0) index += items.Count;
                while (index >= items.Count) index -= items.Count;

                if ( items.Count > 0) text.text = items[index];

                onValueChange.Invoke(index);
            }
        }
        private int index;


        public void Prev()
        {
            Index--;
        }

        public void Next()
        {
            Index++;
        }

        [System.Serializable]
        public class UISelectorValueChanged : UnityEvent<int>{ }

    }
}
