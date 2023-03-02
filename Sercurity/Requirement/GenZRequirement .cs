using Microsoft.AspNetCore.Authorization;

namespace App.Sercurity.Requirement
{
    public class GeZRequirement : IAuthorizationRequirement
    {
        public int FromYear{set;get;}

        public int ToYear{set;get;}

        public GeZRequirement(int _fromyear=1997, int _toyear = 2012)
        {
            FromYear =_fromyear;
            ToYear = _toyear;
        }
    }
}