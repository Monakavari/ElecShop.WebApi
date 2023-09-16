namespace ElecShop.WebApi.Core.Services.Contracts
{
    public interface IMailSender
    {
        void Send(string to, string subject, string body);
    }
}