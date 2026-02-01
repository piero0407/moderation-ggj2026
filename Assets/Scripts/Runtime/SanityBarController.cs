using Scriptable_Objects_Architecture.Runtime.Variables;
using UnityEngine;
using UnityEngine.UI;

public class SanityBarController : MonoBehaviour
{
    [SerializeField] private Slider sanityBarFill;
    [SerializeField] private FloatVariable sanity;

    [SerializeField] private Image maskImage;
    [SerializeField] private Sprite happyMask, sadMask;

    void Start()
    {
        maskImage.sprite = happyMask;
    }

    void Update()
    {
        sanityBarFill.value = sanity.Value;

        if (sanity.Value <= 0.45) maskImage.sprite = sadMask;
        else maskImage.sprite = happyMask;
    }
}
