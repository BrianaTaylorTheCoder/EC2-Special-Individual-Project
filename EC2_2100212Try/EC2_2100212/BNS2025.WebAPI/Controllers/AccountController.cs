using AutoMapper;
using BNS2025.Models;
using BNS2025.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BNS2025.ViewModels;

namespace BNS2025.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        // PUT api/<AccountController>/5
        [HttpPut("Deposit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> Deposit([FromForm] TransactionViewModel transactionViewModel)
        {
            bool isFound = false;

            var account = _unitOfWork.GenericRepository<Account>().GetAllAsync(a => a.CardNumber == transactionViewModel.CardNumber &&
                a.CardSecurityCode == transactionViewModel.CardSecurityCode &&
                a.CardExpirationDate == transactionViewModel.CardExpirationDate).Result.FirstOrDefault();

            if (account == null)
            {
                return BadRequest("Card Details Invalid");
            }
            else
            {
                isFound = true;
                account.Balance += transactionViewModel.Amount;

                await _unitOfWork.GenericRepository<Account>().UpdateAsync(account);
                await _unitOfWork.Save();

                Transaction transaction = new Transaction
                {
                    AccountNumber = account.AccountNumber,
                    Amount = transactionViewModel.Amount,
                    TransactionDate = DateTime.UtcNow,
                    Source = transactionViewModel.Source,
                    TransactionType = transactionViewModel.TransactionType,
                    UserId = transactionViewModel.UserId
                };
                await _unitOfWork.GenericRepository<Transaction>().InsertAsync(transaction);
                await _unitOfWork.Save();

            }

            return Ok(isFound);
        }


        [HttpPut("Withdrawal")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> Withdrawal([FromForm] TransactionViewModel transactionViewModel)
        {
            bool isFound = false;

            var account = _unitOfWork.GenericRepository<Account>().GetAllAsync(a => a.CardNumber == transactionViewModel.CardNumber &&
                a.CardSecurityCode == transactionViewModel.CardSecurityCode &&
                a.CardExpirationDate == transactionViewModel.CardExpirationDate).Result.FirstOrDefault();

            if (account == null)
            {
                return NotFound("Card Details Invalid");
            }
            else
            {

                isFound = true;
                if (account.Balance < transactionViewModel.Amount)
                {
                    return BadRequest("Insufficient Funds");
                }
                account.Balance -= transactionViewModel.Amount;

                await _unitOfWork.GenericRepository<Account>().UpdateAsync(account);
                await _unitOfWork.Save();

                Transaction transaction = new Transaction
                {
                    AccountNumber = account.AccountNumber,
                    Amount = transactionViewModel.Amount,
                    TransactionDate = DateTime.UtcNow,
                    Source = transactionViewModel.Source,
                    TransactionType = TransactionType.Withdrawal,
                    UserId = transactionViewModel.UserId
                };
                await _unitOfWork.GenericRepository<Transaction>().InsertAsync(transaction);
                await _unitOfWork.Save();

            }

            return Ok(isFound);
        }
    }
}
