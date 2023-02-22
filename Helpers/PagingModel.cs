namespace Boom.Heplers
{
    public class PagingModel
    {
        public int currentpage{set;get;}

        public int countpages{set;get;}

        public Func<int?, string>? generateUrl{set;get;}
    }
    
}