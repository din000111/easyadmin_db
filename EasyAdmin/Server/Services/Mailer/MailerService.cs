using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using System.Collections.Generic;
using EasyAdmin.Shared.Common;
using System.IO;
using System.Threading.Tasks;

namespace EasyAdmin.Server.Services.Mailer
{
    public interface IMailerService
    {
        Task SendMailAsync(List<User> addresses, string subject, string messageText);
        void NotifyVmRequested(List<User> addresses, Vm vm);
        void NotifyVmAudit(List<User> addresses, Vm vm, AuditActionTypes.EnumAuditActionTypes auditType);
        void Dispose();
    }

    public class MailerService : IMailerService
    {
        private readonly bool _isActive = false;
        private readonly SmtpClient client;
        private readonly Mail _config;
        
        public MailerService(IOptions<Mail> config)
        {
            _config = config.Value;
            client = new SmtpClient();            
            client.ServerCertificateValidationCallback = (s, c, h, e) => true;            

            if (!string.IsNullOrEmpty(_config.Address) && _config.Port != null)
            {
                client.Connect(_config.Address, (int)_config.Port, false);
            }
            if (client.IsConnected)
            {            
                if ((bool)_config.AuthorizationRequired 
                    && !string.IsNullOrEmpty(_config.Username)
                    && !string.IsNullOrEmpty(_config.Password))
                {                                                
                    client.Authenticate(_config.Username, _config.Password);
                    if (client.IsAuthenticated) {
                        _isActive = true;
                    }  
                }
                else
                {
                    _isActive = true;
                }                
            }
        }
        public void Dispose()
        {
            client.Disconnect(true);
            client.Dispose();
        }

        public void NotifyVmAudit(List<User> addresses, Vm vm, AuditActionTypes.EnumAuditActionTypes auditType)
        {
            string aftermath = "";
            switch (auditType)
            {
                case AuditActionTypes.EnumAuditActionTypes.Archive:
                    aftermath = "Если не продлить машину, она будет заархивирована";
                    break;
                case AuditActionTypes.EnumAuditActionTypes.Shutdown:
                    aftermath = "Если не продлить машину, она будет выключена";
                    break;
            }
            var messageText = BuildMessageText("./Services/Mailer/Templates/VmAudit.html",
                new[] {
                    vm.Name,
                    aftermath
                }
            );
            SendMailAsync(addresses, $"Аудит виртуальной машины {vm.Name}", messageText);
        }

        public void NotifyVmRequested(List<User> addresses, Vm vm)
        {
            var messageText = BuildMessageText("./Services/Mailer/Templates/VmRequested.html",
                new[] { 
                    vm.FullName, 
                    vm.Cpu.ToString(),
                    vm.MemoryGb.ToString(),
                    vm.HddSize.ToString(),
                    vm.AdminId,
                    vm.ManagerId,
                    vm.OwnerId,
                    vm.Project,
                    vm.Services,
                    vm.Domain
                } 
            );
            SendMailAsync(addresses, $"Запрошена виртуальная машина {vm.FullName}", messageText);
        }

        public async Task SendMailAsync(List<User> addresses, string subject, string messageText)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Управление инфраструктурой", _config.Username));
            foreach (var address in addresses)
            {
                message.To.Add(new MailboxAddress(address.Email));
            }
            
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = messageText
            };
            if (_isActive)
            {
                client.Send(message);
                client.Disconnect(true);
            }
        }

        private string BuildMessageText(string templatePath, string[] args)
        {
            var builder = new BodyBuilder();
            using (StreamReader sourceReader = File.OpenText(templatePath))
            {
                builder.HtmlBody = sourceReader.ReadToEnd();
            }
            return string.Format(builder.HtmlBody, args);
        }
    }
}
