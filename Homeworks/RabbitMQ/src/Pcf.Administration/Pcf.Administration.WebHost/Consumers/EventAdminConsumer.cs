using MassTransit;
using Pcf.Administration.Core.Abstractions.Repositories;
using Pcf.Administration.Core.Domain.Administration;
using Pcf.Shared.Models;
using System;
using System.Threading.Tasks;

namespace Pcf.Administration.WebHost.Consumers
{
    
     public class EventAdminConsumer : IConsumer<MessageDto>
    {
        
        private readonly IRepository<Employee> _employeeRepository;

        public EventAdminConsumer(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public async Task Consume(ConsumeContext<MessageDto> context)
        {
            if (context.Message?.Content == null || context.Message?.Content.GetType()!=typeof(string)) return;

            var employee = await _employeeRepository.GetByIdAsync(Guid.Parse(context.Message.Content.ToString()));

            if (employee == null)
                return ;

            employee.AppliedPromocodesCount++;

            await _employeeRepository.UpdateAsync(employee);

            
        }
    }

}
