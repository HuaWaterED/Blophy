using System.Collections;
using UnityEngine;

public class RippleController : MonoBehaviour
{
    public float currentScaleX = 1;
    public float currentScaleY = 1;
    public SpriteRenderer[] texture;
    public RippleController Init(float currentScaleX, float currentScaleY)
    {
        this.currentScaleX = currentScaleX;
        this.currentScaleY = currentScaleY;
        return this;
    }
    private void OnEnable()
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
        for (int i = 0; i < texture.Length; i++)
        {
            texture[i].color = Color.black * .5f;
        }
        texture[0].transform.localScale =//第12根线都是水平的
            texture[1].transform.localScale =
            new Vector2(2 - (ValueManager.Instance.boxFineness / currentScaleX), ValueManager.Instance.boxFineness / currentScaleY);

        texture[2].transform.localScale =//第34都是垂直的
            texture[3].transform.localScale =
            new Vector2(2 + (ValueManager.Instance.boxFineness / currentScaleY), ValueManager.Instance.boxFineness / currentScaleX);
    }
    private void Update()
    {
        transform.localScale = transform.localScale + Vector3.one * Time.deltaTime;
        for (int i = 0; i < texture.Length; i++)
        {
            texture[i].color = new(0, 0, 0, texture[i].color.a - Time.deltaTime / 2);
        }
    }
}
