using Microsoft.EntityFrameworkCore;
namespace creditRecordEvents
{
    public class CreditRecord
    {
        public string CreditId { get; set; }
        public int Added { get; set; }
        public string Spent { get; set; }
    }
    
    public class CreditRecordDbContext : DbContext
    {
        
    }
}