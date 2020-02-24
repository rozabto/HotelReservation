using System;
using Common;

namespace Infrastructure.Common
{
    public class UniversalDateTime : IDateTime
    {
        private static readonly DateTime MinimumDate = DateTime.Parse("1/1/1800 12:00:00 AM");

        public DateTime Now => DateTime.UtcNow;

        public DateTime MinDate => MinimumDate;
    }
}