using System.Collections;
using UnityEngine.UIElements;

namespace Interfaces
{
    public interface IScreen
    {
        UIDocument UIDocument { get; }
        
        IEnumerator Show();
        IEnumerator Hide();
    }
}