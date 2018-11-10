using System;
using N.Package.UiTools.Infrastructure;
using N.Package.UiTools.Utility;
using UnityEngine;

namespace N.Package.UiTools.Components
{
  [ExecuteInEditMode]
  [RequireComponent(typeof(RectTransform))]
  public class UiVerticalLayout : MonoBehaviour, ILayoutComponent
  {
    [Tooltip("Set the size of the container to the height of its content once the render is done?")]
    public bool AdjustContainerSize = true;

    public UiVerticalLayoutDirection Direction;

    public float Padding = 0f;

    public LayoutState Layout;

    public LayoutState State => Layout;

    public RectTransformService Service => _service.Value;

    public RectTransform RectTransform
    {
      get
      {
        if (_rectTransform == null)
        {
          _rectTransform = GetComponent<RectTransform>();
        }

        return _rectTransform;
      }
    }

    private RectTransform _rectTransform;

    private readonly Lazy<RectTransformService> _service = new Lazy<RectTransformService>(() => new RectTransformService());

    public ILayoutComponentState Prepare()
    {
      return new UiVerticalLayoutState()
      {
        Direction = Direction == UiVerticalLayoutDirection.Down ? -1f : 1f
      };
    }

    public void Apply(ILayoutComponentState raw)
    {
      var state = raw as UiVerticalLayoutState;
      if (state == null) return;

      var size = Service.GetSize(state.Child);
      Service.Move(state.Child, new Vector2(0, state.LayoutOffset));

      state.LayoutOffset += (size.y + Padding) * state.Direction;
    }

    public void Complete(ILayoutComponentState raw)
    {
      var state = raw as UiVerticalLayoutState;
      if (state == null) return;

      if (AdjustContainerSize)
      {
        var size = RectTransform.sizeDelta;
        var delta = Mathf.Abs(Mathf.Abs(state.LayoutOffset) - Mathf.Abs(size.y));
        if (delta < 0.1f)
        {
          return;
        }

        if (Direction == UiVerticalLayoutDirection.Down)
        {
          RectTransform.offsetMax = new Vector2(RectTransform.offsetMax.x, 0);
          RectTransform.offsetMin = new Vector2(RectTransform.offsetMax.x, state.LayoutOffset);
        }
        else
        {
          RectTransform.offsetMax = new Vector2(RectTransform.offsetMax.x, state.LayoutOffset);
          RectTransform.offsetMin = new Vector2(RectTransform.offsetMax.x, 0);
        }
      }
    }

    public void Update()
    {
      State?.Update(this);
    }
  }
}