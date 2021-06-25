using System.Collections.Generic;

namespace creditRecordEvents
{
    public class CreditsRecordRepository
    {
        private readonly Dictionary<string, IList<IEvent>> _inMemoryStream = new();

        public CreditsRecord Get(string creditId)
        {
            var creditRecord = new CreditsRecord(creditId);
            
            if (_inMemoryStream.ContainsKey(creditId))
            {
                foreach (var evnt in _inMemoryStream[creditId])
                {
                   creditRecord.AddEvent(evnt);
                }
            }
            return creditRecord;
        }

        public void Save(CreditsRecord creditsRecord)
        {
            _inMemoryStream[creditsRecord.CreditId] = creditsRecord.GetEvents();
        }
    }
}