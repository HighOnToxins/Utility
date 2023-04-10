using System.Diagnostics.CodeAnalysis;

namespace Search;

public static class Search
{

    public static bool TryBreadthSearch<T>(T startingValue, Func<T, bool> terminateCondition, [NotNullWhen(true)] out T? result) where T : ISearchState
    {
        if(terminateCondition.Invoke(startingValue))
        {
            result = startingValue;
            return true;
        }

        Queue<T> frontier = new();
        frontier.Enqueue(startingValue);

        HashSet<T> discovered = new() { startingValue };

        while(frontier.Count > 0)
        {
            T current = frontier.Dequeue();

            IEnumerable<T> options = current.GetSearchOptions()
                .OfType<T>()
                .Where(t => !discovered.Contains(t)));

            foreach(T option in options)
            {
                if(terminateCondition.Invoke(option))
                {
                    result = option;
                    return true;
                }

                discovered.Add(option);
                frontier.Enqueue(option);
            }
        }

        result = default;
        return false;
    }

    public static bool TryWeightedBreadthSearch<T>(T startingValue, Func<T, double> heuristic, [NotNullWhen(true)] out T? result) where T : IWeightedSearchState
    {
        double startHeurisitc = heuristic.Invoke(startingValue);

        if(startHeurisitc <= 0)
        {
            result = startingValue;
            return true;
        }

        PriorityQueue<T, double> frontier = new();
        frontier.Enqueue(startingValue, startHeurisitc);

        Dictionary<T, double> discovered = new() {
            { startingValue, heuristic.Invoke(startingValue) }
        };

        while(frontier.Count > 0)
        {
            if(!frontier.TryDequeue(out T? current, out double currentTotalpriority) || !discovered.TryGetValue(current, out double currentHeuristic) || current == null)
            {
                break;
            }

            double currentPriority = currentTotalpriority - currentHeuristic;

            IEnumerable<(T value, double priority)> options = current.GetSearchOptions()
                .OfType<(T value, double)>()
                .Where(v => !discovered.ContainsKey(v.value));

            foreach((T option, double optionPriorityDifference) in options)
            {
                double optionHeuristic = heuristic.Invoke(option);

                if(optionHeuristic <= 0)
                {
                    result = option;
                    return true;
                }

                double optionPriority = currentPriority + optionPriorityDifference;
                double optionTotalPriority = optionPriority + optionHeuristic;

                frontier.Enqueue(option, optionTotalPriority);
                discovered.Add(option, optionHeuristic);
            }
        }

        result = default;
        return false;
    }

    public static bool TryDepthSearch<T>(T startingValue, Func<T, bool> terminateCondition, out T? result) where T : ISearchState
    {
        throw new NotImplementedException();
    }

    public static bool TryWeightedDepthSearch<T>(T startingValue, Func<T, bool> terminateCondition, Func<T, float> heuristic, out T? result) where T : ISearchState
    {
        throw new NotImplementedException();
    }

}