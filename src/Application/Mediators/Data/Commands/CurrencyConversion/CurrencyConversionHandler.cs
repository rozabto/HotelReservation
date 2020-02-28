using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Data.Commands.CurrencyConversion
{
    public class CurrencyConversionHandler : IRequestHandler<CurrencyConversionCommand>
    {
        private readonly ICurrencyConversionService _currencyConversion;

        public CurrencyConversionHandler(ICurrencyConversionService currencyConversion)
        {
            _currencyConversion = currencyConversion ?? throw new ArgumentNullException(nameof(currencyConversion));
        }

        public async Task<Unit> Handle(CurrencyConversionCommand request, CancellationToken cancellationToken)
        {
            await _currencyConversion.GetLatestCountryCodes();
            return Unit.Value;
        }
    }
}
