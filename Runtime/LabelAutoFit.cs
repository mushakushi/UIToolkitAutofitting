using UnityEngine.UIElements;

namespace Mushakushi.UIToolkit.Runtime
{
    /// <summary>
    /// Auto-sizing <see cref="Label"/>.
    /// </summary>
    public class LabelAutoFit : Label
    {
        [UnityEngine.Scripting.Preserve] public new class UxmlFactory : UxmlFactory<LabelAutoFit, UxmlTraits>
        {
            public override string uxmlNamespace => "Mushakushi";
        }

        public LabelAutoFit() => TextElementAutoFitter.RegisterAutoFitCallbacks(this);
    }
}