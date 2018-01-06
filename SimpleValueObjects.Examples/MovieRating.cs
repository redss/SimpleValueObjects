using System;

namespace SimpleValueObjects.Examples
{
    public class MovieRating : WrapperComparableObject<MovieRating, int>
    {
        public MovieRating(int stars)
            : base(stars)
        {
            if (stars < 1 || stars > 5)
            {
                throw new ArgumentException(
                    $"UserRating should be between 1 and 5 stars, but got {stars} stars.");
            }
        }

        public static readonly MovieRating Lowest = new MovieRating(1);

        public static readonly MovieRating Highest = new MovieRating(5);

        public override string ToString() => new string('★', count: Value);
    }
}