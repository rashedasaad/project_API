namespace project_API.Repositories.Interface;

public interface IReviewRepository
{
    
    Task<bool> AddReview(Review review);
    Task<bool> UpdateReview(Review review);
    Task<bool> DeleteReview(int reviewId);
    Task<Review> GetReviewById(int reviewId);
    Task<IEnumerable<Review>> GetReviewsByProductId(int productId);
    Task<IEnumerable<Review>> GetAllReviews();
    Task<double> GetAverageRating(int productId);
    Task<int> GetTotalReviews(int productId);
    
}