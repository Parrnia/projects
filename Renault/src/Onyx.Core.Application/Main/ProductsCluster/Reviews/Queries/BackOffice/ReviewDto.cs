using AutoMapper;
using Onyx.Application.Common.Mappings;
using Onyx.Domain.Entities.ProductsCluster;

namespace Onyx.Application.Main.ProductsCluster.Reviews.Queries.BackOffice;
public class ReviewDto : IMapFrom<Review>
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; } = null!;
    public string AuthorName { get; set; } = null!;
    public string ProductName { get; set; } = null!;
    public int ProductId { get; set; }
    public Guid CustomerId { get; set; }
    public bool IsActive { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Review, ReviewDto>()
            .ForMember(d => d.ProductName, opt => opt.MapFrom(s => s.Product.LocalizedName));
    }
}
