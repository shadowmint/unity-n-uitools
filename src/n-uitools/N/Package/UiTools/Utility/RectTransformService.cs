using System;
using N.Package.Flow;
using N.Package.UiTools.Utility.Model;
using UnityEngine;

namespace N.Package.UiTools.Utility
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

        /// <summary>
        /// Apply a rect layout from a template onto a target.
        /// </summary>
        public void ApplyTransform(RectTransform layout, RectTransform transform)
        {
            transform.anchorMin = layout.anchorMin;
            transform.anchorMax = layout.anchorMax;
            transform.offsetMin = layout.offsetMin;
            transform.offsetMax = layout.offsetMax;
            transform.pivot = layout.pivot;
            transform.anchoredPosition3D = layout.anchoredPosition3D;
            transform.localScale = layout.localScale;
            transform.rotation = layout.rotation;
        }

        /// <summary>
        /// Apply a rect layout from a template onto a target.
        /// </summary>
        public void ApplyTransform(RectTransform layout, FlowComponentProperties properties)
        {
            var transform = properties.GetComponent<RectTransform>();
            ApplyTransform(layout, transform);
        }

        /// <summary>
        /// Apply a rect layout from a template onto a target.
        /// </summary>
        public void ApplyTransform(RectTransform layout, GameObject target)
        {
            var transform = target.GetComponent<RectTransform>();
            ApplyTransform(layout, transform);
        }

        /// <summary>
        /// Recall the size is defined by the offsetMin -> offsetMax from the anchor point.
        /// This function therefore moves and sets the absolute size of the object; you can't
        /// just set one and then the other.
        ///
        /// As a side effect, the transform is reset to bottom left for this operation.
        /// Note that sizes are absolute.
        /// </summary>
        public void MoveAndResize(RectTransformState state, Vector2 size, Vector2 center)
        {
            SetAnchorPreset(state, RectTransformAnchorPresetX.Left, RectTransformAnchorPresetY.Bottom);
            state.Transform.offsetMin = center - size / 2;
            state.Transform.offsetMax = center + size / 2;
        }

        /// <summary>
        /// Set the fixed size component of the transform.
        /// </summary>
        public void SetFixedSizeFromEdge(RectTransformState state, float size, RectTransformEdge edge, float offset = 0f)
        {
            switch (edge)
            {
                case RectTransformEdge.Left:
                    state.Transform.offsetMax = new Vector2(size + offset, 0);
                    state.Transform.offsetMin = new Vector2(offset, 0);
                    break;
                case RectTransformEdge.Right:
                    state.Transform.offsetMin = new Vector2(-size - offset, 0);
                    state.Transform.offsetMax = new Vector2(-offset, 0);
                    break;
                case RectTransformEdge.Top:
                    state.Transform.offsetMin = new Vector2(0, -size - offset);
                    state.Transform.offsetMax = new Vector2(0, -offset);
                    break;
                case RectTransformEdge.Bottom:
                    state.Transform.offsetMax = new Vector2(0, size + offset);
                    state.Transform.offsetMin = new Vector2(0, offset);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(edge), edge, null);
            }
        }

        public void SetAnchorPreset(RectTransformState state, RectTransformAnchorPresetX x, RectTransformAnchorPresetY y)
        {
            Vector3 yv;
            switch (y)
            {
                case RectTransformAnchorPresetY.Top:
                    yv = new Vector3(1, 1, 0.5f);
                    break;
                case RectTransformAnchorPresetY.Middle:
                    yv = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
                case RectTransformAnchorPresetY.Bottom:
                    yv = new Vector3(0, 0, 0.5f);
                    break;
                case RectTransformAnchorPresetY.Stretch:
                    yv = new Vector3(0, 1, 0.5f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(y), y, null);
            }

            Vector3 xv;
            switch (x)
            {
                case RectTransformAnchorPresetX.Left:
                    xv = new Vector3(0, 0, 0.5f);
                    break;
                case RectTransformAnchorPresetX.Center:
                    xv = new Vector3(0.5f, 0.5f, 0.5f);
                    break;
                case RectTransformAnchorPresetX.Right:
                    xv = new Vector3(1, 1, 0.5f);
                    break;
                case RectTransformAnchorPresetX.Stretch:
                    xv = new Vector3(0, 1, 0.5f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(x), x, null);
            }

            state.Transform.anchorMin = new Vector2(yv[0], xv[0]);
            state.Transform.anchorMax = new Vector2(yv[1], xv[1]);
            state.Transform.pivot = new Vector2(yv[2], xv[2]);
        }
    }
}