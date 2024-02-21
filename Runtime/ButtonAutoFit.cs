using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mushakushi.UIToolkit.Runtime
{
    /// <summary>
    /// Auto-sizing <see cref="Button"/>.
    /// </summary>
    public class ButtonAutoFit: Button
    {
        [UnityEngine.Scripting.Preserve]
        public new class UxmlFactory : UxmlFactory<ButtonAutoFit, UxmlTraits>
        {
            public override string uxmlNamespace => "Mushakushi";
        }
        
        public ButtonAutoFit(Action clickEvent) : base(clickEvent)
        {
            TextElementAutoFitter.RegisterAutoFitCallbacks(this);  
            // bug - button alignment becomes broken 
            style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.UpperCenter);
        }
        public ButtonAutoFit(): this(null) {}
    }
}