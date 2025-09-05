using MassTransit;
using Pcf.Administration.Core.Domain;
using Pcf.ReceivingFromPartner.Core.Abstractions.Gateways;
using Pcf.ReceivingFromPartner.Core.Domain;
using Pcf.ReceivingFromPartner.Integration.Dto;
using Pcf.Shared.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace Pcf.ReceivingFromPartner.Integration
{
    public class GivingPromoCodeToCustomerGateway
        : IGivingPromoCodeToCustomerGateway
    {
        //private readonly HttpClient _httpClient;
        private readonly IBusControl _busControl;
        public GivingPromoCodeToCustomerGateway(IBusControl busControl)
        {
           // _httpClient = httpClient;
            _busControl = busControl;
        }

        public async Task GivePromoCodeToCustomer(PromoCode promoCode)
        {
            var dto = new GivePromoCodeToCustomerDto()
            {
                PartnerId = promoCode.Partner.Id,
                BeginDate = promoCode.BeginDate.ToShortDateString(),
                EndDate = promoCode.EndDate.ToShortDateString(),
                PreferenceId = promoCode.PreferenceId,
                PromoCode = promoCode.Code,
                ServiceInfo = promoCode.ServiceInfo,
                PartnerManagerId = promoCode.PartnerManagerId
            };

            //            var response = await _httpClient.PostAsJsonAsync("api/v1/promocodes", dto);

            await _busControl.Publish(new MessageDto { Content = dto });
           
        }
    }
}