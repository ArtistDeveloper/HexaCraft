using UnityEngine;
using UnityEditor;
using System.Linq.Expressions;

namespace HexaCraft
{
    public static class HexaGUILayout
    {
        public static Material MaterialField(GUIContent gc, Material value)
        {
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(gc, GUILayout.Width(140));
                Material ret = EditorGUILayout.ObjectField(value, typeof(Material), false) as Material;

                return ret;
            }
        }

        public static float FreeSlider(string content, float value, float min, float max)
        {
            GUIContent gc = new GUIContent(content);
            return FreeSlider(gc, value, min, max);
        }

        public static float FreeSlider(GUIContent content, float value, float min, float max)
        {
            const float PAD = 4f;
            const float SLIDER_HEIGHT = 16f;
            const float MIN_LABEL_WIDTH = 0f;
            const float MAX_LABEL_WIDTH = 128f;
            const float MIN_FIELD_WIDTH = 48f;

            Rect previousRect = GUILayoutUtility.GetRect(GUIContent.none, GUI.skin.horizontalSlider, GUILayout.Height(SLIDER_HEIGHT));
            float y = previousRect.y;
            float availableWidth = previousRect.width;

            float labelWidth = content != null ? Mathf.Max(MIN_LABEL_WIDTH, Mathf.Min(GUI.skin.label.CalcSize(content).x + PAD, MAX_LABEL_WIDTH)) : 0f;
            float remaining = availableWidth - labelWidth;
            float sliderWidth = remaining - (MIN_FIELD_WIDTH + PAD);
            float floatWidth = MIN_FIELD_WIDTH;

            Rect labelRect = new Rect(PAD, y + 2f, labelWidth, SLIDER_HEIGHT);
            Rect sliderRect = new Rect(labelRect.x + labelWidth, y + 1f, sliderWidth, SLIDER_HEIGHT);
            Rect floatRect = new Rect(sliderRect.x + sliderRect.width + PAD, y + 1f, floatWidth, SLIDER_HEIGHT);

            if (content != null)
                GUI.Label(labelRect, content);

            EditorGUI.BeginChangeCheck();

            int controlID = GUIUtility.GetControlID(FocusType.Passive, sliderRect);
            float tmp = value;
            tmp = GUI.Slider(sliderRect, tmp, 0f, min, max, GUI.skin.horizontalSlider, (!EditorGUI.showMixedValue) ? GUI.skin.horizontalSliderThumb : "SliderMixed", true, controlID);

            if (EditorGUI.EndChangeCheck())
                value = Event.current.control ? 0.1f * Mathf.Round(tmp / 0.1f) : tmp;

            value = EditorGUI.FloatField(floatRect, value);

            return value;
        }
    }
}