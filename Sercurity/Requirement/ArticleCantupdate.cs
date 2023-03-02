using Microsoft.AspNetCore.Authorization;

namespace App.Sercurity.Requirement
{
    public class ArticleCantupdate : IAuthorizationRequirement
    {
        public int Year{set;get;}

        public int Month{set;get;}

        public int Date{set;get;}

        public ArticleCantupdate(int year=2023, int month=1, int date=1)
        {
            Year =year;
            Month = month;
            Date = date;
        }
        
   
    }
}