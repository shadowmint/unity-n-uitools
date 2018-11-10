using UnityEngine;

namespace N.Package.UiTools.Utility.Model
{
  /// <summary>
  /// This is a persistent stateful class representing a rect transform to save many GetComponent() calls.
  /// </summary>
  public class RectTransformState
  {
    private readonly RectTransform _transform;
    private readonly RectTransform _parent;

    public bool HasParent => _parent != null;

    public RectTransform Transform => _transform;

    public RectTransform Parent => _parent;

    public RectTransformState(RectTransform transform)
    {
      _transform = transform;
      _parent = GetParentTransform(transform);
    }

    private RectTransform GetParentTransform(RectTransform transform)
    {
      var parent = transform.parent;
      return parent != null ? parent.GetComponent<RectTransform>() : null;
    }
  }
}