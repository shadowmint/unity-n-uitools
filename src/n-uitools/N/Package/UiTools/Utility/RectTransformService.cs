using Boo.Lang;
using UnityEditorInternal;
using UnityEngine;

namespace Articles.UiTools
{
  public class RectTransformService
  {
    public RectTransformAnchors GetAnchorsFrom(RectTransform transform)
    {
      if (transform == null) return default(RectTransformAnchors);
      return new RectTransformAnchors()
      {
        TopLeft = new Vector2(transform.anchorMin.x, transform.anchorMax.y),
        TopRight = new Vector2(transform.anchorMax.x, transform.anchorMax.y),
        BottomLeft = new Vector2(transform.anchorMin.x, transform.anchorMin.y),
        BottomRight = new Vector2(transform.anchorMax.x, transform.anchorMin.y),
        WidthFactor = transform.anchorMax.x - transform.anchorMin.x,
        HeightFactor = transform.anchorMax.y - transform.anchorMin.y
      };
    }

    public RectTransformOffsets GetOffsetsFrom(RectTransform transform)
    {
      if (transform == null) return default(RectTransformOffsets);
      var state = StateFrom(transform);
      var parts = GetAnchorsFrom(transform);

      var width = 0f;
      var height = 0f;

      // Try to get parent?
      if (state.HasParent)
      {
        width = state.Parent.rect.width * parts.WidthFactor;
        height = state.Parent.rect.height * parts.HeightFactor;
      }

      return new RectTransformOffsets()
      {
        TopRight = transform.offsetMax,
        BottomLeft = transform.offsetMin,
        Height = height + transform.offsetMax.y - transform.offsetMin.y,
        Width = width + transform.offsetMax.x - transform.offsetMin.x,
      };
    }

    /// <summary>
    /// Get the calculated size of a component.
    /// </summary>
    public Vector2 GetSize(RectTransformState state)
    {
      var widthFactor = state.Transform.anchorMax.x - state.Transform.anchorMin.x;
      var heightFactor = state.Transform.anchorMax.y - state.Transform.anchorMin.y;
      var width = 0f;
      var height = 0f;

      // Try to get parent?
      if (state.HasParent)
      {
        width = state.Parent.rect.width * widthFactor;
        height = state.Parent.rect.height * heightFactor;
      }

      var y = height + state.Transform.offsetMax.y - state.Transform.offsetMin.y;
      var x = width + state.Transform.offsetMax.x - state.Transform.offsetMin.x;
      return new Vector2(x, y);
    }

    /// <summary>
    /// Create a new persistent state object representing the transform.
    /// </summary>
    public RectTransformState StateFrom(RectTransform target)
    {
      return new RectTransformState(target);
    }

    /// <summary>
    /// Move the pivot of the target component state to the given absolute offset from it's bottom left anchor.
    /// </summary>
    public void Move(RectTransformState state, Vector2 value)
    {
      state.Transform.anchoredPosition = value;
    }
  }
}