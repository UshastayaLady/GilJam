using TMPro;
using UnityEngine;

public class DisplayCountView : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI _text;

   public void UpdateCount(int count)
   {
      _text.text = count.ToString();
   }
}
