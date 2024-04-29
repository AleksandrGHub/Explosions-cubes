using UnityEngine;

public class Cube : MonoBehaviour
{
    private float _divider = 2;
    private Renderer _renderer;
    private Transform _transform;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _transform = GetComponent<Transform>();
    }

    public void ChangeColor()
    {
        _renderer.material.color = Random.ColorHSV();
    }

    public void ChangeScale()
    {
        _transform.localScale /= _divider;
    }
}