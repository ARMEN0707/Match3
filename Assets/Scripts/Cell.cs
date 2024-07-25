using System;
using UnityEngine;
using FuryLion.UI;
using FuryLion.ObjectPool;

public class Cell : Element
{
    [SerializeField] private CellType _type;
    [SerializeField] protected SpriteRenderer _sprite;
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _outlineMaterial;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speed = 4f;
    [SerializeField] private float _timeDisappearance = 1f;

    [HideInInspector] public int X;
    [HideInInspector] public int Y;
    [HideInInspector] public bool Disappearance;
    public Vector3? TargetPosition = null;
    public event Action Disappeared;

    private float _time;

    public CellType Type => _type;

    protected void Update()
    {
        if(TargetPosition != null)
            Move();

        if (Disappearance)
        {
            _time += Time.deltaTime;
            if (_time > _timeDisappearance)
            {
                Disappeared?.Invoke();
                CellPool.Release(this);
            }
        }
    }

    public void Init()
    {
        _time = 0;
        Disappearance = false;
        TargetPosition = null;
        Disappeared = null;
    }

    public void SetOutline()
    {
        _sprite.material = _outlineMaterial;
        _animator.SetBool("Selected", true);
    }

    public void RemoveOutline()
    {
        _sprite.material = _defaultMaterial;
        _animator.SetBool("Selected", false);
    }

    public void SetRemoveAnimation()
    {
        _animator.SetTrigger("Remove");
    }

    public void Move()
    {
        if (transform.position != TargetPosition)
            transform.position = Vector3.Lerp(transform.position, TargetPosition.Value, _speed * Time.deltaTime);
        else
            TargetPosition = null;
    }

}
