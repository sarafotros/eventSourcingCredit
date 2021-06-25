using System;
using System.Collections.Generic;

namespace creditRecordEvents
{
    internal class CurrentCredit
    { 
        public int Remaining { get; set; }
    }
    
    public class CreditsRecord
    {

        public string  CreditId { get; }
        private readonly IList<IEvent> _events = new List<IEvent>();
        
        //Projection
        private readonly CurrentCredit _currentCredit = new CurrentCredit();

        public CreditsRecord(string creditId)
        {
            CreditId = creditId;
        }

        
        public void AddCredit(int amount)
        {
            AddEvent(new CreditAdded(CreditId, amount, DateTime.UtcNow));
        }

        
        public void SpendCredit(int amount)
        {
            if (amount > _currentCredit.Remaining)
            {
                throw new InvalidDomainException("Sorry... You don't have enough credit");
            }
            
            AddEvent(new CreditSpent(CreditId, amount, DateTime.UtcNow));
        }

        public void ExpireCredit(int amount, string reason)
        {
            if (_currentCredit.Remaining + amount < 0)
            {
                throw new InvalidDomainException("Credit cannot be negative");
            }
            
            AddEvent(new CreditExpired(CreditId, amount, reason, DateTime.UtcNow));
        }
        
        private void Apply(CreditAdded evnt)
        {
            _currentCredit.Remaining += evnt.CreditAmount;
        }
        
        private void Apply(CreditSpent evnt)
        {
            _currentCredit.Remaining -= evnt.CreditAmount;
        }
        
        private void Apply(CreditExpired evnt)
        {
            _currentCredit.Remaining -= evnt.CreditAmount;
        }

        public IList<IEvent> GetEvents()
        {
            return _events;
        }
        
        public void AddEvent(IEvent evnt)
        {
            switch (evnt)
            {
                case CreditAdded creditAdded:
                    Apply(creditAdded);
                    break;
                case CreditSpent creditSpent:
                    Apply(creditSpent);
                    break;
                case CreditExpired creditExpired:
                    Apply(creditExpired);
                    break;
                default:
                    throw new InvalidOperationException("Unsupported Event.");
            }
            _events.Add(evnt);
        }

       

        public int GetRemainingCredit()
        {
            return _currentCredit.Remaining;
        }
        
    }

    public class InvalidDomainException : Exception
    {
        public InvalidDomainException(string message) : base(message)
        {

        }
    }
}