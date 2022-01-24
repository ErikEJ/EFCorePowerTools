using System.Threading.Tasks;

namespace BaseClasses
{
    public interface IRatingConfig
    {
        int RatingRequests { get; set; }

        Task SaveAsync();
    }
}
