using System.Drawing;

namespace Drawing.classes
{
    internal static class Constants
    {
        public const float Radius = 14.5f;
        public const float Diameter = 2 * Radius;
        public const string FamilyName = "Times New Roman";
        public static readonly Font Font = new Font(FamilyName, Radius);
    }
}