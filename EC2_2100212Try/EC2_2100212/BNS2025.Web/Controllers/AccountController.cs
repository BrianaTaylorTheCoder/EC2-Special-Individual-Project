using AutoMapper;
using BNS2025.ViewModels;
using BNS2025.Models;
using BNS2025.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace BNS2025.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Authorize(Roles = "Teller")]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AccountController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var accounts = await _unitOfWork.GenericRepository<Account>().GetAllAsync();
            var viewModelList = _mapper.Map<IList<AccountViewModel>>(accounts);

            foreach (var account in viewModelList)
            {
                var user = _unitOfWork.GenericRepository<ApplicationUser>().GetAllAsync( au =>au.Id == account.UserId.ToString().ToLower()).Result.FirstOrDefault();
                account.User = _mapper.Map<UserViewModel>(user);

                var currency = await _unitOfWork.GenericRepository<Currency>().GetByIdAsync((int)account.CurrencyId);
                account.Currency = _mapper.Map<CurrencyViewModel>(currency);
            }
            return View(viewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Currencies = new SelectList(await _unitOfWork.GenericRepository<Currency>().GetAllAsync(), "Id", "ShortName");
            ViewBag.Users = new SelectList(await _unitOfWork.GenericRepository<ApplicationUser>().GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountViewModel model)
        {
            var account = _mapper.Map<Account>(model);
            await _unitOfWork.GenericRepository<Account>().InsertAsync(account);
            await _unitOfWork.Save();

            TempData["SuccessMessage"] = "Account created successfully!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var account = _unitOfWork.GenericRepository<Account>().GetByIdAsync(id).Result;
            var viewModel = _mapper.Map<AccountViewModel>(account);

            ViewBag.Users = new SelectList(await _unitOfWork.GenericRepository<ApplicationUser>().GetAllAsync(), "Id", "Name");
            ViewBag.Currencies = new SelectList(await _unitOfWork.GenericRepository<Currency>().GetAllAsync(), "Id", "ShortName");

            return View(viewModel);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AccountViewModel model)
        {
            var account = _mapper.Map<Account>(model);
            await _unitOfWork.GenericRepository<Account>().UpdateAsync(account);
            await _unitOfWork.Save();

            TempData["SuccessMessage"] = "Account updated successfully!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var account = await _unitOfWork.GenericRepository<Account>().GetAllAsync(q => q.Id == id);
            var viewModel = _mapper.Map<AccountViewModel>(account);

            return View(account);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, AccountViewModel model)
        {
            await _unitOfWork.GenericRepository<Account>().DeleteAsync(id);
            await _unitOfWork.Save();

            TempData["SuccessMessage"] = "Account deletion successfully!";

            return RedirectToAction("Index");

        }

        [HttpGet]
        public async Task<IActionResult> ViewTransactions()
        {
            var transactions =  await _unitOfWork.GenericRepository<Transaction>().GetAllAsync();
            var transactionsViewModelList = _mapper.Map<IList<TransactionViewModel>>(transactions);

            foreach (TransactionViewModel transaction in transactionsViewModelList)
            {
                var account = _unitOfWork.GenericRepository<Account>().GetAllAsync(a => a.AccountNumber == transaction.AccountNumber).Result.FirstOrDefault();
                transaction.UserId = account.UserId.ToString();
                transaction.CardNumber = account.CardNumber;
                
                var user = _unitOfWork.GenericRepository<ApplicationUser>().GetAllAsync(u => u.Id == transaction.UserId).Result.FirstOrDefault();
                transaction.User = _mapper.Map<UserViewModel>(user);
            }
            return View(transactionsViewModelList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTransaction()
        {
            ViewBag.Users = new SelectList(await _unitOfWork.GenericRepository<ApplicationUser>().GetAllAsync(), "Id", "Name");
            //ViewBag.Currencies = new SelectList(await _unitOfWork.GenericRepository<Currency>().GetAll(), "Id", "ShortName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(TransactionViewModel transactionViewModel)
        {
            bool isFound = false;
            TransactionType transactionType = TransactionType.Withdrawal;

            var account = _unitOfWork.GenericRepository<Account>().GetAllAsync(a => a.CardNumber == transactionViewModel.CardNumber &&
                a.CardSecurityCode == transactionViewModel.CardSecurityCode && a.CardExpirationDate == transactionViewModel.CardExpirationDate).Result.FirstOrDefault();

            if (account == null)
            {
                TempData["ErrorMessage"] = "Card details invalid!";
                return View(transactionViewModel);
            }
            else
            {
                isFound = true;
                if (transactionViewModel.TransactionType == TransactionType.Withdrawal)
                {
                    if(account.Balance < transactionViewModel.Amount)
                    {
                        TempData["ErrorMessage"] = "Insuccifient funds!";
                        return View(transactionViewModel);
                    }
                    account.Balance -= transactionViewModel.Amount;
                }
                else
                {
                    account.Balance += transactionViewModel.Amount;
                    transactionType = TransactionType.Deposit;
                }
                await _unitOfWork.GenericRepository<Account>().UpdateAsync(account);
                await _unitOfWork.Save();

                Transaction transaction = new Transaction
                {
                    AccountNumber = account.AccountNumber,
                    Amount = transactionViewModel.Amount,
                    TransactionDate = DateTime.UtcNow,
                    Source = "BNS.WebApp",
                    TransactionType = transactionType,
                    UserId = transactionViewModel.UserId
                };
                await _unitOfWork.GenericRepository<Transaction>().InsertAsync(transaction);
                await _unitOfWork.Save();

                TempData["SuccessMessage"] = "Transaction completed successfully!";
            }
            return RedirectToAction("ViewTransactions");
        }
       
    }
}
