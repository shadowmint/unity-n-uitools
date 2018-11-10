using N.Package.UiTools.Infrastructure;
using N.Package.UiTools.Utility.Model;

namespace N.Package.UiTools.Components
{
  public class UiVerticalLayoutState : ILayoutComponentState
  {
    public float LayoutOffset { get; set; }
    public int Count { get; set; }
    public int Offset { get; set; }
    public RectTransformState Child { get; set; }
    public float Direction { get; set; }
  }
}