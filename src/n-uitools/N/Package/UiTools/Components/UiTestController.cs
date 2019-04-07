using System;
using N.Package.Flow;
using UnityEngine;

namespace N.Package.UiTools.Components
{
    /// <summary>
    /// This is just a common base constructor for UI components to test with.
    /// </summary>
    public class UiTestController<TComponent, TState> : FlowController<TState>
        where TState : FlowComponentState
        where TComponent : class, IFlowComponent
    {
        public GameObject rootContainer;
        
        private TComponent _component;

        public void Start()
        {
            var instance = Activator.CreateInstance(typeof(TComponent), MapState(State)) as TComponent;
            _component = instance;
        }

        protected virtual object MapState(TState state)
        {
            return state;
        }

        protected override IFlowComponent OnComponentLayout()
        {
            State.Container = () => rootContainer == null ? gameObject : rootContainer;
            return _component;
        }
    }
}