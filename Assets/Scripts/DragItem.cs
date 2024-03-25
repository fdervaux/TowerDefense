using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject _copyPrefab;
    private GameObject _copy;
    [SerializeField] private LayerMask _layerMask;
    private List<Material> materials = new List<Material>();
    private Color InvalidColor = Colors.FromHex("FF6A3B");
    private Color ValidColor = Colors.FromHex("89BBB2");
    [SerializeField] private TowerData _towerData;
    [SerializeField] private TextMeshPro _costText;
    private List<Material> _materials = new List<Material>();

    bool _canDrag = false;
    bool _isDragging = false;

    public void Start()
    {
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            _materials.Add(renderer.material);
        }
        _costText.text = _towerData.cost.ToString();
    }

    public void Update()
    {
        if(GameManager.Instance.Gold < _towerData.cost)
        {
            foreach (var renderer in _materials)
            {
                renderer.SetColor("_BaseColor", InvalidColor);
                _canDrag = false;
            }
        }
        else
        {
            foreach (var renderer in _materials)
            {
                renderer.SetColor("_BaseColor", ValidColor);
                _canDrag = true;
            }
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!_canDrag)
        {
            return;
        }

        _isDragging = true;

        _copy = Instantiate(_copyPrefab, transform.position, Quaternion.identity, transform.parent);

        materials.Clear();
        foreach (var renderer in _copy.GetComponentsInChildren<Renderer>())
        {
            materials.Add(renderer.material);
            renderer.material.SetColor("_BaseColor", InvalidColor);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_isDragging)
        {
            return;
        }

        Ray R = Camera.main.ScreenPointToRay(Input.mousePosition); 
        RaycastHit hit; 
        if (Physics.Raycast(R, out hit, 1000, _layerMask)) 
        {
            _copy.transform.position = hit.point; 

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                PlatformController platform = hit.collider.gameObject.GetComponentInParent<PlatformController>();

                if (platform.IsEmpty)
                {
                    foreach (var renderer in _copy.GetComponentsInChildren<Renderer>())
                    {
                        renderer.material.SetColor("_BaseColor", ValidColor);
                    }
                    return;
                }
            }

            foreach (var renderer in _copy.GetComponentsInChildren<Renderer>())
            {
                renderer.material.SetColor("_BaseColor", InvalidColor);
            }

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isDragging)
        {
            return;
        }   

        _isDragging = false;

        Ray R = Camera.main.ScreenPointToRay(Input.mousePosition); 
        RaycastHit hit;
        if (Physics.Raycast(R, out hit, 1000, _layerMask))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                PlatformController platform = hit.collider.gameObject.GetComponentInParent<PlatformController>();
                if (platform.IsEmpty)
                {
                    platform.IsEmpty = false;
                    _copy.transform.position = hit.collider.transform.parent.position;
                    _copy.GetComponent<TowerController>().SetActive(true);
                    GameManager.Instance.UpdateGold(-_towerData.cost);
                    return;
                }
            }

        }

        Destroy(_copy);
    }
}

