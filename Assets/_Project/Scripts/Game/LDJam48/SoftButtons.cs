using UnityEngine;
using Util.Var;
using Util.Var.Observe;

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