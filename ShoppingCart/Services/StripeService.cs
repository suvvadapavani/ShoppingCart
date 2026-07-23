using ShoppingCart.Services.ServiceInterfaces;

namespace ShoppingCart.Services
{
    public class StripeService:IPaymentService
    {
        public string Pay(decimal amount)
        {
            return $"paid {amount} using stripe";
        }

    }
}
