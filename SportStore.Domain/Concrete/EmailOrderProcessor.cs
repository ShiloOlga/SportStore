using SportStore.Domain.Abstract;
using SportStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SportStore.Domain.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "orders@example.com";
        public string MailFromAddress = "sport-store@example.com";
        public bool UseSsl = true;
        public string Username = "";
        public string Password = "";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = "SportStoreMail";
    }

    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings _settings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            _settings = settings;
        }

        public void ProcessOrder(Cart cart, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = _settings.UseSsl;
                smtpClient.Host = _settings.ServerName;
                smtpClient.Port = _settings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(_settings.Username, _settings.Password);

                if (_settings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    var targetDir = Path.Combine(Environment.GetEnvironmentVariable("localappdata"), _settings.FileLocation);
                    Directory.CreateDirectory(targetDir);
                    smtpClient.PickupDirectoryLocation = targetDir;
                    smtpClient.EnableSsl = false;
                }

                var body = new StringBuilder()
                    .AppendLine("A new order has been submitted")
                    .AppendLine("---")
                    .AppendLine("items:");

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    body.AppendLine($"{line.Quantity} x {line.Product.Name} subtotal: {subtotal:2:c}");
                }

                body.AppendLine($"Total order value: {cart.ComputeTotalValue().ToString("c")}")
                    .AppendLine("---")
                    .AppendLine("Ship to:")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1)
                    .AppendLine(shippingDetails.Line2 ?? string.Empty)
                    .AppendLine(shippingDetails.Line3 ?? string.Empty)
                    .AppendLine(shippingDetails.City)
                    .AppendLine(shippingDetails.State ?? string.Empty)
                    .AppendLine(shippingDetails.Country)
                    .AppendLine(shippingDetails.Zip)
                    .AppendLine("---")
                    .AppendFormat("Gift wrap: {0}", shippingDetails.GiftWrap ? "Yes" : "No");

                var message = new MailMessage(_settings.MailFromAddress, _settings.MailToAddress, "New order", body.ToString());
                if (_settings.WriteAsFile)
                {
                    message.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(message);
            }
        }
    }
}
