using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

public interface IDataView<in T>
{
    void SetData(T data);
}

[Serializable]
public class SimpleList<TView, TData> where TView : MonoBehaviour, IDataView<TData>
{
    [SerializeField] protected TView view;

    // Помечено как SerializeField чтобы бехавиор с SimpleList можно было копировать через Instantiate()
    [SerializeField] [HideInInspector] protected List<TView> instances = new();
    [SerializeField] [HideInInspector] protected List<GameObject> separators = new() { null };
    [SerializeField] [HideInInspector] private int _curLength;
    private Transform _container;
    public GameObject separator;

    private bool _inited;

    public SimpleList()
    {
    }

    public SimpleList(TView prefab, GameObject separator = null)
    {
        view = prefab;
        this.separator = separator;
    }

    public TView GetPrefab()
    {
        Init();
        return view;
    }

    public int ItemsCount => _curLength;

    private void Awake()
    {
        Init();
    }

    public Transform Container
    {
        get
        {
            Init();
            return _container;
        }
    }

    private void Init()
    {
        if (_inited)
            return;
        _inited = true;
        OnInit();
    }

    protected virtual void OnInit()
    {
        _container = view.transform.parent;
        view.gameObject.SetActive(false);
    }

    public TView GetElement(int index)
    {
        return instances[index];
    }

    public IEnumerable<TView> Elements()
    {
        return instances.Take(_curLength);
    }

    public TView Find(Predicate<TView> predicate)
    {
        for (var i = 0; i < _curLength; i++)
        {
            var component = instances[i];
            if (component != null && predicate(component))
                return component;
        }

        return null;
    }

    public int FindIndex(Predicate<TView> predicate)
    {
        for (var i = 0; i < _curLength; i++)
        {
            var component = instances[i];
            if (component != null && predicate(component))
                return i;
        }

        return -1;
    }

    public void Clear()
    {
        for (var i = 0; i < _curLength; i++)
            instances[i].gameObject.SetActive(false);
        _curLength = 0;
    }

    protected virtual TView Instantiate(int index)
    {
        var obj = Object.Instantiate(view, _container);
        obj.gameObject.name = $"{view.name} #{index}";
        return obj;
    }

    public void SetData(IEnumerable<TData> dataCollection)
    {
        Init();
        var i = 0;
        if (dataCollection != null)
            foreach (var data in dataCollection)
            {
                var element = i < instances.Count ? instances[i] : AddElement(i);
                element.gameObject.SetActive(true);
                element.SetData(data);
                i++;
            }

        var newLength = i;
        for (; i < _curLength; i++)
            instances[i].gameObject.SetActive(false);
        if (separator)
        {
            var max = Mathf.Max(newLength, _curLength);
            for (var j = 1; j < max; j++)
                separators[j].SetActive(j < newLength);
        }

        _curLength = newLength;
    }

    private TView AddElement(int index)
    {
        if (instances.Count == separators.Count && separator)
            separators.Add(Object.Instantiate(separator, _container));
        var element = Instantiate(index);
        instances.Add(element);
        return element;
    }

    public int IndexOf(TView comp)
    {
        return instances.IndexOf(comp);
    }

    public int IndexOf(GameObject go)
    {
        return instances.FindIndex(x => x.gameObject == go);
    }
}