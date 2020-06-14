using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psychotest.Model
{
    public class Shape
    {
        public Shape(int shapeNum)
        {
            // Set shapeType based on shapeNum
            foreach (var key in ShapesConfig.Keys)
                if (ShapesConfig[key].ShapeNum == shapeNum)
                {
                    shapeType = key;
                }
        }

        public ShapeType shapeType 
        {
            get;
            private set;
        }

        public string IconPath
        {
            get => ShapesConfig[shapeType].IconPath;
        }

        public int ShapeNum
        {
            get => ShapesConfig[shapeType].ShapeNum;
        }

        public static Dictionary<ShapeType, ShapeInfo> ShapesConfig = new Dictionary<ShapeType, ShapeInfo>
        {
            {ShapeType.Arrow, new ShapeInfo(@"..\images\arrow.png", 1) },
            {ShapeType.Circle, new ShapeInfo(@"..\images\circle.png", 2) },
            {ShapeType.Square, new ShapeInfo(@"..\images\square.png", 3) },
            {ShapeType.Star, new ShapeInfo(@"..\images\star.png", 4) },
            {ShapeType.Triangle, new ShapeInfo(@"..\images\triangle.png", 5) },
            {ShapeType.Triangle_obr, new ShapeInfo(@"..\images\triangle_obr.png", 6) },
            {ShapeType.Hexagon, new ShapeInfo(@"..\images\hexagon.png", 7) },
            {ShapeType.Pentagon, new ShapeInfo(@"..\images\pentagon.png", 8) },
            {ShapeType.Heart, new ShapeInfo(@"..\images\heart.png", 9) },
            {ShapeType.Rhombus, new ShapeInfo(@"..\images\rhombus.png", 10) },
        };
    }
}
