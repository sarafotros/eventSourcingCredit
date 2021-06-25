using System;

namespace creditRecordEvents
{
    public interface IEvent {}

    public record CreditAdded(string CreditId, int CreditAmount, DateTime DateTime) : IEvent;
    
    public record CreditSpent(string CreditId, int CreditAmount, DateTime DateTime) : IEvent;
    
    public record CreditExpired(string CreditId, int CreditAmount, string Reason, DateTime DateTime) : IEvent;
}