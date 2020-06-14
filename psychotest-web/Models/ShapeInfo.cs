using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psychotest.Model
{
    public class ShapeInfo
    {
        public string IconPath { get; private set; }

        public int ShapeNum { get; private set; }

        public ShapeInfo(string path, int num)
        {
            IconPath = path;
            ShapeNum = num;
        }
    }
}
