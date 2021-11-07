using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.script
{
    public static class AttributeConstants
    {
        public const int DAMAGE_AMOUNT_MIN = 1;
        public const float DAMAGE_HORIZONTAL_MULTIPLIER_MIN = 5.0f;
        public const float DAMAGE_VERTICAL_MULTIPLIER_MIN = 5.0f;
    }

    public enum GroundType
    {
        ground_default,
        ground_slide,
        ground_water,
        ground_grass,
        ground_sand,
        ground_stone,
        ground_wood,
        ground_mud,
        ground_metal,
        ground_foliage,
    }
}
