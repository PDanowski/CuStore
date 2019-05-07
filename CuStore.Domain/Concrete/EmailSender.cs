using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CuStore.Domain.Abstract;
using CuStore.Domain.Constants;
using CuStore.Domain.Entities;

namespace CuStore.Domain.Concrete
{

    public class EmailSender : IEmailSender
    {
        private readonly EmailServerConfiguration _emailConfig;

        public EmailSender(EmailServerConfiguration configuration)
        {
            _emailConfig = configuration;
        }

        public void ProcessOrder(Order order)
        {
            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = _emailConfig.UseSsl;
                client.Host = _emailConfig.ServerName;
                client.Port = _emailConfig.ServerPort;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(_emailConfig.Username, _emailConfig.Password);

                if (_emailConfig.WriteAsFile)
                {
                    client.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    client.PickupDirectoryLocation = _emailConfig.FileLocation;
                    client.EnableSsl = false;
                }

                string body = CreateEmailContent(order);

                MailMessage message = new MailMessage(
                    _emailConfig.MailFromAddress,
                    order.Cart.User.Email,
                    "New order: " + order.Id + "confirmed",
                    body);

                if (_emailConfig.WriteAsFile)
                {
                    message.BodyEncoding = Encoding.ASCII;
                }

                client.Send(message);
            }
        }

        private string CreateEmailContent(Order order)
        {
            StringBuilder body = new StringBuilder()
                .AppendLine("New order:" + order.Id)
                .AppendLine("--------------------------")
                .AppendLine("Items:");

            foreach (var item in order.Cart.CartItems)
            {
                var subtotal = item.Quantity * item.Product.Price;
                body.AppendFormat("{0} x {1} (value: {2:c})", item.Quantity, item.Product.Price, subtotal);
            }

            body.AppendFormat("Total value: {0:c}", order.Cart.GetValue())
                .AppendLine("--------------------------")
                .AppendFormat("Shipping method: {0} ({1:c})", order.ShippingMethod.Name, order.ShippingMethod.Price)
                .AppendLine("--------------------------")
                .AppendLine("Shipping address:");

            if (order.UseUserAddress)
            {
                body.AppendLine(order.Cart.User.FirstName + " " + order.Cart.User.LastName)
                    .AppendLine(order.Cart.User.PhoneNumber)
                    .AppendLine(order.Cart.User.UserAddress.Street)
                    .AppendLine(order.Cart.User.UserAddress.PostalCode + " " + order.Cart.User.UserAddress.City)
                    .AppendLine(order.Cart.User.UserAddress.Country);
            }
            else
            {
                body.AppendLine(order.ShippingAddress.FirstName + " " + order.ShippingAddress.LastName)
                    .AppendLine(order.ShippingAddress.Phone)
                    .AppendLine(order.ShippingAddress.Street)
                    .AppendLine(order.ShippingAddress.PostalCode + " " + order.ShippingAddress.City)
                    .AppendLine(order.ShippingAddress.Country);
            }

            return body.ToString();
        }
    }
}
