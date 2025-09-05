using System;
using System.Net.Http;
using System.Threading.Tasks;
using MassTransit;
using Pcf.Administration.Core.Domain;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.Shared.Models;

namespace Pcf.ReceivingFromPartner.Integration
{
    public class AdministrationGateway
        : IAdministrationGateway
    {
        
        private readonly IBusControl _busControl;
        public AdministrationGateway(IBusControl busControl)
        {

            _busControl = busControl;
        }

        public async Task NotifyAdminAboutPartnerManagerPromoCode(Guid partnerManagerId)
        {
            //var response = await _httpClient.PostAsync($"api/v1/employees/{partnerManagerId}/appliedPromocodes",
            //    new StringContent(string.Empty));
            //response.EnsureSuccessStatusCode();
            await _busControl.Publish(new MessageDto { Content = $"Promocod {partnerManagerId} was applied" });
           
        }
    }
}