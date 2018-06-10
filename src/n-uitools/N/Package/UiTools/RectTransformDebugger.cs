using System;
using UnityEngine;

namespace Articles.UiTools
{
  [ExecuteInEditMode]
  public class RectTransformDebugger : MonoBehaviour
  {
    public RectTransform Target;

    [Tooltip("How often to refresh the display state from the RectTransform? (seconds)")]
    public float RefreshInterval = 0.1f;

    [Tooltip("Should the display state be updated on the next update?")]
    public bool RefreshNow = true;

    public RectTransformDebug State;

    public RectTransformAnchors Anchors;

    public RectTransformOffsets Offsets;

    private float _elapsed;

    private readonly Lazy<RectTransformService> _service = new Lazy<RectTransformService>(() => new RectTransformService());

    public void Update()
    {
      if (Target == null)
      {
        Target = GetComponent<RectTransform>();
        if (Target == null) return;
        return;
      }

      _elapsed += Time.deltaTime;
      if (_elapsed > RefreshInterval)
      {
        _elapsed = 0;
        RefreshNow = true;
      }

      if (RefreshNow)
      {
        RefreshNow = false;
        RefreshDisplayState();
      }
    }

    private void RefreshDisplayState()
    {
      State.Rect = Target.rect;
      State.AnchorMin = Target.anchorMin;
      State.AnchorMax = Target.anchorMax;
      State.AnchoredPosition3D = Target.anchoredPosition3D;
      State.AnchoredPosition = Target.anchoredPosition;
      State.SizeDelta = Target.sizeDelta;
      State.Pivot = Target.pivot;
      State.OffsetMin = Target.offsetMin;
      State.OffsetMax = Target.offsetMax;

      Anchors = _service.Value.GetAnchorsFrom(Target);
      Offsets = _service.Value.GetOffsetsFrom(Target);
    }

    [System.Serializable]
    public class RectTransformDebug
    {
      [Tooltip("The calculated rectangle in the local space of the Transform (read-only).")]
      public Rect Rect;

      [Tooltip("The normalized position in the parent RectTransform that the lower left corner is anchored to.")]
      public Vector2 AnchorMin;

      [Tooltip("The normalized position in the parent RectTransform that the upper right corner is anchored to.")]
      public Vector2 AnchorMax;

      [Tooltip("The 3D position of the pivot of this RectTransform relative to the anchor reference point.")]
      public Vector3 AnchoredPosition3D;

      [Tooltip("The position of the pivot of this RectTransform relative to the anchor reference point.")]
      public Vector2 AnchoredPosition;

      [Tooltip("The size of this RectTransform relative to the distances between the anchors.")]
      public Vector2 SizeDelta;

      [Tooltip("The normalized position in this RectTransform that it rotates around.")]
      public Vector2 Pivot;

      [Tooltip("The offset of the lower left corner of the rectangle relative to the lower left anchor.")]
      public Vector2 OffsetMin;

      [Tooltip("The offset of the upper right corner of the rectangle relative to the upper right anchor.")]
      public Vector2 OffsetMax;
    }
  }
}