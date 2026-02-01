using Scriptable_Objects_Architecture.Runtime.Variables;
using UnityEngine;
using UnityEngine.UI;

public class SanityBarController : MonoBehaviour
{
    [SerializeField] private Slider sanityBarFill;
    [SerializeField] private FloatVariable sanity; // Reference to the sanity variable

    void Update()
    {
        // Assuming sanity.Value is between 0 and 1
        sanityBarFill.value = sanity.Value;
    }
}
