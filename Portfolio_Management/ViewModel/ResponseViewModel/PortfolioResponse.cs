﻿namespace Portfolio_Management.ViewModel.ResponseViewModel
{
    public class PortfolioResponse
    {
        public string Stock { get; set; }
        public long TotalUnit { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal SoldAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal OverAllProfit { get; set; }
    }
}