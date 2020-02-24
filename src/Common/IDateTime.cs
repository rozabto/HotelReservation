using System;

namespace Common
{
    public interface IDateTime
    {
        public DateTime Now { get; }
        public DateTime MinDate { get; }
    }
}