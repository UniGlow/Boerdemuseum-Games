using UnityEngine;
using cakeslice;

public class OutlineAnimation : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] float alphaMax = 1f;
    [Range(0, 1)]
    [SerializeField] float alphaMin = 0f;
    [Range(0.5f, 10f)]
    [SerializeField] float duration = 1f;
    bool pingPong = false;
    OutlineEffect outlineEffect;
    float fadeInModifier = 1f;



    // Update is called once per frame
    private void Start()
    {
        outlineEffect = GetComponent<OutlineEffect>();
    }


    void Update()
    {
        Color c = outlineEffect.lineColor0;

        if(pingPong)
        {
            c.a += Time.deltaTime * (1 / duration) * fadeInModifier;

            if(c.a >= alphaMax)
            {
                fadeInModifier = 1f;
                pingPong = false;
            }
        }
        else
        {
            c.a -= Time.deltaTime * (1 / duration);

            if(c.a <= alphaMin)
                pingPong = true;
        }

        c.a = Mathf.Clamp01(c.a);
        outlineEffect.lineColor0 = c;
        outlineEffect.UpdateMaterialsPublicProperties();
    }



    public void ResetOutline()
    {
        outlineEffect.lineColor0 = new Color(outlineEffect.lineColor0.r, outlineEffect.lineColor0.g, outlineEffect.lineColor0.b, 0f);
        fadeInModifier = 4f;
        pingPong = true;
    }
}