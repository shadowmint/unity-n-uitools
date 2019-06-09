using System;
using System.Collections.Generic;
using System.Linq;
using N.Package.UiTools.Utility.Model;
using UnityEngine;

namespace N.Package.UiTools.Infrastructure
{
    [System.Serializable]
    public class LayoutState
    {
        private const int UpdateThresholdFrames = 4;

        private int _childCount;

        private bool _childrenChanged;

        private int _childConstantFrames;

        public bool requireManual = false;

        public bool executeManual = false;

        public bool applyLayout = true;

        public bool collectChildren = true;

        private readonly List<RectTransformState> _children = new List<RectTransformState>();

        public void Update(ILayoutComponent layout)
        {
            if (requireManual && !executeManual) return;
            if (executeManual)
            {
                executeManual = false;
            }

            DetectChildChanges(layout);
            CollectChildrenForLayout(layout);
            ExecuteLayout(layout);
        }

        private void CollectChildrenForLayout(ILayoutComponent layout)
        {
            if (!collectChildren) return;
            collectChildren = false;

            _children.Clear();
            var transform = layout.RectTransform;
            var count = transform.childCount;
            for (var i = 0; i < count; i++)
            {
                var child = transform.GetChild(i) as RectTransform;
                _children.Add(new RectTransformState(child));
            }
        }

        private void DetectChildChanges(ILayoutComponent layout)
        {
            var transform = layout.RectTransform;

            // If we're waiting to spool a change, wait for the child count to settle so we don't spam.
            if (_childrenChanged)
            {
                if (_childCount == transform.childCount)
                {
                    _childConstantFrames += 1;
                    if (_childConstantFrames > UpdateThresholdFrames)
                    {
                        _childrenChanged = false;
                        collectChildren = true;
                        applyLayout = true;
                    }
                }
                else
                {
                    _childCount = transform.childCount;
                    _childConstantFrames = 0;
                }

                return;
            }

            // Otherwise, start watching if it changed
            if (transform.childCount != _children.Count)
            {
                _childrenChanged = true;
                _childCount = transform.childCount;
                _childConstantFrames = 1;
            }
        }

        private void ExecuteLayout(ILayoutComponent layout)
        {
            if (!applyLayout) return;
            applyLayout = false;

            var state = layout.Prepare();

            var activeChildren = _children.Where(i =>
            {
                try
                {
                    return i.Transform.transform.gameObject.activeInHierarchy;
                }
                catch (Exception)
                {
                    return false;
                }
            }).ToList();

            var count = activeChildren.Count;
            state.Count = count;

            for (var i = 0; i < count; i++)
            {
                state.Child = activeChildren[i];
                state.Offset = i;
                layout.Apply(state);
            }

            layout.Complete(state);
        }
    }
}