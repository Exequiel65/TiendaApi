namespace TiendaApi.Helpers
{
    public class Pager<T> where T : class
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public IEnumerable<T> Registers { get; set; }

        public Pager (IEnumerable<T> registers, int total, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
            Registers = registers;
        }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(Total / (double)PageSize);
            }
        }
        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }
        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
    }
}
