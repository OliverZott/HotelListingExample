namespace HotelListingExample.Models
{
    public class RequestParams
    {
        private const int maxPageSize = 4;

        private int _pageSize = 2;


        public int PageNumber { get; set; } = 1;    // Default value!

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }


    }
}
