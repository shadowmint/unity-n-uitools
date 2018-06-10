namespace Articles.UiTools.Infrastructure
{
  public interface ILayoutComponentState
  {
    int Count { get; set; }
    int Offset { get; set; }
    RectTransformState Child { get; set; }
  }
}