using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Rigidbody _prefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private Cube _cube;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;

    private Ray _ray;
    private Transform _transform;
    private List<Rigidbody> _explodableCubes = new();
    private int _maxPercentProbability = 100;

    private void Update()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetButtonDown("Fire1") & Physics.Raycast(_ray, out hit, Mathf.Infinity))
        {
            _transform = hit.transform;

            if (hit.transform.GetComponent(_cube.GetType()))
            {
                Destroy(hit.transform.gameObject);

                if (TryCallMethod())
                {
                    Spawn();
                    Explode();
                    DecreaseProbability();
                }
            }
        }
    }

    private void Spawn()
    {
        Rigidbody gameObject;
        float positionY = 0;
        float angleDegreesX = 0;
        float angleDegreesZ = 0;
        float radiusInstantiation = 0.3f;
        int minNumberObjects = 2;
        int maxNumberObjects = 6;
        int multiplier = 2;
        int numberObjects = Random.Range(minNumberObjects, maxNumberObjects);

        _explodableCubes.Clear();

        for (int i = 0; i < numberObjects; i++)
        {
            float angle = i * Mathf.PI * multiplier / numberObjects;
            float positionX = Mathf.Cos(angle) * radiusInstantiation;
            float positionZ = Mathf.Sin(angle) * radiusInstantiation;
            Vector3 position = _transform.position + new Vector3(positionX, positionY, positionZ);
            float angleDegreesY = -angle * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(angleDegreesX, angleDegreesY, angleDegreesZ);
            gameObject = Instantiate(_prefab, position, rotation);
            gameObject.transform.localScale = _transform.localScale;
            gameObject.GetComponent<Cube>().ChangeScale();
            gameObject.GetComponent<Cube>().ChangeColor();
            _explodableCubes.Add(gameObject);
        }
    }

    private void Explode()
    {
        foreach (Rigidbody explodableCube in _explodableCubes)
        {
            explodableCube.AddExplosionForce(_explosionForce, _transform.position, _explosionRadius);
        }
    }

    private bool TryCallMethod()
    {
        int minNumber = 0;
        int maxNumber = 100;
        return Random.Range(minNumber, maxNumber) <= _maxPercentProbability;
    }

    private void DecreaseProbability()
    {
        int reduceNumber = 2;
        _maxPercentProbability /= reduceNumber;
    }
}