﻿namespace Math.Strings;

public sealed class Automaton<T>
{
    private readonly T initialState;
    private readonly Func<T, char, T> transitions;
    private readonly IReadOnlySet<T> finalStates;

    public Automaton(T initialState, Func<T, char, T> transitions, IReadOnlySet<T> finalStates)
    {
        this.initialState = initialState;
        this.transitions = transitions;
        this.finalStates = finalStates;
    }

    public IEnumerable<T> Read(string s)
    {
        T frontier = initialState;

        for (int i = 0; i < s.Length; i++)
        {
            yield return frontier;
            frontier = transitions.Invoke(frontier, s[i]);
        }

        if (!finalStates.Contains(frontier))
        {
            throw new ArgumentException("The automaton ended on the wrong state.");
        }

        yield return frontier;
    }

    public IEnumerable<Token<T>> ReadTokens(string s)
    {
        T frontier = initialState;

        for(int i = 0; i < s.Length; i++)
        {
            yield return new Token<T>(frontier, s[i], i);
            frontier = transitions.Invoke(frontier, s[i]);
        }

        if(!finalStates.Contains(frontier))
        {
            throw new ArgumentException("The automaton ended on the wrong state.");
        }

        yield return new Token<T>(frontier, s[^1], ^1);
    }
}

public record Token<T>(T State, char Lexeme, Index Index);