using System.Collections.Generic;
using UnityEngine;

namespace Articles.UiTools.Infrastructure
{
  [System.Serializable]
  public class LayoutState
  {
    public bool ApplyLayout = true;

    public bool CollectChildren = true;

    private readonly List<RectTransformState> _children = new List<RectTransformState>();

    public void Update(ILayoutComponent layout)
    {
      CollectChildrenForLayout(layout);
      ExecuteLayout(layout);
    }

    private void CollectChildrenForLayout(ILayoutComponent layout)
    {
      var transform = layout.RectTransform;
      if (transform.childCount != _children.Count)
      {
        CollectChildren = true;
        ApplyLayout = true;
      }

      if (!CollectChildren) return;
      CollectChildren = false;

      _children.Clear();
      var count = transform.childCount;
      for (var i = 0; i < count; i++)
      {
        var child = transform.GetChild(i) as RectTransform;
        _children.Add(new RectTransformState(child));
      }
    }

    private void ExecuteLayout(ILayoutComponent layout)
    {
      if (!ApplyLayout) return;
      ApplyLayout = false;

      var state = layout.Prepare();
      var count = _children.Count;
      state.Count = count;

      for (var i = 0; i < count; i++)
      {
        state.Child = _children[i];
        state.Offset = i;
        layout.Apply(state);
      }

      layout.Complete(state);
    }
  }
}