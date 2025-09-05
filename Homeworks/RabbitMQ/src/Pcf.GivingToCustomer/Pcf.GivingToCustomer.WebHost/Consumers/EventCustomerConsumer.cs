using MassTransit;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.WebHost.Mappers;
using Pcf.GivingToCustomer.WebHost.Models;
using Pcf.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.Shared.Services
{
    public class EventCustomerConsumer : IConsumer<MessageDto>
    {
        private readonly IRepository<PromoCode> _promoCodesRepository;
        private readonly IRepository<Preference> _preferencesRepository;
        private readonly IRepository<Customer> _customersRepository;

        public EventCustomerConsumer(IRepository<PromoCode> promoCodesRepository,
            IRepository<Preference> preferencesRepository, IRepository<Customer> customersRepository)
        {
            _promoCodesRepository = promoCodesRepository;
            _preferencesRepository = preferencesRepository;
            _customersRepository = customersRepository;
        }
        public async Task Consume(ConsumeContext<MessageDto> context)
        {
            if (context.Message.Content is GivePromoCodeRequest request)
            {

                //Получаем предпочтение по имени
                var preference = await _preferencesRepository.GetByIdAsync(request.PreferenceId);

                if (preference == null)
                {
                    return;
                }

                //  Получаем клиентов с этим предпочтением:
                var customers = await _customersRepository
                    .GetWhere(d => d.Preferences.Any(x =>
                        x.Preference.Id == preference.Id));

                PromoCode promoCode = PromoCodeMapper.MapFromModel(request, preference, customers);

                await _promoCodesRepository.AddAsync(promoCode);

             
            }
        }
    }
}
