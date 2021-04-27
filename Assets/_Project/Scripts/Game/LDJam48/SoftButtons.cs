using UnityEngine;
using Util.Variable;

namespace LDJam48
{
    public class SoftButtons : MonoBehaviour
    {
        [SerializeField] private int sign;
        [SerializeField] private ObservableIntVariable axis;



        public void SetHorizontal()
        {
            axis.Value = sign;
        }
    }
}