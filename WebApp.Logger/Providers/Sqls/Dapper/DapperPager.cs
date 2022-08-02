namespace WebApp.Logger.Providers.Sqls
{
    public class DapperPager
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }

        public int Offset { get; set; }
        public int Next { get; set; }

        public DapperPager(int pageIndex = 0, int pageSize = 10)
        {
            PageIndex = pageIndex < 1 ? 1 : pageIndex;
            PageSize = pageSize < 1 ? 10 : pageSize;

            Next = pageSize;
            Offset = (PageIndex - 1) * Next;
        }
    }
}
