using System;
using Articles.UiTools;
using UnityEngine;

namespace Articles
{
  [ExecuteInEditMode]
  public class RectTransformTests : MonoBehaviour
  {
    public RectTransform Target;
    public RectTransformTestType Task;
    public Vector2 Data1;

    private readonly Lazy<RectTransformService> _service = new Lazy<RectTransformService>(() => new RectTransformService());

    public void Update()
    {
      if (Task == RectTransformTestType.Nothing) return;
      DispatchTask(Task);
      Task = RectTransformTestType.Nothing;
    }

    private void DispatchTask(RectTransformTestType task)
    {
      switch (task)
      {
        case RectTransformTestType.Nothing:
          break;
        case RectTransformTestType.MoveToTopLeft:
          var state = _service.Value.StateFrom(Target);
          _service.Value.Move(state, Data1);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(task), task, null);
      }
    }
  }
}