using System;
using System.Collections.Generic;
using System.Linq;
using GreenFluxAssignment.Domain.Entities;
using GreenFluxAssignment.Domain.Exceptions;
using GreenFluxAssignment.Domain.ValueObjects;

namespace GreenFluxAssignment.Domain.Services
{
    public class ConnectorRemovalSuggestionService
    {
        private readonly IReadOnlyList<ConnectorRemovalSuggestion> _allConnectorSuggestions;
        private readonly Stack<ConnectorRemovalSuggestion> _usedConnectors;
        private readonly IList<IList<ConnectorRemovalSuggestion>> _suggestions;

        private ConnectorRemovalSuggestionService(IReadOnlyList<ConnectorRemovalSuggestion> allConnectorSuggestions)
        {
            _allConnectorSuggestions = allConnectorSuggestions;
            _usedConnectors = new Stack<ConnectorRemovalSuggestion>();
            _suggestions = new List<IList<ConnectorRemovalSuggestion>>();
        }

        public static IList<IList<ConnectorRemovalSuggestion>> SuggestConnectors(Group group, decimal excessiveCurrent)
        {
            if (group is null)
            {
                throw new InvalidArgumentException(nameof(group), "Group should not be null.");
            }

            if (excessiveCurrent <= 0)
            {
                throw new InvalidArgumentException(nameof(excessiveCurrent), "Excessive current should be greater than 0.");
            }

            var connectors = group.Stations
                .SelectMany(s => s.Value.Connectors.Values, (s, c) => (station: s.Value, connector: c))
                .Select(sc => new ConnectorRemovalSuggestion(sc.station.Id, sc.connector.Id, sc.connector.MaxCurrent))
                .OrderByDescending(crs => crs.ConnectorCurrent)
                .ToList();

            var suggestionEngine = new ConnectorRemovalSuggestionService(connectors);

            suggestionEngine.BuildSuggestions(excessiveCurrent);

            var suggestionGroup = suggestionEngine._suggestions
                .GroupBy(s => s.Count)
                .OrderBy(g => g.Key)
                .FirstOrDefault();

            return suggestionGroup == null
                ? Array.Empty<IList<ConnectorRemovalSuggestion>>()
                : suggestionGroup.ToList() as IList<IList<ConnectorRemovalSuggestion>>;
        }

        private void BuildSuggestions(decimal current, int index = 0)
        {
            if (current == 0)
            {
                _suggestions.Add(new List<ConnectorRemovalSuggestion>(_usedConnectors));
                return;
            }

            if (index >= _allConnectorSuggestions.Count || HasSuggestionBetterThanCurrent())
            {
                return;
            }

            var connector = _allConnectorSuggestions[index];
            if (connector.ConnectorCurrent <= current)
            {
                _usedConnectors.Push(connector);
                BuildSuggestions(current - connector.ConnectorCurrent, index + 1);
                _usedConnectors.Pop();
            }

            BuildSuggestions(current, index + 1);
        }

        private bool HasSuggestionBetterThanCurrent()
        {
            return _suggestions.Count > 0 && _suggestions[0].Count < _usedConnectors.Count;
        }
    }
}
