namespace project_API.Repositories;

public class ReviewRepository: IReviewRepository
{
    public Task<bool> AddReview(Review review)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateReview(Review review)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteReview(int reviewId)
    {
        throw new NotImplementedException();
    }

    public Task<Review> GetReviewById(int reviewId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Review>> GetReviewsByProductId(int productId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Review>> GetAllReviews()
    {
        throw new NotImplementedException();
    }

    public Task<double> GetAverageRating(int productId)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetTotalReviews(int productId)
    {
        throw new NotImplementedException();
    }
}