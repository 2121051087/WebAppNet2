namespace WebAppNet2.Models.DTO
{
    public class DashBoardModelView
    {
        public List<double>? chartDataetTotalAmountByMonth { get; set; }

        public List<GetTotalAmountByWeekVM>? chartDataGetTotalAmountByWeek { get; set; }


        public int? CountConfirmedOrders { get; set; }

        public int? CountSoldProducts { get; set; }

        public int? CountCategories { get; set; }

        public int? CountProduct { get; set; }

        public int? CountUsers { get; set; }
    }
}
