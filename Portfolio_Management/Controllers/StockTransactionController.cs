﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio_Management.Dto;
using Portfolio_Management.Extension;
using Portfolio_Management.Infrastructure.Enum;
using Portfolio_Management.Repository.Interface;
using Portfolio_Management.Services.Interface;
using Portfolio_Management.ViewModel;

namespace Portfolio_Management.Controllers
{
    [ApiController]
    public class StockTransactionController : ControllerBase
    {
        private readonly IStockTransactionRepository _transactionRepository;
        private readonly IStockTransactionService _transactionService;
        private readonly IStockRepository _stockRepository;

        public StockTransactionController(IStockTransactionRepository transactionRepository,
            IStockTransactionService transactionService, IStockRepository stockRepository)
        {
            _transactionRepository = transactionRepository;
            _transactionService = transactionService;
            _stockRepository = stockRepository;
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var transactions = await _transactionRepository.GetQueryable().OrderByDescending(x => x.TransactionDate)
                    .Select(y => new
                    {
                        company = y.Stock.StockName,
                        Quantity = y.Quantity,
                        TransactionType = (y.TransactionType == TransactionType.Buy) ? "BUY" : "SELL",
                        TransactionDate = y.TransactionDate.ToShortDateString(),
                    }).ToListAsync();
                return this.SendSuccess("transactions", transactions);
            }
            catch (Exception e)
            {
                return this.SendError(e.Message);
            }
        }

        [HttpPost("AddTransaction")]
        public async Task<IActionResult> New(StockTransactionVm viewModel)
        {
            try
            {
                var stock = await _stockRepository.FindOrThrowAsync(viewModel.StockId);

                if (viewModel.TransactionType == TransactionType.Sell)

                {
                    stock.ClosingRate += stock.ClosingRate * 10 / 100;
                    if (stock.ClosingRate > (decimal?)viewModel.Price)
                    {
                        throw new Exception($"Max is {stock.ClosingRate}");
                    }
                }

                var dto = new StockTransactionDto(stock, viewModel.Quantity, viewModel.TransactionType, viewModel.Price,
                    viewModel.TransactionDate);
                await _transactionService.RecordStockTransaction(dto);
                if (dto.TransactionType == TransactionType.Buy)
                {
                    return this.SendSuccess($"{dto.Stock.Prefix} Added {dto.Quantity} at {dto.Price}");
                }

                return this.SendSuccess($"{dto.Stock.Prefix} Sold {dto.Quantity} at {dto.Price}");
            }
            catch (Exception e)
            {
                return this.SendError(e.Message);
            }
        }
        
    }
}