using System;
using N.Package.UiTools.Infrastructure;
using N.Package.UiTools.Utility;
using N.Package.UiTools.Utility.Model;
using UnityEngine;

namespace N.Package.UiTools.Components
{
    public class UiGridLayoutState : ILayoutComponentState
    {
        public int Count { get; set; }
        public int Offset { get; set; }
        public RectTransformState Child { get; set; }
        public float UnitSizeX { get; set; }
        public float TotalY { get; set; }
        public int Row;
        public int Col;
    }

    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    public class UiGridLayout : MonoBehaviour, ILayoutComponent
    {
        public int cellsPerRow = 2;

        public float padding = 0f;

        public LayoutState Layout;

        public LayoutState State => Layout;

        public RectTransformService Service => _service.Value;

        public RectTransform RectTransform => RectTransformState.Transform;

        public RectTransformState RectTransformState
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = new RectTransformState(GetComponent<RectTransform>());
                }

                return _rectTransform;
            }
        }

        private RectTransformState _rectTransform;

        private readonly Lazy<RectTransformService> _service = new Lazy<RectTransformService>(() => new RectTransformService());

        public ILayoutComponentState Prepare()
        {
            var size = Service.GetSize(RectTransformState);
            var totalXPadding = (cellsPerRow - 1) * padding;
            return new UiGridLayoutState()
            {
                UnitSizeX = (size.x - totalXPadding) / cellsPerRow,
                TotalY = size.y,
                Row = 0,
                Col = 0
            };
        }

        public void Apply(ILayoutComponentState raw)
        {
            var state = raw as UiGridLayoutState;
            if (state == null) return;

            var rows = (int) Math.Ceiling(state.Count / (float) cellsPerRow);
            var totalPadding = padding * (rows - 1);
            var cellHeight = (state.TotalY - totalPadding) / rows;

            // Layout from the top left
            var positionY = state.TotalY - cellHeight - state.Row * padding - state.Row * cellHeight + cellHeight / 2f;
            var positionX = state.Col * padding + state.Col * state.UnitSizeX + state.UnitSizeX / 2f;

            Service.MoveAndResize(state.Child, new Vector2(state.UnitSizeX, cellHeight), new Vector2(positionX, positionY));
            
            // Step
            state.Col += 1;
            if (state.Col >= cellsPerRow)
            {
                state.Row += 1;
                state.Col = 0;
            }
        }

        public void Complete(ILayoutComponentState raw)
        {
            var state = raw as UiGridLayoutState;
            if (state == null) return;
        }

        public void Update()
        {
            State?.Update(this);
        }
    }
}