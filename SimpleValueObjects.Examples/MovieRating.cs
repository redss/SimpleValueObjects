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
                    $"UserRating should be between 1 and 5 stars, but found {stars}.");
            }
        }

        public override string ToString() => new string('★', count: Value);
    }
}