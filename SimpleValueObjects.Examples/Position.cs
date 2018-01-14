using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleValueObjects.Examples
{
    public class Position : AutoValueObject<Position>
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    // the purpose of below code is to be copy-pased into documentation

    public class VisitedLocationsRegistry
    {
        private readonly HashSet<Position> _visitedLocations;

        public VisitedLocationsRegistry(IEnumerable<Position> visitedLocations)
        {
            _visitedLocations = visitedLocations.ToHashSet();
        }

        public bool WasLocationVisited(Position position)
        {
            return _visitedLocations.Contains(position);
        }

        public IEnumerable<Position> NotVisitedLocations(IEnumerable<Position> positions)
        {
            return positions.Except(_visitedLocations);
        }
    }

    public static class PositionEqualityExample
    {
        public static void EqualityExample()
        {
            var first = new Position(5, 5);
            var second = new Position(5, 5);

            Console.WriteLine(first == second); // True
            Console.WriteLine(first != second); // False
            Console.WriteLine(first.Equals(second)); // True
        }
    }
}