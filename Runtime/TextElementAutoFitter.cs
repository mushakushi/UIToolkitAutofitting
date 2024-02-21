// workaround for: https://forum.unity.com/threads/button-text-size-in-doesnt-work.1037551/
// related: https://answers.unity.com/questions/1865976/ui-toolkit-text-best-fit.html
// related: https://forum.unity.com/threads/button-text-size-in-doesnt-work.1037551/#post-8833276
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Mushakushi.UIToolkit.Runtime
{
    /// <summary>
    /// Resizes a <see cref="TextElement"/> font size with respect to its <see cref="VisualElement.contentRect"/>. 
    /// </summary>
    public static class TextElementAutoFitter
    {
        /// <summary>
        /// Registers callbacks for the <see cref="TextElement"/> to update its font size. 
        /// </summary>
        public static void RegisterAutoFitCallbacks(TextElement textElement)
        {
            textElement.RegisterCallback<AttachToPanelEvent, TextElement>(OnAttachToPanel, textElement);
            textElement.RegisterCallback<DetachFromPanelEvent, TextElement>(OnDetachFromPanel, textElement);
        }

        private static void OnAttachToPanel(AttachToPanelEvent _, TextElement textElement)
        {
            UpdateFontSize(textElement);
            textElement.RegisterCallback<ChangeEvent<string>, TextElement>(OnChange, textElement);
            textElement.RegisterCallback<ChangeEvent<StyleFont>, TextElement>(OnChange, textElement);
            textElement.RegisterCallback<ChangeEvent<StyleFontDefinition>, TextElement>(OnChange, textElement);
            textElement.RegisterCallback<ChangeEvent<StyleLength>, TextElement>(OnChange, textElement);
            textElement.RegisterCallback<GeometryChangedEvent, TextElement>(OnChange, textElement);
            textElement.UnregisterCallback<AttachToPanelEvent, TextElement>(OnAttachToPanel);
        }

        private static void OnDetachFromPanel(DetachFromPanelEvent _, TextElement textElement)
        {
            textElement.UnregisterCallback<ChangeEvent<string>, TextElement>(OnChange);
            textElement.UnregisterCallback<ChangeEvent<StyleFont>, TextElement>(OnChange);
            textElement.UnregisterCallback<ChangeEvent<StyleFontDefinition>, TextElement>(OnChange);
            textElement.UnregisterCallback<ChangeEvent<StyleLength>, TextElement>(OnChange);
            textElement.UnregisterCallback<GeometryChangedEvent, TextElement>(OnChange);
            textElement.UnregisterCallback<DetachFromPanelEvent, TextElement>(OnDetachFromPanel);
        }

        /// <summary>
        /// Resize the text element when geometry is changed.
        /// </summary>
        private static void UpdateFontSize(TextElement textElement)
        {
            if (textElement.text == string.Empty) return;
            
            var textSize = textElement.MeasureTextSize(
                textElement.text, 
                textElement.contentRect.width, 
                VisualElement.MeasureMode.AtMost, 
                textElement.contentRect.height, 
                VisualElement.MeasureMode.AtMost
            );
            var baseFontSize = Mathf.Max(textElement.resolvedStyle.fontSize, 1);
            var heightDeterminedFontSize = Mathf.Abs(textElement.contentRect.height / textSize.y * baseFontSize);
            var widthDeterminedFontSize = Mathf.Abs(textElement.contentRect.width / textSize.x * baseFontSize);
            var targetFontSize = Mathf.FloorToInt(Math.Max(Mathf.Min(heightDeterminedFontSize, widthDeterminedFontSize), 1));
            
            if (Mathf.FloorToInt(textSize.y) == targetFontSize) return;
            textElement.style.fontSize = new StyleLength(new Length(targetFontSize));
        }

        /// <summary>
        /// Generic callback handler use to <see cref="UpdateFontSize"/>.
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        private static void OnChange<TEvent>(TEvent eventBase, TextElement textElement) where TEvent: EventBase<TEvent>, new()
        {
            // eventBase.StopPropagation();
            UpdateFontSize(textElement); 
        }
    }
}