namespace MVC.ViewModels
{
    public class PaginationInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public string PreviousPage { get; set; } = null!;
        public string NextPage { get; set; } = null!;
    }
}
