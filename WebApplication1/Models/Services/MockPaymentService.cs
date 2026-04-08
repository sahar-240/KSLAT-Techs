using WebApplication1.Models;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Services
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(Donation donation);
    }

    public class PaymentResult
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string TransactionId { get; set; } = string.Empty;
    }

    public class MockPaymentService : IPaymentService
    {
        private readonly ILogger<MockPaymentService> _logger;

        public MockPaymentService(ILogger<MockPaymentService> logger)
        {
            _logger = logger;
        }

        public async Task<PaymentResult> ProcessPaymentAsync(Donation donation)
        {
            try
            {
                await Task.Delay(1500);

                if (donation.Amount <= 0)
                {
                    return new PaymentResult
                    {
                        Success = false,
                        Message = "Invalid donation amount"
                    };
                }

                var random = new Random();
                bool isSuccessful = random.Next(0, 100) < 95;

                if (isSuccessful)
                {
                    string transactionId = "TXN_" + Guid.NewGuid().ToString().Substring(0, 12).ToUpper();
                    _logger.LogInformation($"Payment processed. ID: {transactionId}, Amount: £{donation.Amount}");

                    return new PaymentResult
                    {
                        Success = true,
                        Message = "Payment successful",
                        TransactionId = transactionId
                    };
                }
                else
                {
                    return new PaymentResult
                    {
                        Success = false,
                        Message = "Payment declined. Please try again."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Payment error: {ex.Message}");
                return new PaymentResult
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }
    }
}
