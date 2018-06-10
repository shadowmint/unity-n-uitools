using Articles.UiTools.Infrastructure;

namespace Articles.UiTools.Components
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