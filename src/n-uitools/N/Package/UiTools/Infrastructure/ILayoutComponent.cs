using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Articles.UiTools.Infrastructure
{
  public interface ILayoutComponent
  {
    RectTransform RectTransform { get; }
    LayoutState State { get; }

    /// <summary>
    /// Prepare the state for a layout run.
    /// </summary>
    ILayoutComponentState Prepare();

    /// <summary>
    /// Apply the layout to a single child component.
    /// </summary>
    void Apply(ILayoutComponentState state);

    /// <summary>
    /// Execute any post-layout actions.
    /// </summary>
    void Complete(ILayoutComponentState state);
  }
}