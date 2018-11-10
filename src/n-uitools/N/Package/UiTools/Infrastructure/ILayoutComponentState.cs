using N.Package.UiTools.Utility.Model;

namespace N.Package.UiTools.Infrastructure
{
  public interface ILayoutComponentState
  {
    int Count { get; set; }
    int Offset { get; set; }
    RectTransformState Child { get; set; }
  }
}