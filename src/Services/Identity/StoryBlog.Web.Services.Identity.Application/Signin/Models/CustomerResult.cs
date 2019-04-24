using StoryBlog.Web.Services.Identity.Persistence.Models;

namespace StoryBlog.Web.Services.Identity.Application.Signin.Models
{
    public class CustomerResult
    {
        public bool Success
        {
            get;
            private set;
        }

        public bool IsNotAllowed
        {
            get;
            private set;
        }

        public bool IsLockedOut
        {
            get;
            private set;
        }

        public bool RequiresTwoFactor
        {
            get;
            protected set;
        }

        public Customer Customer
        {
            get;
        }

        private CustomerResult(Customer customer)
        {
            Customer = customer;
        }

        public static CustomerResult Succeeded(Customer customer) => new CustomerResult(customer)
        {
            Success = true
        };

        public static CustomerResult NotAllowed(Customer customer) => new CustomerResult(customer)
        {
            IsNotAllowed = true
        };

        public static CustomerResult LockedOut(Customer customer) => new CustomerResult(customer)
        {
            IsLockedOut = true
        };

        public static CustomerResult TwoFactorRequired(Customer customer) => new CustomerResult(customer)
        {
            RequiresTwoFactor = true
        };
    }
}